using System.Text.Json.Serialization;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class ClienteDTO
    {
        [JsonPropertyName("id_cliente")]
        public string? IdCliente    { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre       { get; set; }

        [JsonPropertyName("email")]
        public string? Email        { get; set; }

        [JsonPropertyName("ciudad")]
        public string? Ciudad       { get; set; }

        [JsonPropertyName("tipo_cliente")]
        public string? TipoCliente  { get; set; }
    }
}
