namespace Models;

public record Extrato(ExtratoSaldo Saldo, ExtratoTransacao[] Ultimas_Transacoes);

public record ExtratoSaldo(DateTime Data_Extrato, int Limite, int Total);

public record ExtratoTransacao(
    int Valor,
    TipoTransacao Tipo,
    string Descricao,
    DateTime Realizada_Em
);
