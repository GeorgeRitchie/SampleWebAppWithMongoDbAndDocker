using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	[BsonIgnoreExtraElements]
	public class Teacher : User
	{
		public string Major { get; set; }

		public List<Student> Students { get; set; }
	}
}
