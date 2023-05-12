namespace SampleWebAppWithMongoDbAndDocker.ViewModels
{
	public class MarkFilter
	{
		public Guid TeacherId { get; set; }
		public Guid? MarkId { get; set; }
		public Guid? StudentId { get; set; }
		public string? SubjectName { get; set; }
		public DateTime? DateTime { get; set; }
	}
}
