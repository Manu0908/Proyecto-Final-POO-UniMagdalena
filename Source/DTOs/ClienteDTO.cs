// Proyecto Final POO — DTO de entrada: ClienteDTO
// Reflejo plano y crudo de una fila del archivo de clientes (CSV o JSON).
// Todos los campos son string? porque en este punto los datos aún no han sido validados.
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

        // "natural" o "empresarial" — se valida y diferencia en la capa de mapeo.
        [JsonPropertyName("tipo_cliente")]
        public string? TipoCliente  { get; set; }
    }
}
