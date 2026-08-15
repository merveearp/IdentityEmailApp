namespace IdentityEmailApp.Services.Abstract
{
    public interface ITranslateService
    {
        Task<string> TranslateAsync(
            string text,
            string targetLanguage ="tr",
            string sourceLanguage="auto");
    }
}
