using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	public class Role
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		[BsonIgnore]
		public List<User> Users { get; set; }
	}
}
