using FluentValidation;
using APISolution.BLL;
using APISolution.BLL.Interfaces;
using APISolution.BLL.DTOs.Validation;
using APISolution.Data;
using APISolution.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APISolution.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register DI
builder.Services.AddScoped<ICategoryData, CategoryData>();
builder.Services.AddScoped<ICategoryBLL, CategoryBLL>();
builder.Services.AddScoped<IArticleData, ArticleData>();
builder.Services.AddScoped<IArticleBLL, ArticleBLL>();
builder.Services.AddScoped<IUserBLL, UserBLL>();
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IRoleBLL, RoleBLL>();
builder.Services.AddScoped<IRoleData, RoleData>();

//builder.Services.AddScoped<IUserBLL, UserBLL>();
//builder.Services.AddScoped<IRoleBLL, RoleBLL>();

//automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<CategoryCreateDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryUpdateDTOValidator>();

//jwt token
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//inject db context
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
