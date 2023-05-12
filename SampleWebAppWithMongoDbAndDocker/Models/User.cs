using MongoDB.Bson;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	public class User
	{
		public ObjectId Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
	}
}
