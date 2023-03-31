using Exceptions;
using Serilog;

namespace FxPlatformApp.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public ExceptionHandlingMiddleware(IDiagnosticContext diagnosticContext, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (FinnhubException ex)
            {
                LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw;
            }
        }

        private void LogException(Exception exception)
        {
            if(exception.InnerException != null)
            {
                if (exception.InnerException.InnerException != null)
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}", exception.InnerException.GetType().ToString(), exception.InnerException.InnerException.Message);
                    _diagnosticContext.Set("Exception", $"{exception.InnerException.InnerException.GetType().ToString()}, {exception.InnerException.InnerException.Message}, {exception.InnerException.InnerException.StackTrace}");
                }
                else
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}", exception.InnerException.GetType().ToString(), exception.InnerException.Message);

                    _diagnosticContext.Set("Exception", $"{exception.InnerException.GetType().ToString()}, {exception.InnerException.Message}, {exception.InnerException.StackTrace}");
                }
            }
            else
            {
                _logger.LogError("{ExceptionType} {ExceptionMessage}", exception.GetType().ToString(), exception.Message);

                _diagnosticContext.Set("Exception", $"{exception.GetType().ToString()}, {exception.Message}, {exception.StackTrace}");
            }
        }
    }
}
