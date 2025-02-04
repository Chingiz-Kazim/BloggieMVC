using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Bloggie.Web.Controllers;

public class AdminTagsController : Controller
{
    private  BloggieDbContext _bloggieDbContext;

    public AdminTagsController(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName,
        };

        await _bloggieDbContext.Tags.AddAsync(tag);
        await _bloggieDbContext.SaveChangesAsync();

        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List() 
    {
        var tags = await _bloggieDbContext.Tags.ToListAsync();

        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(i => i.Id == id);

        if (tag != null)
        {
            var editTagRequest = new EditTagRequest
            {
                Id=tag.Id,
                Name=tag.Name,
                DisplayName=tag.DisplayName,
            };
            return View(editTagRequest);
        }
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName,
        };

        var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);
        
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;

            await _bloggieDbContext.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var existingTag = await _bloggieDbContext.Tags.FindAsync(editTagRequest.Id);

        if (existingTag != null)
        {
            _bloggieDbContext.Tags.Remove(existingTag);
            await _bloggieDbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}
