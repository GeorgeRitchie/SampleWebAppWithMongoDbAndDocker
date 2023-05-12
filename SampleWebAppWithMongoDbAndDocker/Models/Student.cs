using MongoDB.Bson;

namespace SampleWebAppWithMongoDbAndDocker.Models
{
	public class Student : User
	{
		public List<Mark> Marks { get; set; }
		public Teacher Teacher { get; set; }
	}
}
