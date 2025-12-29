using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace WebApi.ReflectionDemo.Filters
{
    public class ActionTimingFilter : IActionFilter
    {
        private Stopwatch _stopwatch;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            Console.WriteLine(
                $"ACTION TIME => {context.ActionDescriptor.DisplayName} | {_stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
