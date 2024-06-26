﻿using FluentValidation;

namespace APISolution.BLL.DTOs.Validation
{
	public class CategoryUpdateDTOValidator : AbstractValidator<CategoryUpdateDTO>	
	{
		public CategoryUpdateDTOValidator()
		{
			RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name harus diisi");
		}
	}
}
