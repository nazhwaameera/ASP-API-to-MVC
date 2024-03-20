using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APISolution.Data.Interfaces;
using APISolution.Domain;
using Microsoft.EntityFrameworkCore;

namespace APISolution.Data
{
	public class ArticleData : IArticleData
	{
		private readonly AppDbContext _context;
		public ArticleData(AppDbContext context)
		{
			_context = context;
		}
		public async Task<bool> Delete(int id)
		{
			try
			{
				var article = await GetById(id);
				_context.Articles.Remove(article);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IEnumerable<Article>> GetAll()
		{
			var articles = await _context.Articles.OrderBy(a => a.Title).ToListAsync();
			return articles;
		}

		public async Task<IEnumerable<Article>> GetArticleByCategory(int categoryId)
		{
			try
			{
				var articles = await _context.Articles.Where(a => a.CategoryId == categoryId).OrderBy(a => a.Title).ToListAsync();
				if (articles == null)
				{
					throw new Exception("Articles not found");
				}
				return articles;
			}
			catch (Exception)
			{
				throw new Exception("Articles not found");
			}
		}

		public async Task<IEnumerable<Article>> GetArticleWithCategory()
		{
			try
			{
				var articles = await _context.Articles.Include(a => a.Category).ToListAsync();
				return articles;
			}
			catch (Exception)
			{
				throw new Exception("There is no article.");
			}
		}

		public async Task<Article> GetById(int id)
		{
			try
			{
				var article = await _context.Articles.FirstOrDefaultAsync(a => a.ArticleId == id);
				if (article == null)
				{
					throw new Exception("Article not found");
				}
				return article;
			}
			catch (Exception)
			{
				throw new Exception("Article not found");
			}
		}

		public async Task<int> GetCountArticles()
		{
			var result = await _context.Articles.CountAsync();
			return result;
		}

		public async Task<IEnumerable<Article>> GetWithPaging(int categoryId, int pageNumber, int pageSize)
		{
			try
			{
				var query = _context.Articles
					.Where(a => a.CategoryId == categoryId)
					.OrderBy(a => a.Title)
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize);

				return await query.ToListAsync();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public async Task<Article> Insert(Article entity)
		{
			try
			{
				await _context.Articles.AddAsync(entity);
				await _context.SaveChangesAsync();
				return entity;
			}
			catch (Exception)
			{
				throw new Exception("Insert data failed");
			}
		}

		public async Task<Article> InsertArticleWithCategory(Article article)
		{
			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					// Check if the category exists
					var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == article.Category.CategoryName);

					if (existingCategory == null)
					{
						// If the category doesn't exist, insert it
						_context.Categories.Add(article.Category);
						await _context.SaveChangesAsync();
					}
					else
					{
						// If the category already exists, use its ID
						article.Category.CategoryId = existingCategory.CategoryId;
					}

					// Insert the article
					_context.Articles.Add(article);
					await _context.SaveChangesAsync();

					// Commit the transaction
					await transaction.CommitAsync();

					return article;
				}
				catch (Exception)
				{
					// Rollback the transaction if an error occurs
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		public async Task<int> InsertWithIdentity(Article article)
		{
			await _context.Articles.AddAsync(article);
			await _context.SaveChangesAsync();
			return article.ArticleId;
		}

		public async Task<Article> Update(Article entity)
		{
			try
			{
				var article = await GetById(entity.ArticleId);
				if (article == null)
				{
					throw new Exception("Article not found");
				}
				article.ArticleId = entity.ArticleId;
				article.CategoryId = entity.CategoryId;
				article.Title = entity.Title;
				article.Details = entity.Details;
				article.PublishDate = entity.PublishDate;
				article.IsApproved = entity.IsApproved;
				article.Pic = entity.Pic;
				article.Username = article.Username;

				await _context.SaveChangesAsync();
				return article;
			}
			catch (Exception)
			{
				throw new Exception("Update data failed");
			}
		}
	}
}
