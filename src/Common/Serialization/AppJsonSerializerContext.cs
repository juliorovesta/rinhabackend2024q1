using System.Text.Json.Serialization;
using Models;

namespace Common.Serialization;

[JsonSerializable(typeof(Cliente))]
[JsonSerializable(typeof(ClienteSaldo))]
[JsonSerializable(typeof(Transacao))]
[JsonSerializable(typeof(ExtratoTransacao))]
[JsonSerializable(typeof(ExtratoSaldo))]
[JsonSerializable(typeof(Extrato))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
