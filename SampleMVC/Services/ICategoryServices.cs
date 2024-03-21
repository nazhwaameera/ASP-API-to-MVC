﻿using APISolution.BLL.DTOs;

namespace SampleMVC.Services
{
	public interface ICategoryServices
	{
		Task<IEnumerable<CategoryDTO>> GetAll();
		Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber, int pageSize, string name = "");
		Task<CategoryDTO> GetById(int id);
		Task<CategoryDTO> Insert(CategoryCreateDTO categoryCreateDTO);
		Task Update(int id, CategoryUpdateDTO categoryUpdateDTO);
		Task<int> GetCountCategories(string name = "");
        Task Delete(int id);
	}
}
