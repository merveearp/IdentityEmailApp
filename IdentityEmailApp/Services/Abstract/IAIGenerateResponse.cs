namespace IdentityEmailApp.Services.Abstract
{
    public interface IAIGenerateResponse
    {
        Task<string> ResponseAsync(int id);
    }
}
