using Entities1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFcore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(

                new Book { Id = 1, Title = "Beyaz Diş", Price = 70 },
                new Book { Id = 2, Title = "kara Diş", Price = 270 },
                new Book { Id = 3, Title = "kırmızı Diş", Price = 170 }
             );
        }
    }
}
