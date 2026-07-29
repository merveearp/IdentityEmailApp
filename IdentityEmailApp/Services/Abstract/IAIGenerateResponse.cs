using IdentityEmailApp.DTOs.AIDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface IAIGenerateResponse
    {
        Task<string> GenerateResponseAsync(int messageId);
        Task<SpamAnalysisDto> AnalyzeSpamAsync(int messageId);
    }
}
