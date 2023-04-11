using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.API.Data;
using ShopOnline.API.Data.Repository;
using ShopOnline.API.Data.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using ShopOnline.API.Entities;
using ShopOnline.Api;
using Stripe;
using ShopOnline.Api.Data.Repository.Contracts;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using ShopOnline.Api.Data.Repository;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,

            });
            option.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("My top secret key")),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        builder.Services.AddDbContextPool<ShopOlineDbContext>(option
			=> option.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));
	//	builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
	//.AddEntityFrameworkStores<ShopOlineDbContext>();
		builder.Services.AddScoped<IProductRepository, ProductRepository>();
		builder.Services.AddScoped<ICartRepository, CartRepository>();
		builder.Services.AddScoped<IServiceProvider, ServiceProvider>();
		builder.Services.AddScoped<IUserRepository, UserRepository>();
		builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
		StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSetting:SecretKey").Get<string>();
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
		app.UseCors(policy =>
		{
			policy.WithOrigins("http://localhost:7125", "https://localhost:7125")
			.AllowAnyMethod()
			.AllowAnyHeader();
		});

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}

