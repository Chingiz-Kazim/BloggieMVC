using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;

    public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository)
    {
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
        var blogPostLikeVM = new BlogDetailsViewModel();

        if (blogPost != null)
        {
            var likes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);

            blogPostLikeVM = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                PublishDate = blogPost.PublishDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = urlHandle,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                TotalLikes = likes,
            };
        }
        return View(blogPostLikeVM);
    }
}
