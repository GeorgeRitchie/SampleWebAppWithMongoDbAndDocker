using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[ApiVersion("2.0")]
	[Route("api/{version:apiVersion}/[controller]/[action]")]
	[Authorize]
	public class TeacherController : ControllerBase
	{
		private readonly IMongoCollection<Teacher> teacherCollection;

		public TeacherController(IMongoDatabase db)
		{
			teacherCollection = db.GetCollection<Teacher>("Teachers");
		}

		[HttpGet]
		public JsonResult Get()
		{
			return new JsonResult(new { Teachers = teacherCollection.Find("{}").ToList() });
		}

		[HttpGet("{id}")]
		public JsonResult Get(Guid id)
		{
			return new JsonResult(new { Teacher = teacherCollection.Find(p => p.Id == id).FirstOrDefault() });
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Create([FromBody] CreateTeacherModel newTeacher)
		{
			if (teacherCollection.Find(p => p.Email == newTeacher.Email).FirstOrDefault() != default)
			{
				return BadRequest("This email is used!");
			}

			var teacher = new Teacher
			{
				Name = newTeacher.Name,
				Phone = newTeacher.Phone,
				Major = newTeacher.Major,
				Email = newTeacher.Email,
				Password = newTeacher.Password
			};

			teacherCollection.InsertOne(teacher);

			return RedirectToAction("LogIn", "User", new LogInModel { Email = teacher.Email, Password = teacher.Password });
		}

		[HttpPut]
		public JsonResult Update([FromBody] UpdateTeacherModel newTeacher)
		{
			var filter = Builders<Teacher>.Filter.Eq(p => p.Id, newTeacher.Id);
			var updater = Builders<Teacher>.Update.Set(p => p.Name, newTeacher.Name).Set(p => p.Phone, newTeacher.Phone).Set(p => p.Major, newTeacher.Major);
			return new JsonResult(new { ModifiedObjectsAmount = teacherCollection.UpdateOne(filter, updater).ModifiedCount });
		}

		[HttpDelete("{id}")]
		public ActionResult Delete(Guid id)
		{
			teacherCollection.FindOneAndDelete(p => p.Id == id);
			return NoContent();
		}
	}
}