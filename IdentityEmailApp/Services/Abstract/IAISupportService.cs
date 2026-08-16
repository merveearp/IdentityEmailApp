namespace IdentityEmailApp.Services.Abstract
{
    public interface IAISupportService
    {
        Task<string> GetSupportResponseAsync(string question);
    }
}
