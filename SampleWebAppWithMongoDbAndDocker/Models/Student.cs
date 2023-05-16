using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	[BsonIgnoreExtraElements]
	public class Student : User
	{
		[BsonIgnore]
		public List<Mark> Marks { get; set; }

		public Guid TeacherId { get; set; }
	}
}
