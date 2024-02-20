using Common;
using Common.Types;
using Models;
using Npgsql;

namespace Persistence;

public class DbContext : IAsyncDisposable
{
    private readonly NpgsqlConnection _connection;
    private bool _disposed;

    public DbContext(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<Result<Extrato>> ObterExtrato(int clienteId)
    {
        await OpenConnectionIfClosed();

        await using var cmd = new NpgsqlCommand(
            @"SELECT
                r_result_code,
                r_cliente_limite,
                r_cliente_saldo,
                r_tran_valor,
                r_tran_tipo,
                r_tran_descricao,
                r_tran_realizada_em,
                r_tran_count
            FROM public.obter_extrato(@clienteId)",
            _connection
        );
        cmd.Parameters.AddWithValue("clienteId", clienteId);
        await using var reader = await cmd.ExecuteReaderAsync();

        Result<string> resultCode = await reader.ReadAsync()
            ? reader.GetString(reader.GetOrdinal("r_result_code"))
            : Errors.Unknown.Code;

        if (resultCode.IsFailure || resultCode.Value != "[OK]")
            return resultCode.Match<Extrato>(code => Errors.FromCode(code));

        var extratoSaldo = new ExtratoSaldo(
            DateTime.UtcNow,
            reader.GetInt32(reader.GetOrdinal("r_cliente_limite")),
            reader.GetInt32(reader.GetOrdinal("r_cliente_saldo"))
        );

        if (
            reader.GetInt32(reader.GetOrdinal("r_tran_count")) is int quantidade_transacoes
            && quantidade_transacoes == 0
        )
            return new Extrato(extratoSaldo!, []);

        var extratoUltimasTransacoes = new ExtratoTransacao[quantidade_transacoes];

        var transacao_index = 0;
        do
        {
            extratoUltimasTransacoes[transacao_index] = new ExtratoTransacao(
                reader.GetInt32(reader.GetOrdinal("r_tran_valor")),
                reader.GetChar(reader.GetOrdinal("r_tran_tipo")) == 'c'
                    ? TipoTransacao.c
                    : TipoTransacao.d,
                reader.GetString(reader.GetOrdinal("r_tran_descricao")),
                reader.GetDateTime(reader.GetOrdinal("r_tran_realizada_em"))
            );
            transacao_index++;
        } while (await reader.ReadAsync());

        return new Extrato(extratoSaldo, extratoUltimasTransacoes);
    }

    public async ValueTask<Result<ClienteSaldo>> CriarTransacao(int clienteId, Transacao transacao)
    {
        await OpenConnectionIfClosed();

        await using var tran = await _connection.BeginTransactionAsync(
            System.Data.IsolationLevel.ReadCommitted
        );
        await using var cmd = new NpgsqlCommand(
            @"SELECT
                r_result_code,
                r_cliente_limite,
                r_cliente_saldo
            FROM public.criar_transacao(@clienteId, @valor, @tipo, @descricao, @realizadaEm)",
            _connection,
            tran
        );
        cmd.Parameters.AddWithValue("clienteId", clienteId);
        cmd.Parameters.AddWithValue("valor", transacao.Valor);
        cmd.Parameters.AddWithValue("tipo", transacao.Tipo.ToString());
        cmd.Parameters.AddWithValue("descricao", transacao.Descricao);
        cmd.Parameters.AddWithValue("realizadaEm", DateTime.UtcNow);
        await using var reader = await cmd.ExecuteReaderAsync(
            System.Data.CommandBehavior.SingleRow
        );

        Result<string> resultCode = await reader.ReadAsync()
            ? reader.GetString(reader.GetOrdinal("r_result_code"))
            : Errors.Unknown.Code;

        var clienteSaldo = resultCode.Match<ClienteSaldo>(code =>
            code == "[OK]"
                ? new ClienteSaldo(
                    reader.GetInt32(reader.GetOrdinal("r_cliente_limite")),
                    reader.GetInt32(reader.GetOrdinal("r_cliente_saldo"))
                )
                : Errors.FromCode(code)
        );

        await reader.CloseAsync();

        await (clienteSaldo.IsSuccess ? tran.CommitAsync() : tran.RollbackAsync());

        return clienteSaldo;
    }

    private async ValueTask OpenConnectionIfClosed()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
            return;

        await _connection.OpenAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        await _connection.DisposeAsync();

        _disposed = true;
    }
}
