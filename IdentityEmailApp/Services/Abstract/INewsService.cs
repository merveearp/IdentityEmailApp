using IdentityEmailApp.DTOs.NewsDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface INewsService
    {
        Task<List<ResultLatestOfNewDto.Item>> GetCurrentNewsAsync();
    }
}
