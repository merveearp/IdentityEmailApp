using IdentityEmailApp.DTOs.NewsDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface INewsService
    {
        Task<List<ResultLatestOfNewDto.Item>> GetCurrentNewsAsync();
        Task<List<ResultLatestOfNewDto.Item>> GetCategoryByNewsAsync(string category);
        Task<List<ResultLatestOfNewDto.Item>> GetLocalNewsAsync();
        Task<List<ResultLatestOfNewDto.Subnew>> GetNewsAsync();
    }
}
