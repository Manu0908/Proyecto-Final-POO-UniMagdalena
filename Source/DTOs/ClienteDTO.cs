// Proyecto Final POO — DTO de entrada: ClienteDTO
// Reflejo plano y crudo de una fila del archivo de clientes (CSV o JSON).
// Todos los campos son string? porque en este punto los datos aún no han sido validados.
namespace Proyecto_Final_POO_C_.Source.DTOs
{
    public class ClienteDTO
    {
        public string? IdCliente    { get; set; }
        public string? Nombre       { get; set; }
        public string? Email        { get; set; }
        public string? Ciudad       { get; set; }

        // "natural" o "empresarial" — se valida y diferencia en la capa de mapeo.
        public string? TipoCliente  { get; set; }
    }
}
