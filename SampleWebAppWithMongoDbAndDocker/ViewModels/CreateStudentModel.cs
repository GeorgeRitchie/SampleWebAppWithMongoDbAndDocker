namespace SampleWebAppWithMongoDbAndDocker.ViewModels
{
	public class CreateStudentModel
	{
		public string Name { get; set; }
		public string Phone { get; set; }
		public Guid TeacherId { get; set; }
	}
}
