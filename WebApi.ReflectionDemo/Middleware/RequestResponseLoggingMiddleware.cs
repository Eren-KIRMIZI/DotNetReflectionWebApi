namespace WebApi.ReflectionDemo.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine(
                $"REQUEST => {context.Request.Method} {context.Request.Path} | {DateTime.Now}");

            await _next(context);

            Console.WriteLine(
                $"RESPONSE => Status Code: {context.Response.StatusCode}");
        }
    }
}
