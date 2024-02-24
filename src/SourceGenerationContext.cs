using RinhaBackEnd2024.Models;
using System.Text.Json.Serialization;

namespace RinhaBackEnd2024
{
    [JsonSerializable(typeof(Extrato))]
    [JsonSerializable(typeof(TransacaoRequest))]
    [JsonSerializable(typeof(TransacaoResult))]
    public partial class SourceGenerationContext
        : JsonSerializerContext
    { }
}
