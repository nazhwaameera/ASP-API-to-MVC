using APISolution.Domain;
using System.Collections.Generic;

namespace APISolution.Data.Interfaces
{
    public interface IArticleData : ICrud<Article>
    {
        Task<IEnumerable<Article>> GetArticleWithCategory();
        Task<IEnumerable<Article>> GetArticleByCategory(int categoryId);
        Task<IEnumerable<Article>> GetWithPaging(int categoryId, int psageNumber, int pageSize);

        Task<int> GetCountArticles();
        Task<int> InsertWithIdentity(Article article);

        Task<Article> InsertArticleWithCategory(Article article);
    }
}
