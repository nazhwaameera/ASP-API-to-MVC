using AutoMapper;	
using APISolution.BLL.DTOs;
using APISolution.Domain;

namespace APISolution.BLL.Profiles
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Article, ArticleDTO>().ReverseMap();
			CreateMap<ArticleCreateDTO, Article>();
			CreateMap<ArticleUpdateDTO, Article>();
			CreateMap<Category, CategoryDTO>().ReverseMap();
			CreateMap<CategoryCreateDTO, Category>();
			CreateMap<CategoryUpdateDTO, Category>();
			CreateMap<Role, RoleDTO>().ReverseMap();
			CreateMap<RoleCreateDTO, Role>();
		}
	}
}
