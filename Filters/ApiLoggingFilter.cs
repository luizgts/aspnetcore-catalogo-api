namespace CatalogoApi.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"\n\n# ModelState: {context.ModelState.IsValid}\n");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"\n\n# StatusCode: {context.HttpContext.Response.StatusCode}\n");
    }

}