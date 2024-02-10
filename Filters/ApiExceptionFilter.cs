namespace CatalogoApi.Filter;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


public class ApiExceptionFilter : IExceptionFilter
{

    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Ocorreu uma exceção não tradada.");
        context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitação")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}