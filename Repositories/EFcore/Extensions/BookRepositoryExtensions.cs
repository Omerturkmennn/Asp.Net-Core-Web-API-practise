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
            var orderParams = orderByQueryString.Trim().Split(',');

            // GÜVENLİK/AYNA TUTMA (Reflection): 
            // Dışarıdan gelen "title" kelimesine güvenemeyiz. Book sınıfının içindeki GERÇEK özellikleri (Id, Title, Price)
            // listeliyoruz ki, birazdan kullanıcının istediği kolon gerçekten var mı diye kontrol edebilelim.
            var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 4. İNŞA ALANI: SQL'in anlayacağı ("Title descending, Price ascending") metnini 
            // parça parça birleştirmek için bir StringBuilder oluşturuyoruz. (String birleştirirken performansı artırır)
            var orderQueryBuilder = new StringBuilder();

            // 5. DÖNGÜ: Parçaladığımız her bir isteği ("title desc", "price asc") tek tek geziyoruz.
            foreach (var param in orderParams)
            {
                
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                // "title desc" metnini boşluktan (" ") ikiye böl ve ilk sıradakini (0) al. 
                // Yani elimizde sadece "title" kelimesi kaldı.
                var propertyFromQueryName = param.Split(" ")[0];

                // Book sınıfının özellikleri içinde, kullanıcının gönderdiği "title" kelimesi var mı?
                // StringComparison.InvariantCultureIgnoreCase -> Büyük/küçük harf duyarlılığını kapatır ("Title" == "title" olur)
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                   StringComparison.InvariantCultureIgnoreCase));

                // Eğer kullanıcı veritabanında olmayan uydurma bir kolon (örn: "YazarAdi") gönderdiyse
                // objectProperty null döner. Hemen 'continue' deyip bu hatalı isteği atlıyoruz (Sistemi patlatmıyoruz).
                if (objectProperty == null)
                    continue;

                // Metnin sonu " desc" ile bitiyorsa tersine sıralama ("descending"), bitmiyorsa düz sıralama ("ascending") kelimesini seç.
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                // StringBuilder'ın içine orijinal sınıf özelliğinin adını (Title) ve yönünü yazıp sonuna virgül koyuyoruz.
                // Örnek çıktı: "Title descending, "
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
            }

            // DÖNGÜ BİTTİ. 
            // 6. TEMİZLİK: Elimizde "Title descending, Price ascending, " gibi bir metin var. 
            // Sondaki gereksiz virgülü ve boşluğu TrimEnd ile tıraşlıyoruz.
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            // 7. SON KONTROL: Eğer kullanıcının gönderdiği tüm kolonlar uydurmaydıysa ve StringBuilder boş kaldıysa...
            if (string.IsNullOrWhiteSpace(orderQuery))
                // Yİne Id'ye göre sıralayıp işi bitir.
                return books.OrderBy(b => b.Id);

            // 8. İŞLEM TAMAM: System.Linq.Dynamic.Core kütüphanesi sayesinde, oluşturduğumuz temiz 
            // string metnini (örn: "Title descending, Price ascending") LINQ komutuna verip veritabanına yolluyoruz.
            return books.OrderBy(orderQuery);
        }
    }
}
