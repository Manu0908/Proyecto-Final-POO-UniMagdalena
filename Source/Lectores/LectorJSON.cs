// Proyecto Final POO — Estrategia concreta: LectorJSON
// Implementa IImportarDatos para leer archivos en formato JSON.
// Pendiente de implementar (paso 4 del plan de trabajo).
using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;

namespace Proyecto_Final_POO_C_.Source.Lectores
{
    public class LectorJson : IImportarDatos
    {
        public List<ClienteDTO> LeerClientes(string ruta)
        {
            // TODO: implementar deserialización JSON con System.Text.Json.
            throw new NotImplementedException("LectorJson.LeerClientes aún no está implementado.");
        }

        public List<PedidoItemDTO> LeerPedidos(string ruta)
        {
            // TODO: implementar deserialización JSON con System.Text.Json.
            throw new NotImplementedException("LectorJson.LeerPedidos aún no está implementado.");
        }
    }
}
