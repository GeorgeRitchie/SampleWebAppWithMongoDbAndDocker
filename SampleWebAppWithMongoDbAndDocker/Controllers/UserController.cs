using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiController]
	[ApiVersionNeutral]
	[Route("api/{version:apiVersion}/[controller]/[action]")]
	public class UserController : ControllerBase
	{
		private readonly IMongoCollection<Teacher> teacherCollection;
		private readonly IMongoCollection<Student> studentCollection;

		public UserController(IMongoDatabase db)
		{
			teacherCollection = db.GetCollection<Teacher>("Teachers");
			studentCollection = db.GetCollection<Student>("Students"); ;
		}

		[HttpGet]
		public async Task<IActionResult> LogIn([FromQuery] LogInModel model)
		{
			User user = teacherCollection.Find(p => p.Email == model.Email && p.Password == model.Password).FirstOrDefault();
			
			if (user == null)
				user = studentCollection.Find(p => p.Email == model.Email && p.Password == model.Password).FirstOrDefault();

			if (user != null)
			{
				await Authenticate(model.Email);

				return Ok();
			}

			return BadRequest("Not found user");
		}

		private async Task Authenticate(string userName)
		{
			// создаем один claim
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
			};
			// создаем объект ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			// установка аутентификационных куки
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}

		[Authorize]
		[HttpGet]
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return NoContent();
		}
	}
}
