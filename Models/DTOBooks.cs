using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOBooks
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        public Guid ISBN { get; set; }

        [Required(ErrorMessage = "Publication Date is required")]
        public DateTime Publication_Date { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Language is required")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "PageCount is required")]
        [Range(1, int.MaxValue)]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "AvgRating is required")]
        public float AvgRating { get; set; }

        [Required(ErrorMessage = "BookGenre is required")]
        public int BookGenre { get; set; }

        [Required]
        public List<Guid> AuthorID { get; set; }
    }

    public class UpdateBookModel
    {
        public Guid BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ISBN { get; set; }
        public DateTime Publication_Date { get; set; }
        public float Price { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public int PageCount { get; set; }
        public float AvgRating { get; set; }
        public int BookGenre { get; set; }
        public bool IsAvailable { get; set; }
        public List<Guid> AuthorID { get; set; }
    }
}