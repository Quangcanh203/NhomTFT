using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DoAn.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower(); // Chuyển thành chữ thường
                string[] extensions = { ".jpg", ".png", ".jpeg" }; // Thêm dấu chấm trước phần mở rộng
                bool result = extensions.Any(x => extension.Equals(x));
                if (!result)
                {
                    return new ValidationResult("Allowed extensions are .jpg, .jpeg, or .png");
                }
            }
            return ValidationResult.Success;
        }
    }
}
