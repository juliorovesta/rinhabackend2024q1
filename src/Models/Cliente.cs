namespace Models;

public record Cliente(int Id, string Nome, ClienteSaldo Saldo);

public record ClienteSaldo
{
    public int Limite { get; }

    public int Saldo { get; private set; }

    public ClienteSaldo(int limite, int saldo)
    {
        Limite = limite;
        Saldo = saldo;
    }

    public void AtualizarSaldo(int novoSaldo)
    {
        Saldo = novoSaldo;
    }
}
