using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Bloggie.Web.Models.ViewModels;

public class AddBlogPostRequest
{
    public string Heading { get; set; }
    public string PageTitle { get; set; }
    public string Content { get; set; }
    public string ShortDescription { get; set; }
    public string FeaturedImageUrl { get; set; }
    public string UrlHandle { get; set; }
    public DateTime PublishDate { get; set; }
    public string Author { get; set; }
    public bool Visible { get; set; }
    [DisplayName("Tags")]
    public IEnumerable<SelectListItem> Tags { get; set; }
    public string[] SelectedTags { get; set; } = Array.Empty<string>();
}
