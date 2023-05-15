using Amazon.Util.Internal.PlatformServices;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebAppWithMongoDbAndDocker
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			// Custom services
			string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			string dbName = builder.Configuration.GetConnectionString("DefaultDb");
			builder.Services.AddSingleton(new MongoClient(connection).GetDatabase(dbName));

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'v'VVV");
			builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddCors();
			builder.Services.AddApiVersioning();

			var app = builder.Build();

			string addSwaggerToProduction = Environment.GetEnvironmentVariable("AddSwaggerToProduction")?.ToLower() ?? "false";

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment() || addSwaggerToProduction == "true")
			{
				app.UseSwagger();
				app.UseSwaggerUI(config =>
				{
					var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
					foreach (var description in provider.ApiVersionDescriptions)
					{
						config.SwaggerEndpoint(
							$"/swagger/{description.GroupName}/swagger.json",
							$"{app.Environment.ApplicationName} {description.GroupName.ToUpperInvariant()}");
						config.RoutePrefix = "swagger";
					}
				});
			}

			app.UseHttpsRedirection();

			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

			app.UseAuthorization();

			app.UseApiVersioning();

			app.MapControllers();

			app.Run();
		}
	}
}