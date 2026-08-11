namespace IdentityEmailApp.Services.Abstract
{
    public interface ITranslateService
    {
        Task<string> TranlateAsync(string text);
    }
}
