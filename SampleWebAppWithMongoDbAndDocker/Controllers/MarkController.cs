using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiVersion("2.0")]
	[Route("api/{version:apiVersion}/[controller]/[action]")]
	[Authorize(Roles = "teacher, admin")]
	public class MarkController : ApiBaseController
	{
		private readonly IMongoCollection<Mark> markCollection;

		public MarkController(IMongoDatabase db)
		{
			markCollection = db.GetCollection<Mark>("Marks");
		}

		[HttpGet]
		public IActionResult Get([FromQuery] MarkFilter filter)
		{
			var filterDB = Builders<Mark>.Filter.Eq(p => p.TeacherId, filter.TeacherId);

			if (string.IsNullOrEmpty(filter.SubjectName) == false)
				filterDB = filterDB & Builders<Mark>.Filter.Eq(p => p.SubjectName, filter.SubjectName);

			if (filter.StudentId != null)
				filterDB = filterDB | Builders<Mark>.Filter.Eq(p => p.StudentId, filter.StudentId);

			if (filter.DateTime != null)
				filterDB = filterDB | Builders<Mark>.Filter.Eq(p => p.DateTime, filter.DateTime);

			if (filter.MarkId != null)
				filterDB = filterDB | Builders<Mark>.Filter.Eq(p => p.Id, filter.MarkId);

			return JsonActionResult(new { Marks = markCollection.Find(filterDB).ToList() });
		}

		[HttpPost]
		public IActionResult Create([FromBody] CreateMarkModel newMark)
		{
			var mark = new Mark
			{
				SubjectName = newMark.SubjectName,
				DateTime = DateTime.Now,
				Value = newMark.Value,
				StudentId = newMark.StudentId,
				TeacherId = newMark.TeacherId
			};
			markCollection.InsertOne(mark);
			return JsonActionResult(new { Id = mark.Id });
		}

		[HttpPut]
		public IActionResult Update([FromBody] UpdateMarkModel newMark)
		{
			var filter = Builders<Mark>.Filter.Eq(p => p.Id, newMark.Id);
			var updater = Builders<Mark>.Update.Set(p => p.Value, newMark.Value);
			var modifiedCount = markCollection.UpdateOne(filter, updater).ModifiedCount;
			return JsonActionResult(new { ModifiedObjectsAmount = modifiedCount });
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			markCollection.FindOneAndDelete(p => p.Id == id);
			return JsonActionResult();
		}
	}
}
