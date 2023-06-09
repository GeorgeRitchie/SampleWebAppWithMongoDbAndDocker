﻿using Microsoft.AspNetCore.Authorization;
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
	[Authorize]
	public class StudentController : ApiBaseController
	{
		private readonly IMongoCollection<Student> studentsCollection;
		private readonly IMongoCollection<Teacher> teachersCollection;
		private readonly IMongoCollection<Role> roleCollection;

		public StudentController(IMongoDatabase db)
		{
			studentsCollection = db.GetCollection<Student>("Students");
			teachersCollection = db.GetCollection<Teacher>("TeacherId");
			roleCollection = db.GetCollection<Role>("Roles");
		}

		[Authorize(Roles = "teacher, admin")]
		[HttpGet]
		public IActionResult Get()
		{
			return JsonActionResult(new { Students = studentsCollection.Find("{}").ToList() });
		}

		[HttpGet("{id}")]
		public IActionResult Get(Guid id)
		{
			return JsonActionResult(new { Student = studentsCollection.Find(p => p.Id == id).FirstOrDefault() });
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] CreateStudentModel newStudent)
		{
			if (studentsCollection.Find(p => p.Email == newStudent.Email).FirstOrDefault() != default)
			{
				return JsonActionResultError(new string[] { "This email is used!" });
			}

			if (teachersCollection.Find(p => p.Id == newStudent.TeacherId).FirstOrDefault() == null)
			{
				return JsonActionResultError(new string[] { $"Could not find teacher with Id {newStudent.TeacherId}" });
			}

			var student = new Student
			{
				Name = newStudent.Name,
				Phone = newStudent.Phone,
				TeacherId = newStudent.TeacherId,
				Email = newStudent.Email,
				Password = newStudent.Password,
				RoleId = roleCollection.Find(p => p.Name == "student").First().Id
			};

			studentsCollection.InsertOne(student);

			return RedirectToAction("LogIn", "User", new LogInModel { Email = student.Email, Password = student.Password });
		}

		[HttpPut]
		public IActionResult Update([FromBody] UpdateStudentModel newStudent)
		{
			if (teachersCollection.Find(p => p.Id == newStudent.TeacherId).FirstOrDefault() == null)
			{
				return BadRequest($"Could not find teacher with Id {newStudent.TeacherId}");
			}

			var filter = Builders<Student>.Filter.Eq(p => p.Id, newStudent.Id);
			var updater = Builders<Student>.Update.Set(p => p.Name, newStudent.Name).Set(p => p.Phone, newStudent.Phone).Set(p => p.TeacherId, newStudent.TeacherId);
			return JsonActionResult(new { ModifiedObjectsAmount = studentsCollection.UpdateOne(filter, updater).ModifiedCount });
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			studentsCollection.FindOneAndDelete(p => p.Id == id);
			return JsonActionResult();
		}
	}
}
