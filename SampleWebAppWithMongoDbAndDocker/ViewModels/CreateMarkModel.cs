namespace SampleWebAppWithMongoDbAndDocker.ViewModels
{
	public class CreateMarkModel
	{
		public string SubjectName { get; set; }
		public int Value { get; set; }
		public Guid StudentId { get; set; }
		public Guid TeacherId { get; set; }
	}
}
