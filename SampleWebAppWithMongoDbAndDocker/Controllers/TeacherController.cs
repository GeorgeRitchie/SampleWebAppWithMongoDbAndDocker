using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TeacherController : ControllerBase
	{
		private readonly IMongoCollection<Teacher> teacherCollection;

		public TeacherController(IMongoDatabase db)
		{
			teacherCollection = db.GetCollection<Teacher>("Teachers");
		}

		// GET: api/<TeacherController>
		[HttpGet]
		public IEnumerable<Teacher> Get()
		{
			return teacherCollection.Find("{}").ToList();
		}

		// GET api/<TeacherController>/5
		[HttpGet("{id}")]
		public Teacher Get(Guid id)
		{
			return teacherCollection.Find(p => p.Id == id).FirstOrDefault();
		}

		// POST api/<TeacherController>
		[HttpPost]
		public Guid Create([FromBody] CreateTeacherModel teacherData)
		{
			var teacher = new Teacher { Name = teacherData.Name, Phone = teacherData.Phone, Major = teacherData.Major };
			teacherCollection.InsertOne(teacher);
			return teacher.Id;
		}

		// PUT api/<TeacherController>/5
		[HttpPut]
		public long Update([FromBody] UpdateTeacherModel newTeacher)
		{
			var filter = Builders<Teacher>.Filter.Eq(p => p.Id, newTeacher.Id);
			var updater = Builders<Teacher>.Update.Set(p => p.Name, newTeacher.Name).Set(p => p.Phone, newTeacher.Phone).Set(p => p.Major, newTeacher.Major);
			return teacherCollection.UpdateOne(filter, updater).ModifiedCount;
		}

		// DELETE api/<TeacherController>/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			teacherCollection.FindOneAndDelete(p => p.Id == id);
		}
	}
}
