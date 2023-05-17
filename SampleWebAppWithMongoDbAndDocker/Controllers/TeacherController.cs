using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiVersion("1.0")]
	[ApiVersion("2.0")]
	[Route("api/{version:apiVersion}/[controller]/[action]")]
	[Authorize(Roles = "teacher, admin")]
	public class TeacherController : ApiBaseController
	{
		private readonly IMongoCollection<Teacher> teacherCollection;
		private readonly IMongoCollection<Role> roleCollection;

		public TeacherController(IMongoDatabase db)
		{
			teacherCollection = db.GetCollection<Teacher>("Teachers");
			roleCollection = db.GetCollection<Role>("Roles");
		}

		//[Authorize(Roles = "admin")]
		[HttpGet]
		public IActionResult Get()
		{
			return JsonActionResult(new { Teachers = teacherCollection.Find("{}").ToList() });
		}

		[HttpGet("{id}")]
		public IActionResult Get(Guid id)
		{
			return JsonActionResult(new { Teacher = teacherCollection.Find(p => p.Id == id).FirstOrDefault() });
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] CreateTeacherModel newTeacher)
		{
			if (teacherCollection.Find(p => p.Email == newTeacher.Email).FirstOrDefault() != default)
			{
				return JsonActionResultError(new string[] { "This email is used!" });
			}

			var teacher = new Teacher
			{
				Name = newTeacher.Name,
				Phone = newTeacher.Phone,
				Major = newTeacher.Major,
				Email = newTeacher.Email,
				Password = newTeacher.Password,
				RoleId = roleCollection.Find(p => p.Name == "teacher").First().Id
			};

			teacherCollection.InsertOne(teacher);

			return RedirectToAction("LogIn", "User", new LogInModel { Email = teacher.Email, Password = teacher.Password });
		}

		[HttpPut]
		public IActionResult Update([FromBody] UpdateTeacherModel newTeacher)
		{
			var filter = Builders<Teacher>.Filter.Eq(p => p.Id, newTeacher.Id);
			var updater = Builders<Teacher>.Update.Set(p => p.Name, newTeacher.Name).Set(p => p.Phone, newTeacher.Phone).Set(p => p.Major, newTeacher.Major);
			return JsonActionResult(new { ModifiedObjectsAmount = teacherCollection.UpdateOne(filter, updater).ModifiedCount });
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			teacherCollection.FindOneAndDelete(p => p.Id == id);
			return JsonActionResult();
		}
	}
}