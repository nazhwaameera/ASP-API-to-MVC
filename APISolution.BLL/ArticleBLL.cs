using AutoMapper;
using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
using APISolution.Data.Interfaces;
using APISolution.Domain;

namespace APISolution.BLL
{
	public class ArticleBLL : IArticleBLL
	{
		private readonly IArticleData _articleData;
		private readonly IMapper _mapper;
		public ArticleBLL(IArticleData articleData, IMapper mapper)
		{
			_articleData = articleData;
			_mapper = mapper;
		}
		public Task<bool> Delete(int id)
		{
			var result = _articleData.Delete(id);
			return result;
		}

		public async Task<IEnumerable<ArticleDTO>> GetArticleByCategory(int categoryId)
		{
			var articles = await _articleData.GetArticleByCategory(categoryId);
			var articlesDTO = _mapper.Map<IEnumerable<ArticleDTO>>(articles);
			return articlesDTO;
		}

		public async Task<ArticleDTO> GetArticleById(int id)
		{
			var article = await _articleData.GetById(id);
			var articleDTO = _mapper.Map<ArticleDTO>(article);
			return articleDTO;
		}

		public async Task<IEnumerable<ArticleDTO>> GetArticleWithCategory()
		{
			var articles = await _articleData.GetArticleWithCategory();
			var articlesDTO = _mapper.Map<IEnumerable<ArticleDTO>>(articles);
			return articlesDTO;
		}

		public async Task<int> GetCountArticles()
		{
			var result = await _articleData.GetCountArticles();
			return result;
		}

		public async Task<IEnumerable<ArticleDTO>> GetWithPaging(int categoryId, int pageNumber, int pageSize)
		{
			var articles = await _articleData.GetWithPaging(categoryId, pageNumber, pageSize);
			var articlesDTO = _mapper.Map<IEnumerable<ArticleDTO>>(articles);
			return articlesDTO;
		}

		public async Task<ArticleDTO> Insert(ArticleCreateDTO article)
		{
			var articleDomain =  _mapper.Map<Article>(article);
			var result = await _articleData.Insert(articleDomain);
			var articleDTO = _mapper.Map<ArticleDTO>(result);
			return articleDTO;
		}

		public async Task<int> InsertWithIdentity(ArticleCreateDTO article)
		{
			var articleDomain = _mapper.Map<Article>(article);
			var result = await _articleData.InsertWithIdentity(articleDomain);
			return result;
		}

		public async Task<ArticleDTO> Update(ArticleUpdateDTO article)
		{
			var articleDomain = _mapper.Map<Article>(article);
			var result = await _articleData.Update(articleDomain);
			var articleDTO = _mapper.Map<ArticleDTO>(result);
			return articleDTO;
		}
	}
}
