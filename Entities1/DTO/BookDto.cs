using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities1.DTO
{
    public record BookDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int Price { get; init; }
    }
}
