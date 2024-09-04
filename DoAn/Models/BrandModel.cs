using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập lại tên Thương Hiệu")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập lại Mô tả Thương hiệu")]
		public string Description { get; set; }

		public string Slug { get; set; }
		public int Status { get; set; }
	}
}
