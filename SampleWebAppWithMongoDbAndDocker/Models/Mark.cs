using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	[BsonIgnoreExtraElements]
	public class Mark
	{
		public ObjectId Id { get; set; }

		public string SubjectName { get; set; }
		public DateTime DateTime { get; set; }
		public int Value { get; set; }
	}
}
