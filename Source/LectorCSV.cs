using Proyecto_Final_POO_C_.Source.DTOs;
using Proyecto_Final_POO_C_.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Final_POO_C_.Source
{
    public class LectorCsv : IImportarDatos
    {
        public List<ClienteDTO> LeerClientes(string ruta)
        {
            var clientesDTO = new List<ClienteDTO>();

            // id_cliente:   Identificador del cliente.
            // nombre:       Nombre completo(obligatorio, no vacío).
            // email:        Dirección de correo electrónico válida(obligatorio, campo de unión).
            // ciudad:       Ciudad de residencia(puede ser vacía).
            // tipo_cliente: Clasificación("natural" o "empresarial").

            foreach (string linea in File.ReadLines(ruta).Skip(1))
            {
                string[] datos = linea.Split(','); 
                ClienteDTO cliente = new ClienteDTO();

                cliente.IdCliente   = datos[0];
                cliente.Nombre      = datos[1];
                cliente.Email       = datos[2];
                cliente.Ciudad      = datos[3];
                cliente.TipoCliente = datos[4];
                
                clientesDTO.Add(cliente);
            }
            return clientesDTO; 
        }
        public List<PedidoDTO> LeerPedidos(string ruta)
        {
            var pedidos = new List<PedidoDTO>();

            // id_pedido:          Identificador del pedido.
            // email_cliente:      Email del cliente asociado.
            // fecha:              Fecha de compra.
            // tipo_pedido:        Clasificación("nacional" o "internacional").
            // id_producto:        ID del artículo comprado.
            // nombre_producto:    Nombre del artículo.
            // categoria_producto: Categoría del artículo.
            // cantidad:           Cantidad de unidades(entero mayor a cero).
            // precio_unitario:    Precio pactado(decimal mayor a cero).

            foreach (string linea in File.ReadLines(ruta).Skip(1))
            {
                string[] datos = linea.Split(',');
                PedidoDTO pedido = new PedidoDTO();

                pedido.IdPedido          = datos[0];
                pedido.EmailCliente      = datos[1];
                pedido.Fecha             = DateTime.Parse(datos[2]);
                pedido.TipoPedido        = datos[3];
                pedido.IdProducto        = datos[4];
                pedido.NombreProducto    = datos[5];
                pedido.CategoriaProducto = datos[6];
                pedido.Cantidad          = int.Parse(datos[7]);
                pedido.PrecioUnitario    = decimal.Parse(datos[8]);;

                pedidos.Add(pedido);
            }
            return pedidos;
        }
    }
}