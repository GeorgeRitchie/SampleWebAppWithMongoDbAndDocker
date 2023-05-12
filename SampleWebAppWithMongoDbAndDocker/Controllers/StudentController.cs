﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SampleWebAppWithMongoDbAndDocker.Models;
using SampleWebAppWithMongoDbAndDocker.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAppWithMongoDbAndDocker.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		private readonly IMongoCollection<Student> studentsCollection;
		//private readonly IMongoCollection<Teacher> teachersCollection;

		public StudentController(IMongoDatabase db)
		{
			studentsCollection = db.GetCollection<Student>("Students");
			//teachersCollection = db.GetCollection<Teacher>("TeacherId");
		}

		// GET: api/<StudentController>
		[HttpGet]
		public IEnumerable<Student> Get()
		{
			return studentsCollection.Find("{}").ToList();
		}

		// GET api/<StudentController>/5
		[HttpGet("{id}")]
		public Student Get(Guid id)
		{
			return studentsCollection.Find(p => p.Id == id).FirstOrDefault();
		}

		// POST api/<StudentController>
		[HttpPost]
		public Guid Create([FromBody] CreateStudentModel newStudent)
		{
			var student = new Student
			{
				Name = newStudent.Name,
				Phone = newStudent.Phone,
				TeacherId = newStudent.TeacherId,//teachersCollection.Find(p => p.Id == newStudent.TeacherId).FirstOrDefault(),
			};
			studentsCollection.InsertOne(student);
			return student.Id;
		}

		// PUT api/<StudentController>/5
		[HttpPut]
		public long Update([FromBody] UpdateStudentModel newStudent)
		{
			var filter = Builders<Student>.Filter.Eq(p => p.Id, newStudent.Id);
			var updater = Builders<Student>.Update.Set(p => p.Name, newStudent.Name).Set(p => p.Phone, newStudent.Phone).Set(p => p.TeacherId, newStudent.TeacherId); //teachersCollection.Find(t => t.Id == newStudent.TeacherId).FirstOrDefault());
			return studentsCollection.UpdateOne(filter, updater).ModifiedCount;
		}

		// DELETE api/<StudentController>/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			studentsCollection.FindOneAndDelete(p => p.Id == id);
		}
	}
}