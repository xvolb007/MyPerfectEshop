using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EshopBooks.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
		[DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display Order must be in 1-100 range")]
		public int DisplayOrder { get; set; }
    }
}
