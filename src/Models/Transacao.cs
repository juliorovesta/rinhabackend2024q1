using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public record Transacao(int Valor, TipoTransacao Tipo, string Descricao);

[JsonConverter(typeof(JsonStringEnumConverter<TipoTransacao>))]
public enum TipoTransacao
{
    none,
    c,
    d
}
