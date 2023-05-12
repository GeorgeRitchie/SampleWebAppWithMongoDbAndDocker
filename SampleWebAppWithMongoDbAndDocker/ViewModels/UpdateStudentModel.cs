namespace SampleWebAppWithMongoDbAndDocker.ViewModels
{
	public class UpdateStudentModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public Guid TeacherId { get; set; }
	}
}
