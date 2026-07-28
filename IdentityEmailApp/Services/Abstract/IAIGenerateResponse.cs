using IdentityEmailApp.DTOs.AIDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface IAIGenerateResponse
    {
        Task<string> GenerateResponseAsync(int messageId);
    }
}
