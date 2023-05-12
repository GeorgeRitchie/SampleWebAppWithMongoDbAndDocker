﻿using MongoDB.Bson;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
	}
}