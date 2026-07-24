using Proyecto_Final_POO_C_.Source.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Final_POO_C_.Source.Interfaces
{
    public interface IImportarDatos
    {
        List<ClienteDTO> LeerClientes(string ruta);
        List<PedidoDTO> LeerPedidos(string ruta);
    }
}
