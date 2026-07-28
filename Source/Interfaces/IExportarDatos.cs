using Proyecto_Final_POO_C_.Source.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Final_POO_C_.Source.Interfaces
{
    public interface IExportarDatos
    {
        void EscribirClientes(List<ClienteDTO> clientesDTO, string ruta);
        void EscribirPedidos(List<PedidoDTO> pedidosDTO, string ruta);
    }
}
