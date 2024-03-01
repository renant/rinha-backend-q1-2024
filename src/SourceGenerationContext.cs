using RinhaBackEnd2024.Models;
using System.Text.Json.Serialization;

namespace RinhaBackEnd2024
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Extrato))]
    [JsonSerializable(typeof(TransacaoRequest))]
    [JsonSerializable(typeof(TransacaoResult))]
    [JsonSerializable(typeof(Transacao))]
    public partial class SourceGenerationContext
        : JsonSerializerContext
    { }
}
