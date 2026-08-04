using System.Collections.Generic;
using Proyecto_Final_POO_C_.Source.DTOs;

namespace Proyecto_Final_POO_C_.Source.Interfaces
{
    public interface IImportarDatos
    {
        List<ClienteDTO> LeerClientes(string ruta);
        List<PedidoItemDTO> LeerPedidos(string ruta);
    }
}
