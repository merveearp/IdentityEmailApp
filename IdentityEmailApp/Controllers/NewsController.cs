using IdentityEmailApp.Entities;
using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;


        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }
        public IActionResult Layout()
        {
            return View();
        }
        public async Task<IActionResult> GetCategoryByNews(string category)
        {
            var searchCategory = string.IsNullOrWhiteSpace(category)
                ? "latest"
                : category.ToLower();

            var values =
                await _newsService.GetCategoryByNewsAsync(searchCategory);

            ViewBag.Category = searchCategory;

            return View(values);
        }



       
       
    }
}
