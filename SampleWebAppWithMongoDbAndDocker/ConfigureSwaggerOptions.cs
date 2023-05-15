using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebAppWithMongoDbAndDocker
{
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider provider;
		private readonly IWebHostEnvironment webHostEnvironment;

		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IWebHostEnvironment webHostEnvironment)
		{
			this.provider = provider;
			this.webHostEnvironment = webHostEnvironment;
		}

		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description in provider.ApiVersionDescriptions)
			{
				var apiVersion = description.ApiVersion.ToString();

				// configure to add general info about program in swagger UI
				options.SwaggerDoc(description.GroupName,
					new OpenApiInfo
					{
						Version = apiVersion,
						Title = $"{webHostEnvironment.EnvironmentName} {apiVersion}",
						Description = "Sample web api app with mongoDB and docker",
						//TermsOfService = new Uri("https://example.com/terms"),
						//Contact = new OpenApiContact
						//{
						//	Name = "Example Contact",
						//	Email = "example@gmail.com",
						//	Url = new Uri("https://example.com/contact")
						//},
						//License = new OpenApiLicense
						//{
						//	Name = "Example License",
						//	Url = new Uri("https://example.com/license")
						//}
					});
			}
		}
	}
}
