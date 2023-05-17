using Microsoft.AspNetCore.Mvc;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiController]
	public abstract class ApiBaseController : ControllerBase
	{
		public ApiBaseController()
		{

		}

		protected IActionResult JsonActionResult()
		{
			return new JsonActionResult<object>();
		}

		protected IActionResult JsonActionResult<T>(T model) where T : class
		{
			return new JsonActionResult<T>(model);
		}

		protected IActionResult JsonActionResultError(string[] errorMessages, int statusCode = StatusCodes.Status400BadRequest)
		{
			return new JsonActionResult<object>(statusCode, errorMessages);
		}

		protected IActionResult JsonActionResult<T>(T? model = default, int statusCode = StatusCodes.Status200OK, string[]? errorMessages = default) where T : class
		{
			return new JsonActionResult<T>(model!, statusCode, errorMessages ?? new string[0]);
		}
	}
}
