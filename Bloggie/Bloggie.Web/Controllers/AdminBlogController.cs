using Azure;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers;

public class AdminBlogController : Controller
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogPostRepository _blogPostRepository;

    public AdminBlogController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
    {
        _tagRepository = tagRepository;
        _blogPostRepository = blogPostRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var tags = await _tagRepository.GetAllAsync();

        var model = new AddBlogPostRequest
        {
            Tags = tags.Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() })
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
    {
        var blog = new BlogPost
        {
            Heading = addBlogPostRequest.Heading,
            PageTitle = addBlogPostRequest.PageTitle,
            Content = addBlogPostRequest.Content,
            ShortDescription = addBlogPostRequest.ShortDescription,
            FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
            UrlHandle = addBlogPostRequest.UrlHandle,
            PublishDate = addBlogPostRequest.PublishDate,
            Author = addBlogPostRequest.Author,
            Visible = addBlogPostRequest.Visible,
        };

        var selectedTags = new List<Tag>();
        foreach (var tagId in addBlogPostRequest.SelectedTags)
        {
            var selectedTagId = Guid.Parse(tagId);
            var existingTag = await _tagRepository.GetAsync(selectedTagId);

            if (existingTag != null)
            {
                selectedTags.Add(existingTag);
            }
        }

        blog.Tags = selectedTags;

        await _blogPostRepository.AddAsync(blog);

        return RedirectToAction("Add");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();
        return View(blogPosts);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var blogPost = await _blogPostRepository.GetAsync(id);
        var tags = await _tagRepository.GetAllAsync();

        if (blogPost != null)
        {
            var editBlogPostRequest = new EditBlogPostRequest
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishDate = blogPost.PublishDate,
                Author = blogPost.Author,
                Visible = blogPost.Visible,
                Tags = tags.Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }),
                SelectedTags = blogPost.Tags.Select(t => t.Id.ToString()).ToArray()
            };
            return View(editBlogPostRequest);
        }
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPost)
    {
        var blogPost = new BlogPost
        {
            Id = editBlogPost.Id,
            Heading = editBlogPost.Heading,
            PageTitle = editBlogPost.PageTitle,
            Content = editBlogPost.Content,
            ShortDescription = editBlogPost.ShortDescription,
            FeaturedImageUrl = editBlogPost.FeaturedImageUrl,
            UrlHandle = editBlogPost.UrlHandle,
            PublishDate = editBlogPost.PublishDate,
            Author = editBlogPost.Author,
            Visible = editBlogPost.Visible,
        };

        var selectedTags = new List<Tag>();
        foreach (var tagId in editBlogPost.SelectedTags)
        {
            if (Guid.TryParse(tagId, out var tag))
            {
                var foundTag = await _tagRepository.GetAsync(tag);

                if (foundTag != null)
                {
                    selectedTags.Add(foundTag);
                }
            }
        }

        blogPost.Tags = selectedTags;

        var updatedBlog = await _blogPostRepository.UpdateAsync(blogPost);

        if (updatedBlog != null)
        {
            return RedirectToAction("Edit");
        }

        return RedirectToAction("Edit");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPost)
    {
        var deletedBlogPost = await _blogPostRepository.DeleteAsync(editBlogPost.Id);

        if (deletedBlogPost != null) 
        {
            return RedirectToAction("List");
        }

        return RedirectToAction("Edit", new { id = editBlogPost.Id });
    }
}
