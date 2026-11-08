using Entities1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFcore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String  CreateOrderQuery<T>(String orderByQueryString)
        {

            //PARÇALAMA: Kullanıcı "title desc, price asc" gibi virgüllü (birden çok) sıralama istemiş olabilir.
            // Metnin sağındaki solundaki boşlukları sil ve virgüle göre parçalara ayırarak bir dizi (array) yap.
            var orderParams = orderByQueryString.Trim().Split(',');

            // GÜVENLİK/AYNA TUTMA (Reflection): 
            // Book sınıfının iç Dışarıdan gelen "title" kelimesine güvenemeyiz.indeki GERÇEK özellikleri (Id, Title, Price)
            // listeliyoruz ki, birazdan kullanıcının istediği kolon gerçekten var mı diye kontrol edebilelim.
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // İNŞA ALANI: SQL'in anlayacağı ("Title descending, Price ascending") metnini 
            // parça parça birleştirmek için bir StringBuilder oluşturuyoruz. (String birleştirirken performansı artırır)
             var orderQueryBuilder = new StringBuilder();

            //DÖNGÜ: Parçaladığımız her bir isteği ("title desc", "price asc") tek tek geziyoruz.
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
            // TEMİZLİK: Elimizde "Title descending, Price ascending, " gibi bir metin var. 
            // Sondaki gereksiz virgülü ve boşluğu TrimEnd ile tıraşlıyoruz.
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            // İŞLEM TAMAM: System.Linq.Dynamic.Core kütüphanesi sayesinde, oluşturduğumuz temiz 
            // string metnini (örn: "Title descending, Price ascending") LINQ komutuna verip veritabanına yolluyoruz.
            return orderQuery;
        }
    }
}
