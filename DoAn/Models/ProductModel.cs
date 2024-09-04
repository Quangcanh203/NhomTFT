using DoAn.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên Sản Phẩm")]
		public string Name { get; set; }
		public string slug { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả cho sản phẩm.")]
        public string Description { get; set; }


        [Required(ErrorMessage ="Yêu cầu nhập giá sản phẩm")]
		[Range(0.01, double.MaxValue)]
		[Column(TypeName ="decimal(8, 2)")]	
        public decimal Price { get; set; }
		public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }

        [NotMapped]
        [FileExtension(ErrorMessage = "Allowed extensions are .jpg, .jpeg, or .png")]
        public IFormFile? ImageUpload { get; set; }


    }
}
