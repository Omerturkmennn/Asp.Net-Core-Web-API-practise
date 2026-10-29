using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities1.DTO
{
    public abstract record BookDtoForManipulation()
    {
        [Required(ErrorMessage ="Title is a required field.")]
        [MinLength(2,ErrorMessage ="Title must consist of at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Title must consist of at maximum 50 characters")]
        public string Title { get; init; } = string.Empty;

        [Required(ErrorMessage = "Price is a required field.")]
        [Range(10,1000)]
        public int Price { get; init; }
    }
}
