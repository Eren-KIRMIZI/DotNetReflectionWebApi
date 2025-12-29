using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.ReflectionDemo.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine("GLOBAL EXCEPTION FILTER ÇALIŞTI");

            context.Result = new ObjectResult(new
            {
                error = "Beklenmeyen bir hata oluştu",
                detail = context.Exception.Message
            })
            {
                StatusCode = 500
            };
        }
    }
}
