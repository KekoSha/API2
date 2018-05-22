using API2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API2.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ScaffoldColumn(false)] //will not display this field in the views
        public String UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(100)]
        public String Subject { get; set; }

        [Required]
        [StringLength(5000)]
        public String Body { get; set; }

        [ScaffoldColumn(false)] //will not display this field in the views
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Auther Name")]
        [StringLength(150)]
        public String AutherName { get; set; }

        [ScaffoldColumn(false)] //will not display this field in the views
        public int Readers { get; set; }

        [Display(Name = "Image")]
        public String ImageUrl { get; set; }
    }
}
