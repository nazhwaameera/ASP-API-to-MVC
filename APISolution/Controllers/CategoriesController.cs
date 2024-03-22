using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APISolution.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryBLL _categoryBLL;

		public CategoriesController(ICategoryBLL categoryBLL)
		{
			_categoryBLL = categoryBLL;
		}
		[HttpGet]
		public async Task<IEnumerable<CategoryDTO>> Get()
		{
			var results = await _categoryBLL.GetAll();
			return results;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryDTO>> GetById(int id)
		{
			var result = await _categoryBLL.GetById(id);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpGet("/api/Categories/byname/{name}")]
		public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetByName(string name)
		{
			var result = await _categoryBLL.GetByName(name);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<CategoryDTO>> Post(CategoryCreateDTO categoryCreateDTO)
		{
			if (categoryCreateDTO == null)
			{
				return BadRequest();
			}

			try
			{
				return await _categoryBLL.Insert(categoryCreateDTO);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryUpdateDTO categoryUpdateDTO)
		{
			try
			{
				var oldData = await _categoryBLL.GetById(id);
				if (oldData == null)
				{
					return NotFound();
				}
				var data = await _categoryBLL.Update(categoryUpdateDTO);
				return Ok(new { Message = "Update data success", Result = data });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			}

		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			if (await _categoryBLL.GetById(id) == null)
			{
				return NotFound();
			}

			try
			{
				await _categoryBLL.Delete(id);
				return Ok("Delete data success");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("/api/Categories/paging/byname")]
		public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetWithPaging([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string name = "")
		{
			var result = await _categoryBLL.GetWithPaging(pageNumber, pageSize, name);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpGet("/api/Categories/count/")]
		public async Task<ActionResult<int>> GetCountCategories([FromQuery] string name = "")
		{
			var result = await _categoryBLL.GetCountCategories(name);
			if (result == 0)
			{
				return NotFound();
			}
			return Ok(result);
		}

	}
}
