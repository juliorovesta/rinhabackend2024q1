using Common.Types;

namespace Common;

public static class Errors
{
    public static readonly Error Unknown = new("[UnknownError]", "Erro não conhecido.");

    public static readonly Error NotFound = new("[NOT_FOUND]", "Cliente não encontrado.");

    public static readonly Error LimitExceeded =
        new("[LIMIT_EXCEEDED]", "A transação excede o limite de saldo permitido para o cliente.");

    public static Error FromCode(string code) =>
        code switch
        {
            _ when code == Errors.NotFound.Code => Errors.NotFound,
            _ when code == Errors.LimitExceeded.Code => Errors.LimitExceeded,
            _ => Errors.Unknown
        };
}
