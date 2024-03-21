using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
using FluentValidation;
using APISolution.Models;

namespace APISolution.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArticlesController : ControllerBase
	{
		private readonly IArticleBLL _articleBLL;
		private readonly IValidator<ArticleCreateDTO> _validatorArticleCreateDto;
		private readonly IValidator<ArticleUpdateDTO> _validatorArticleUpdateDTO;

		public ArticlesController(IArticleBLL articleBLL, IValidator<ArticleCreateDTO> validatorArticleCreateDto,
			IValidator<ArticleUpdateDTO> validatorArticleUpdateDTO)
		{
			_articleBLL = articleBLL;
			_validatorArticleCreateDto = validatorArticleCreateDto;
			_validatorArticleUpdateDTO = validatorArticleUpdateDTO;
		}

		[HttpGet]
		public async Task<IEnumerable<ArticleDTO>> Get()
		{
			var results = await _articleBLL.GetArticleWithCategory();
			return results;
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<ArticleDTO>> GetById(int id)
		{
			var result = await _articleBLL.GetArticleById(id);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}
		[HttpGet("/api/Articles/bycategory/{categoryId}")]
		public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticlebyCategory(int categoryId)
		{
			var result = await _articleBLL.GetArticleByCategory(categoryId);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}
		[HttpPost]
		public async Task<ActionResult<ArticleDTO>> Post(ArticleCreateDTO articleCreateDTO)
		{
			if (articleCreateDTO == null)
			{
				return BadRequest();
			}
			try
			{
				var data = await _articleBLL.Insert(articleCreateDTO);
				return Ok(new { Message = "Insert data success", Result = data });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
			}
		}
		[HttpPost("upload")]
		public async Task<IActionResult> Post([FromForm] ArticleWithFile articleWithFile)
		{
			if (articleWithFile.file == null || articleWithFile.file.Length == 0)
			{
				return BadRequest("File is required");
			}
			var newName = $"{Guid.NewGuid()}_{articleWithFile.file.FileName}";
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newName);
			using (var stream = new FileStream(path, FileMode.Create))
			{
				await articleWithFile.file.CopyToAsync(stream);
			}

			var articleCreateDTO = new ArticleCreateDTO
			{
				CategoryID = articleWithFile.CategoryId,
				Title = articleWithFile.Title,
				Details = articleWithFile.Details,
				IsApproved = articleWithFile.IsApproved,
				Pic = newName
			};

			var article = await _articleBLL.Insert(articleCreateDTO);

			return CreatedAtAction(nameof(Get), new { id = article.ArticleID }, article);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var result = await _articleBLL.Delete(id);
			if (result)
			{
				return Ok(new { Message = "Delete data success", Result = result });
			}
			else
			{
				return NotFound();
			}
		}
		[HttpPut("{id}")]
		public async Task<ActionResult<ArticleDTO>> Update(int id, ArticleUpdateDTO articleUpdateDTO)
		{
			try
			{
				var oldData = await _articleBLL.GetArticleById(id);
				if (oldData == null)
				{
					return NotFound();
				}
				var data = await _articleBLL.Update(articleUpdateDTO);
				return Ok(new { Message = "Update data success", Result = data });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
			}
		}

		[HttpGet("/api/Articles/count")]
		public async Task<ActionResult<int>> GetCountArticles()
		{
			var result = await _articleBLL.GetCountArticles();
			return Ok(result);
		}
		[HttpGet("/api/Articles/paging/{categoryId}/{pageNumber}/{pageSize}")]
		public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetWithPaging(int categoryId, int pageNumber, int pageSize) 
		{
			var result = await _articleBLL.GetWithPaging(categoryId, pageNumber, pageSize);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpPost("/api/Articles/insertwithidentity")]
		public async Task<ActionResult<int>> InsertWithIdentity(ArticleCreateDTO articleCreateDTO)
		{
			if (articleCreateDTO == null)
			{
				return BadRequest();
			}
			try
			{
				var data = await _articleBLL.InsertWithIdentity(articleCreateDTO);
				return Ok(new { Message = "Insert data success", Result = data });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
			}
		}
	}
}
