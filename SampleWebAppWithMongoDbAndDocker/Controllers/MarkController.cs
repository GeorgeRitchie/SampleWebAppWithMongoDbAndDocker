using Microsoft.AspNetCore.Mvc;
//using MongoDB.Bson;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[ApiController]
	[ApiVersion("2.0")]
	[Route("api/{version:apiVersion}/[controller]/[action]")]
	public class MarkController : ControllerBase
	{
		private readonly IMongoCollection<Mark> markCollection;

		public MarkController(IMongoDatabase db)
		{
			markCollection = db.GetCollection<Mark>("Marks");
		}

		// GET api/<MarkController>/5
		[HttpGet]
		public JsonResult Get([FromQuery] MarkFilter filter)
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

			return new JsonResult(new { Marks = markCollection.Find(filterDB).ToList() });
		}

		// POST api/<MarkController>
		[HttpPost]
		public JsonResult Create([FromBody] CreateMarkModel newMark)
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
			return new JsonResult(new { Id = mark.Id });
		}

		// PUT api/<MarkController>/5
		[HttpPut]
		public JsonResult Update([FromBody] UpdateMarkModel newMark)
		{
			var filter = Builders<Mark>.Filter.Eq(p => p.Id, newMark.Id);
			var updater = Builders<Mark>.Update.Set(p => p.Value, newMark.Value);
			var modifiedCount = markCollection.UpdateOne(filter, updater).ModifiedCount;
			return new JsonResult(new { ModifiedObjectsAmount = modifiedCount });
		}

		// DELETE api/<MarkController>/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			markCollection.FindOneAndDelete(p => p.Id == id);
		}
	}
}
