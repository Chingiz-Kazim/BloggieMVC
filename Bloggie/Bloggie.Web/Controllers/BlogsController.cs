using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;
    private readonly IBlogPostCommentRepository _blogPostCommentRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public BlogsController(IBlogPostRepository blogPostRepository, 
        IBlogPostLikeRepository blogPostLikeRepository, 
        IBlogPostCommentRepository blogPostCommentRepository, 
        SignInManager<IdentityUser> signInManager, 
        UserManager<IdentityUser> userManager)
    {
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
        _blogPostCommentRepository = blogPostCommentRepository;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var liked = false;
        var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
        var blogPostLikeVM = new BlogDetailsViewModel();

        if (blogPost != null)
        {
            var likes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);
            var comments = await _blogPostCommentRepository.GetCommentsForBlog(blogPost.Id);
            var commentsVM = new List<BlogComment>();

            foreach (var comment in comments) 
            {
                commentsVM.Add(new BlogComment
                {
                    Description = comment.Description,
                    Author = (await _userManager.FindByIdAsync(comment.UserId.ToString())).UserName,
                    DateAdded = comment.DateAdded
                });
            }

            if (_signInManager.IsSignedIn(User))
            {
                var likesBP = await _blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                var userId = _userManager.GetUserId(User);

                if (userId != null)
                {
                    var likeUser = likesBP.FirstOrDefault(l => l.UserId == Guid.Parse(userId));
                    liked = likeUser != null;
                }
            }


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
                Liked = liked,
                Comments = commentsVM
            };
        }
        return View(blogPostLikeVM);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsVM)
    {
        if (_signInManager.IsSignedIn(User))
        {
            var domainModel = new BlogPostComment
            {
                BlogPostId = blogDetailsVM.Id,
                Description = blogDetailsVM.CommentDescription,
                UserId = Guid.Parse(_userManager.GetUserId(User)),
                DateAdded = DateTime.Now,
            };
            await _blogPostCommentRepository.AddAsync(domainModel);

            return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsVM.UrlHandle});
        }

        return View();
    }
}
