// Proyecto Final POO — Interfaz: IImportarDatos
// Contrato del patrón Strategy para la lectura de archivos de entrada.
// Cada implementación concreta (LectorCSV, LectorJSON) lee el mismo formato de datos
// sin que el resto del sistema sepa cómo se leen físicamente.
using Proyecto_Final_POO_C_.Source.DTOs;

namespace Proyecto_Final_POO_C_.Source.Interfaces
{
    public interface IImportarDatos
    {
        // Lee el archivo de clientes en la ruta indicada y devuelve una lista de DTOs crudos.
        List<ClienteDTO> LeerClientes(string ruta);

        // Lee el archivo de pedidos en la ruta indicada y devuelve una lista de ítems crudos.
        // Nota: un pedido con N ítems genera N entradas en la lista (estructura plana del archivo).
        List<PedidoItemDTO> LeerPedidos(string ruta);
    }
}
