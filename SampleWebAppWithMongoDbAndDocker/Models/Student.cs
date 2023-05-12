using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	[BsonIgnoreExtraElements]
	public class Student : User
	{
		public List<Mark> Marks { get; set; }
		public Guid TeacherId { get; set; }
	}
}
