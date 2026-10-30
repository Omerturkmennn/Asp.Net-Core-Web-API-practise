using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //İsteğin tam olarak kime geldiğini not alıyoruz
            //Hangi Controller'a ve onun içindeki hangi metoda (action) gidilmeye çalışılıyor?
            //Bunu hata mesajında "Şu Controller'daki şu metoda boş veri gönderdin" diyebilmek için aldık
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            //isteğin içinde gelen parametreleri (ActionArguments) tarıyoruz.
            //İçinde "Dto" kelimesi geçen nesneyi bulmaya çalışıyoruz.
            //Eğer API'ye dışarıdan bir veri (JSON body vb.) gönderilmemişse veya gönderilen nesne boşsa, bu satırın sonucunda param değişkeni null olur
            var param = context.ActionArguments.SingleOrDefault(p => p.Value.ToString().Contains("Dto")).Value;

            if (param == null) 
            {
                context.Result = new BadRequestObjectResult($"Object is null" + $"Controller : {controller}" +
                    $"Action : {action}");

                return; //400 
            }

            if (!context.ModelState.IsValid) 
            {
                context.Result=new UnprocessableEntityObjectResult(context.ModelState); //422
            }
        }
    }
}