﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services;

namespace NetBlog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public AdminTagsController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            Tag tag = _mapper.Map<Tag>(addTagRequest);
            await _tagService.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await _tagService.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagService.GetAsync(id);

            if (tag != null)
            {
                EditTagRequest editTagRequest = _mapper.Map<EditTagRequest>(tag);
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        public  async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            Tag tag = _mapper.Map<Tag>(editTagRequest);
            var updatedTag = await _tagService.UpdateAsync(tag);
            if (updatedTag != null)
            {

            }
            else
            {

            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await _tagService.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {

                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}