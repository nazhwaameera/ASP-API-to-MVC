using AutoMapper;
using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
using APISolution.Data.Interfaces;
using APISolution.Domain;

namespace APISolution.BLL
{
	public class CategoryBLL : ICategoryBLL
	{
		private readonly ICategoryData _categoryData;
		private readonly IMapper _mapper;

		public CategoryBLL(ICategoryData categoryData, IMapper mapper)
		{
			_categoryData = categoryData;
			_mapper = mapper;
		}
		public async Task<bool> Delete(int id)
		{
			var result = await _categoryData.Delete(id);
			return result;
		}

		public async Task<IEnumerable<CategoryDTO>> GetAll()
		{
			var categories = await _categoryData.GetAll();
			var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
			return categoriesDTO;
		}

		public async Task<CategoryDTO> GetById(int id)
		{
			var category = await _categoryData.GetById(id);
			var categoryDTO = _mapper.Map<CategoryDTO>(category);
			return categoryDTO;
		}

		public async Task<IEnumerable<CategoryDTO>> GetByName(string name)
		{
			var categories = await _categoryData.GetByName(name);
			var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
			return categoriesDTO;
		}

		public async Task<int> GetCountCategories(string name)
		{
			int result = await _categoryData.GetCountCategories(name);
			return result;
		}

		public async Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name)
		{
			var categories = await _categoryData.GetWithPaging(pageNumber, pageSize, name);
			var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
			return categoriesDTO;
		}

		public async Task<CategoryDTO> Insert(CategoryCreateDTO entity)
		{
			var category = _mapper.Map<Category>(entity);
			var categoryDomain = await _categoryData.Insert(category);
			var categoryDTO = _mapper.Map<CategoryDTO>(categoryDomain); // <1
			return categoryDTO;
		}

		public async Task<CategoryDTO> Update(CategoryUpdateDTO entity)
		{
			var category = _mapper.Map<Category>(entity);
			var categoryDomain = await _categoryData.Update(category);
			var categoryDTO = _mapper.Map<CategoryDTO>(categoryDomain);
			return categoryDTO;
		}
	}
}
