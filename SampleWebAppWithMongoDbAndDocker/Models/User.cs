using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	[BsonIgnoreExtraElements]
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public Guid RoleId { get; set; }
	}
}
