using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories
{
    public class UpdateCategoryRequest : Request
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Título inválido")]
        [MaxLength(80, ErrorMessage ="O título deve ter no máximo 80 caracteres")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage ="Descição inválida")]
        public string Description { get; set; }= string.Empty;
    }
}