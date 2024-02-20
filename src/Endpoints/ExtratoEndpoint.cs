using Common;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace Endpoints;

public class ExtratoEndpoint
{
    public static async ValueTask<IResult> Handle(
        [FromRoute] int id,
        [FromServices] DbContext dbContext
    ) =>
        await dbContext.ObterExtrato(id) is var extrato && extrato.IsSuccess
            ? Results.Ok(extrato.Value)
            : extrato.Error switch
            {
                _ when extrato.Error == Errors.NotFound => Results.NotFound(),
                _ => Results.UnprocessableEntity()
            };
}
