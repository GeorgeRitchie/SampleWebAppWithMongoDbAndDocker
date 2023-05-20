using System.ComponentModel.DataAnnotations;

namespace ClientWebApp.Models.RequestModels
{
	public class LogInDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 6)]
		public string Password { get; set; }
	}
}
