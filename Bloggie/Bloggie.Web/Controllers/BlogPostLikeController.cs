﻿using Bloggie.Web.Migrations;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostLikeController : ControllerBase
{
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;

    public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
    {
        _blogPostLikeRepository = blogPostLikeRepository;
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
    {
        var model = new BlogPostLike
        {
            BlogPostId = addLikeRequest.BlogPostId,
            UserId = addLikeRequest.UserId,
        };

        await _blogPostLikeRepository.AddLikeForBlog(model);

        return Ok();
    }
}
