using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ITagRepository _tagRepository;

    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
    {
        _logger = logger;
        _blogPostRepository = blogPostRepository;
        _tagRepository = tagRepository;
    }

    public async Task<IActionResult> Index()
    {
        var blogs = await _blogPostRepository.GetAllAsync();

        var tags = await _tagRepository.GetAllAsync();

        var homeVM = new HomeViewModel{ BlogPosts = blogs, Tags = tags };

        return View(homeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
