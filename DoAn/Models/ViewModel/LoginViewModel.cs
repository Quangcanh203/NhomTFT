using System.ComponentModel.DataAnnotations;

namespace DoAn.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Làm ơn hãy nhập tên")]
		public string Username { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "làm ơn hãy nhập mật khẩu ")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; } // Thêm thuộc tính này
	}
}
		