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
	public class CategoryData : ICategoryData
	{
		private readonly AppDbContext _context;
		public CategoryData(AppDbContext context)
		{
			_context = context;
		}
		public async Task<bool> Delete(int id)
		{
			try
			{
				var category = await GetById(id);
				_context.Categories.Remove(category);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<IEnumerable<Category>> GetAll()
		{
			var categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
			return categories;
		}

		public async Task<Category> GetById(int id)
		{
			try
			{
				var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
				if (category == null)
				{
					throw new Exception("Category not found");
				}
				return category;
			}
			catch (Exception)
			{
				throw new Exception("Category not found");
			}
		}

		public async Task<IEnumerable<Category>> GetByName(string name)
		{
			try
			{
				var categories = await _context.Categories.Where(c => c.CategoryName.Contains(name)).ToListAsync();
				if (categories == null)
				{
					throw new Exception("Category not found");
				}
				return categories;
			}
			catch (Exception)
			{
				throw new Exception("Category not found");
			}
		}

		public async Task<int> GetCountCategories(string name)
		{
			try
			{
				var result = await _context.Categories.Where(c => c.CategoryName.Contains(name)).CountAsync();
				return result;
			}
			catch (Exception)
			{
				throw new Exception("Category not found");
			}

		}

		public async Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string name)
		{
			try
			{
				var query = _context.Categories
					.Where(c => c.CategoryName.Contains(name))
					.OrderBy(c => c.CategoryName)
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize);

				return await query.ToListAsync();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Category> Insert(Category entity)
		{
			try
			{
				await _context.Categories.AddAsync(entity);
				await _context.SaveChangesAsync();
				return entity;
			}
			catch (Exception)
			{
				throw new Exception("Insert data failed");
			}
		}

		public Task<int> InsertWithIdentity(Category category)
		{
			throw new NotImplementedException();
		}

		public async Task<Category> Update(Category entity)
		{
			try
			{
				var category = await GetById(entity.CategoryId);
				if (category == null)
				{
					throw new Exception("Category not found");
				}
				category.CategoryName = entity.CategoryName;
				await _context.SaveChangesAsync();
				return category;
			}
			catch (Exception)
			{
				throw new Exception("Update data failed");
			}
		}
	}
}
