using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SampleWebAppWithMongoDbAndDocker.ViewModels
{
	public class JsonActionResult<T> : IActionResult where T : class
	{
		private JsonResultObject<T> result = new JsonResultObject<T>();

		public JsonActionResult(T data, int statusCode, string[] errorMessages)
		{
			result.StatusCode = statusCode;
			result.ErrorMessages = errorMessages;
			result.DateTime = DateTime.UtcNow;
			result.Body = data;
		}

		public JsonActionResult() : this(null!, StatusCodes.Status204NoContent, new string[0])
		{ }

		public JsonActionResult(T data) : this(data, StatusCodes.Status200OK, new string[0])
		{ }

		public JsonActionResult(string[] errorMessages) : this(null!, StatusCodes.Status400BadRequest, errorMessages)
		{ }

		public JsonActionResult(T data, int statusCode) : this(data, statusCode, new string[0])
		{ }

		public JsonActionResult(T data, string[] errorMessages) : this(data, StatusCodes.Status400BadRequest, errorMessages)
		{ }

		public JsonActionResult(int statusCode, string[] errorMessages) : this(null!, statusCode, errorMessages)
		{ }

		public async Task ExecuteResultAsync(ActionContext context)
		{
			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.StatusCode = result.StatusCode;
			await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(result));
		}
	}
}
