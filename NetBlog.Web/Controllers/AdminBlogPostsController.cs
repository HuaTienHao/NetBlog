﻿using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBlog.Web.Models.Domain;
using AutoMapper;

namespace NetBlog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IBlogPostService _blogPostService;
        private readonly IMapper _mapper;

        public AdminBlogPostsController(ITagService tagService, IBlogPostService blogPostService, IMapper mapper)
        {
            _tagService = tagService;
            _blogPostService = blogPostService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagService.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Mapping
            BlogPost blogPost = _mapper.Map<BlogPost>(addBlogPostRequest);
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existedTag = await _tagService.GetAsync(selectedTagIdAsGuid);
                if (existedTag != null)
                {
                    selectedTags.Add(existedTag);
                }
            }
            blogPost.Tags = selectedTags;

            // Add to database
            await _blogPostService.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await _blogPostService.GetAllAsync();

            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await _blogPostService.GetAsync(id);
            var tagsDomainModel = await _tagService.GetAllAsync();

            if (blogPost != null)
            {
                // Mapping
                var model = _mapper.Map<EditBlogPostRequest>(blogPost);
                model.Tags = tagsDomainModel.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                model.SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray();

                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // Mapping
            var blogPostDomainModel = _mapper.Map<BlogPost>(editBlogPostRequest);
            var selectedTags = new List<Tag>();
            foreach (var selectedtTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedtTag, out var tag))
                {
                    var foundTag = await _tagService.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectedTags;
            
            var updatedBlog = await _blogPostService.UpdateAsync(blogPostDomainModel);
            if (updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlog = await _blogPostService.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }
    }
}