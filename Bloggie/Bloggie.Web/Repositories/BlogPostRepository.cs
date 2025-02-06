using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BloggieDbContext _bloggieDbContext;

    public BlogPostRepository(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _bloggieDbContext.BlogPosts.Include(t=> t.Tags).ToListAsync();
    }

    public async Task<BlogPost?> GetAsync(Guid id)
    {
        return await _bloggieDbContext.BlogPosts.Include(t => t.Tags).FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await _bloggieDbContext.BlogPosts.AddAsync(blogPost);
        await _bloggieDbContext.SaveChangesAsync();
        return blogPost;
    }
    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
        var existingBlog = await _bloggieDbContext.BlogPosts.FindAsync(id);

        if (existingBlog != null)
        {
            _bloggieDbContext.BlogPosts.Remove(existingBlog);
            await _bloggieDbContext.SaveChangesAsync();
            return existingBlog;
        }
        return null;
    }
    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingBlog = await _bloggieDbContext.BlogPosts.Include(t => t.Tags).FirstOrDefaultAsync(i => i.Id == blogPost.Id);

        if (existingBlog != null)
        {
            existingBlog.Id = blogPost.Id;
            existingBlog.Heading = blogPost.Heading;
            existingBlog.PageTitle = blogPost.PageTitle;
            existingBlog.Content = blogPost.Content;
            existingBlog.ShortDescription = blogPost.ShortDescription;
            existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingBlog.UrlHandle = blogPost.UrlHandle;
            existingBlog.PublishDate = blogPost.PublishDate;
            existingBlog.Author = blogPost.Author;
            existingBlog.Visible = blogPost.Visible;
            existingBlog.Tags = blogPost.Tags;

            await _bloggieDbContext.SaveChangesAsync();

            return existingBlog;
        }
        return null;
    }
}
