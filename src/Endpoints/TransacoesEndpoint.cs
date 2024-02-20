using Common;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;

namespace Endpoints;

public class TransacoesEndpoint
{
    public static async ValueTask<IResult> Handle(
        [FromRoute] int id,
        [FromBody] Transacao transacao,
        [FromServices] DbContext dbContext
    )
    {
        if (
            transacao.Valor <= 0
            || (transacao.Tipo != TipoTransacao.c && transacao.Tipo != TipoTransacao.d)
            || string.IsNullOrWhiteSpace(transacao.Descricao)
            || transacao.Descricao.Length > 10
        )
            return Results.UnprocessableEntity();

        if (
            await dbContext.CriarTransacao(id, transacao) is var clienteSaldo
            && clienteSaldo.IsSuccess
        )
            return Results.Ok(clienteSaldo.Value);

        return clienteSaldo.Error switch
        {
            _ when clienteSaldo.Error == Errors.NotFound => Results.NotFound(),
            _ => Results.UnprocessableEntity()
        };
    }
}
