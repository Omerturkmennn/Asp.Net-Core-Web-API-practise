using Entities1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Repositories.EFcore.Extensions
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint maxPrice, uint minPrice) =>

            books.Where(book => book.Price >= minPrice && book.Price <= maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return books;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            
            return books.Where(book => book.Title.ToLower().Contains(lowerCaseTerm));
        }


        
        
        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b => b.Id);

            //PARÇALAMA: Kullanıcı "title desc, price asc" gibi virgüllü (birden çok) sıralama istemiş olabilir.
            // Metnin sağındaki solundaki boşlukları sil ve virgüle göre parçalara ayırarak bir dizi (array) yap.
            //  var orderParams = orderByQueryString.Trim().Split(',');

            // GÜVENLİK/AYNA TUTMA (Reflection): 
            // Book sınıfının iç Dışarıdan gelen "title" kelimesine güvenemeyiz.indeki GERÇEK özellikleri (Id, Title, Price)
            // listeliyoruz ki, birazdan kullanıcının istediği kolon gerçekten var mı diye kontrol edebilelim.
            //  var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // İNŞA ALANI: SQL'in anlayacağı ("Title descending, Price ascending") metnini 
            // parça parça birleştirmek için bir StringBuilder oluşturuyoruz. (String birleştirirken performansı artırır)
            //  var orderQueryBuilder = new StringBuilder();


            //DÖNGÜ: Parçaladığımız her bir isteği ("title desc", "price asc") tek tek geziyoruz.
            //foreach (var param in orderParams)
            

            // DÖNGÜ BİTTİ. 
            // TEMİZLİK: Elimizde "Title descending, Price ascending, " gibi bir metin var. 
            // Sondaki gereksiz virgülü ve boşluğu TrimEnd ile tıraşlıyoruz.
            // var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            //Base sınıftan gelen yapı!!!
            var orderQuery=OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);


            // 7. SON KONTROL: Eğer kullanıcının gönderdiği tüm kolonlar uydurmaydıysa ve StringBuilder boş kaldıysa...
            if (string.IsNullOrWhiteSpace(orderQuery))
                // Yİne Id'ye göre sıralayıp işi bitir.
                return books.OrderBy(b => b.Id);

            // İŞLEM TAMAM: System.Linq.Dynamic.Core kütüphanesi sayesinde, oluşturduğumuz temiz 
            // string metnini (örn: "Title descending, Price ascending") LINQ komutuna verip veritabanına yolluyoruz.
             return books.OrderBy(orderQuery);
        }
    }
}
