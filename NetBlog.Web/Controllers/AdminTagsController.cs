using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services;

namespace NetBlog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["FailAlertMsg"] = "Failed To Add New Tag";
                return View();
            }

            Tag tag = _mapper.Map<Tag>(addTagRequest);
            await _tagService.AddAsync(tag);

            TempData["SuccessAlertMsg"] = "Tag Added Successfully!";

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(
            string? searchQuery, 
            string? sortBy, 
            string? sortDirection,
            int pageSize = 5,
            int pageNumber = 1)
        {
            var totalRecords = await _tagService.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
                pageNumber--;

            if (pageNumber < 1)
                pageNumber++;

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            var tags = await _tagService.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

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
            if (!ModelState.IsValid)
            {
                TempData["FailAlertMsg"] = "Failed To Edit Tag";
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

            Tag tag = _mapper.Map<Tag>(editTagRequest);
            var updatedTag = await _tagService.UpdateAsync(tag);
            if (updatedTag != null)
            {
                TempData["SuccessAlertMsg"] = "Tag Edited Successfully!";
                return RedirectToAction("List");
            }
            TempData["FailAlertMsg"] = "Failed To Edit Tag";
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
