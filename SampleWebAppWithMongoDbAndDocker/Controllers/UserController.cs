using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiVersionNeutral]
	[Route("api/{version:apiVersion}/[controller]/[action]")]
	public class UserController : ApiBaseController
	{
		private readonly IMongoCollection<Teacher> teacherCollection;
		private readonly IMongoCollection<Student> studentCollection;
		private readonly IMongoCollection<Role> roleCollection;

		public UserController(IMongoDatabase db)
		{
			teacherCollection = db.GetCollection<Teacher>("Teachers");
			studentCollection = db.GetCollection<Student>("Students");
			roleCollection = db.GetCollection<Role>("Roles");
		}

		[HttpGet]
		public async Task<IActionResult> LogIn([FromQuery] LogInModel model)
		{
			User user = teacherCollection.Find(p => p.Email == model.Email && p.Password == model.Password).FirstOrDefault();

			if (user == null)
				user = studentCollection.Find(p => p.Email == model.Email && p.Password == model.Password).FirstOrDefault();

			if (user != null)
			{
				await Authenticate(user, model.RememberMe);

				return JsonActionResult();
			}

			return JsonActionResultError(new string[] { "Not found user" });
		}

		private async Task Authenticate(User user, bool? rememberMe = false)
		{
			var roleName = roleCollection.Find(p => p.Id == user.RoleId).First()?.Name ?? "";

			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)
			};

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

			var authProp = new AuthenticationProperties();
			if (rememberMe == true)
				authProp.IsPersistent = true;
			else
				authProp.IsPersistent = false;

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), authProp);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return JsonActionResult();
		}
	}
}
