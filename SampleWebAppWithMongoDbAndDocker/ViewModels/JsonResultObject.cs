namespace SampleWebAppWithMongoDbAndDocker.ViewModels
{
	public class JsonResultObject<T> where T : class
	{
		public int StatusCode { get; set; }
		public string[]? ErrorMessages { get; set; }
		public DateTime DateTime { get; set; }
		public T Body { get; set; }
	}
}
