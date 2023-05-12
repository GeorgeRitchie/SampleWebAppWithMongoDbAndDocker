using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	[BsonIgnoreExtraElements]
	public class Mark
	{
		public Guid Id { get; set; }

		public string SubjectName { get; set; }
		public DateTime DateTime { get; set; }
		public int Value { get; set; }
		public Guid StudentId { get; set; }
		public Guid TeacherId { get; set; }		// To prevent modifying mark by another teacher
	}
}
