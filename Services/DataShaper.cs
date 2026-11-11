using Entities1.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    // "where T : class" sadece referans tipleri (class) kabul eder.
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        // T nesnesinin sahip olduğu tüm public özellikleri hafızada tutacağımız yer.
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            
            // Sadece Public  ve Instance (nesneye ait) olan özellikleri alıp Properties dizisine atarız.
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        // =========================================================================
        // DIŞARIYA AÇILAN KAPILAR (PUBLIC METOTLAR)
        // Controller veya Servis katmanından sadece bu iki metot çağrılacak.
        // =========================================================================

        // A) LİSTE İÇİN: Dışarıdan bir nesne listesi (entities) ve istenen alanlar ("title, price") gelir.
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            // 1. Önce "title, price" metnini al, gerçek C# özelliklerine dönüştür (Güvenlik filtrelemesi).
            var requiredFields = GetRequiredProperties(fieldsString);

            // 2. Filtrelenmiş özellikleri ve ham veriyi alıp, şekillendirme mutfağına yolla.
            return FetchData(entities, requiredFields);
        }

        // B) TEK NESNE İÇİN: Dışarıdan sadece 1 tane nesne (entity) gelir. (Örn: Sadece 1. kitabı getir isteği)
        public ShapedEntity ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        // =========================================================================
        // ARKA PLAN İŞLEMLERİ (MUTFAK - PRIVATE METOTLAR)
        // =========================================================================

        // Kullanıcının istediği alanları (fieldsString) kontrol eden metot.
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredFields = new List<PropertyInfo>();

            // Kullanıcı URL'ye "?fields=title,price" gibi bir şey yazmış mı?
            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                // Virgüllerden böl. (Örn: "title", "price")
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)
                {
                    // Kullanıcının yazdığı metin, bizim sınıfımızın özellikleri içinde var mı?
                    var property = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    // Yoksa (uydurma bir kolonsa) es geç.
                    if (property is null)
                        continue;

                    // Varsa, güvenli sepete ekle.
                    requiredFields.Add(property);
                }
            }
            else
            {
                // Kullanıcı alan belirtmediyse (veya boş gönderdiyse), tüm özellikleri sepete at.
                requiredFields = Properties.ToList();
            }

            return requiredFields;
        }

        // 2. ASIL YOĞURMA İŞLEMİ (TEK NESNE İÇİN)
        // Elimizde asıl veri (entity) ve sadece istenen alanların listesi (requiredProperties) var.
        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            // ExpandoObject: Çalışma zamanında içine özellik ekleyebildiğimiz  C# nesnesi 
            // Arka planda aslında bir Dictionary<string, object> gibi çalışır.
            var shapedObject = new ShapedEntity();

            // Sadece kullanıcının istediği (filtrelenmiş) özellikleri dönüyoruz.
            foreach (var property in requiredProperties)
            {
                // Reflection ile asıl verinin (entity) içindeki o özelliğin "değerini" okuyoruz.
                // (Örn: property "Title" ise, objectPropertyValue "1984" olur).
                var objectPropertyValue = property.GetValue(entity);

                // Okuduğumuz bu değeri, ExpandoObject'in içine "Anahtar - Değer" olarak ekliyoruz.
                // Örn: ("Title", "1984")
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id");
            shapedObject.Id = (int)objectProperty.GetValue(entity);
            // İçinde sadece kullanıcının istediği verilerin olduğu şekillenmiş objeyi geri dön.
            return shapedObject;
        }

        // 3. LİSTE YOĞURMA İŞLEMİ
        // Eğer dışarıdan tek bir nesne değil de bir liste geldiyse, bu metot devreye girer.
        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            // Şekillenmiş nesneleri biriktireceğimiz boş liste.
            var shapedData = new List<ShapedEntity>();

            // Asıl veritabanından gelen tüm kayıtları tek tek dönüyoruz.
            foreach (var entity in entities)
            {
                // Her bir kaydı, az önce yazdığımız "Tek Nesne Yoğurma" metoduna gönderiyoruz.
                var shapedObject = FetchDataForEntity(entity, requiredProperties);

                // Şekillenmiş halini listeye ekliyoruz.
                shapedData.Add(shapedObject);
            }

            // Sonuç olarak sadece istenen kolonların olduğu yepyeni bir listeyi dışarı fırlatıyoruz.
            return shapedData;
        }
    }
}