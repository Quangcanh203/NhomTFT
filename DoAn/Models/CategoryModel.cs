using System.ComponentModel.DataAnnotations;

namespace DoAn.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập lại tên Danh mục")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập lại Mô tả Danh mục")]
        public string Description { get; set; }

        public string Slug { get; set; }

        public int Status { get; set; }
    }
}
