namespace Common.Http;

public static class CustomHttpResponseStatusCode
{
    public static IApplicationBuilder UseStatusCode422ForBadRequest(this IApplicationBuilder app) =>
        app.Use(
            async (context, next) =>
            {
                try
                {
                    await next(context);

                    if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                        context.Response.SetStatus422UnprocessableEntity();
                }
                catch (BadHttpRequestException)
                {
                    context.Response.SetStatus422UnprocessableEntity();
                }
            }
        );

    public static void SetStatus422UnprocessableEntity(this HttpResponse response) =>
        response.StatusCode = StatusCodes.Status422UnprocessableEntity;
}
