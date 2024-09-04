using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Làm ơn hãy nhập tên")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Làm ơn hãy nhập tên"),EmailAddress]

		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="làm ơn hãy nhập mật khẩu ")]
		public string Password { get; set; }
	}
}
