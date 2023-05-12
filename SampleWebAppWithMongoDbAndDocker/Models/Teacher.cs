using MongoDB.Bson;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	public class Teacher : User
	{
		public string Major { get; set; }
	}
}
