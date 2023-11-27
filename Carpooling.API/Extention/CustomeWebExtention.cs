using System;
using System.Net;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Carpooling.API.Extension
{
    public static class CustomeWebExtention
    {
        public static Action<IApplicationBuilder> HandleExceptions()
        {
            return options =>
            {
                options.Run(
                async context =>
                {
                    var exceptionContext = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionContext.Error.GetType().Name;
                    if(exception.Equals(nameof(EmailIncorrectException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if(exception.Equals(nameof(EmailExistsException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exceptionContext.Error.Message.Equals(nameof(ArgumentException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if(exception.Equals(nameof(AuthorisationException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if(exception.Equals(nameof(UserNotExistException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                    else if(exception.Equals(nameof(UserExistException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(PasswordIncorrectException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(NudeAvatarException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(EntityNotFound)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                    else if(exception.Equals(nameof(UserNameExistException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(TravelDoesNotExistException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                    else if(exception.Equals(nameof(FeedbackException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(InvalidOperationException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(NoApprovedCandidates)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if(exception.Equals(nameof(TravelDoesNotExistException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }

                    if(exceptionContext != null)
                    {
                        var error = exceptionContext.Error.Message;
                        var excepiton = $"Exception message: {exceptionContext.Error.Message}";
                        await context.Response.WriteAsync(excepiton);
                    }
                });
            };
        }
    }
}