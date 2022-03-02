using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using moosik.services.Exceptions;
using MimeTypes = System.Net.Mime.MediaTypeNames;

namespace moosik.api.Exception

{
    [ExcludeFromCodeCoverage]
    internal class GeneralExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GeneralExceptionFilter(IWebHostEnvironment hostingEnvironment) =>
            _hostingEnvironment = hostingEnvironment;

        public void OnException(ExceptionContext context)
        {
            if (context == null) return;

            var response = context.HttpContext.Response;
            response.StatusCode = (int) RetrieveStatusCodeForException(context.Exception);
            response.ContentType = MimeTypes.Application.Json;
            context.Result = new JsonResult(new
            {
                error = new[] {context.Exception.Message},
                stackTrace = _hostingEnvironment.IsDevelopment() ? context.Exception.StackTrace : string.Empty,
            });
        }

        private static HttpStatusCode RetrieveStatusCodeForException(System.Exception exception)
        {
            if (exception is ArgumentException) return HttpStatusCode.BadRequest;
            if (exception is NotFoundException) return HttpStatusCode.NotFound;

            return HttpStatusCode.InternalServerError;
        }
    }
}