using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace WebApi.ReflectionDemo.Controllers
{
    [ApiController]
    [Route("api/system")]
    public class SystemController : ControllerBase
    {
        [HttpGet("attribute-map")]
        public IActionResult GetAttributeMap()
        {
            var controllers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(ControllerBase).IsAssignableFrom(t) &&
                    !t.IsAbstract);

            var result = controllers.Select(controller => new
            {
                Controller = controller.Name,
                Actions = controller.GetMethods(
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.DeclaredOnly)
                    .Select(action => new
                    {
                        ActionName = action.Name,
                        HttpAttributes = action
                            .GetCustomAttributes()
                            .Where(a => a.GetType().Name.StartsWith("Http"))
                            .Select(a => a.GetType().Name)
                    })
            });

            return Ok(result);
        }
    }
}