using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Controllers
   
{
    public class TranslateController : Controller
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITranslateService _translateService;

        public TranslateController(EmailContext emailContext, UserManager<AppUser> userManager, ITranslateService translateService)
        {
            _emailContext = emailContext;
            _userManager = userManager;
            _translateService = translateService;
        }

        public IActionResult Layout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Translate(
            string text ,
            string targetLanguage ="tr",
            string sourceLanguage ="auto"
            )
        {
            var user = await _userManager.GetUserAsync(User);

            if(string.IsNullOrEmpty(text))
            {
                return Json(new
                {
                    success =false,
                    message = "Lütfen Çevrilcek metni giriniz ."
                    
                });
            }

            try
            {
                var translatedText = await _translateService.TranslateAsync(text, targetLanguage, sourceLanguage);

                var translationHistory = new TranslationHistory
                {
                    SourceText = text.Trim(),
                    TranslatedText = translatedText,
                    SourceLanguage = sourceLanguage,
                    TargetLanguage = targetLanguage,
                    CreatedDate = DateTime.Now,
                    IsSaved = false,
                    AppUserId = user.Id

                };

                _emailContext.TranslationHistories.Add(translationHistory);
                await _emailContext.SaveChangesAsync();


                return Json(new
                {
                    success = true,
                    translatedText,
                    translationId = translationHistory.TranslationHistoryId
                });

            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Çeviri kayıt edilirken bir hata oluştu."
                });
            }

        }


        [HttpGet]
        public async Task<IActionResult> TranslationHistory()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var translations = await _emailContext
                .TranslationHistories
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(translations);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSaved(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var translation = await _emailContext
                .TranslationHistories
                .FirstOrDefaultAsync(x =>
                    x.TranslationHistoryId == id &&
                    x.AppUserId == user.Id);

            if (translation == null)
            {
                TempData["ErrorMessage"] =
                    "Çeviri kaydı bulunamadı.";

                return RedirectToAction(nameof(TranslationHistory));
            }

            translation.IsSaved = !translation.IsSaved;

            await _emailContext.SaveChangesAsync();

            return RedirectToAction(nameof(TranslationHistory));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromHistory(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var translation = await _emailContext
                .TranslationHistories
                .FirstOrDefaultAsync(x =>
                    x.TranslationHistoryId == id &&
                    x.AppUserId == user.Id);

            if (translation == null)
            {
                TempData["ErrorMessage"] =
                    "Çeviri kaydı bulunamadı.";

                return RedirectToAction(nameof(TranslationHistory));
            }

            _emailContext.Remove(translation);

            await _emailContext.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Çeviri geçmişten kaldırıldı.";

            return RedirectToAction(nameof(TranslationHistory));
        }

        public async Task<IActionResult> SavedTranslations()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var translations = await _emailContext
                .TranslationHistories
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id && x.IsSaved)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(translations);
        }


    }
}
