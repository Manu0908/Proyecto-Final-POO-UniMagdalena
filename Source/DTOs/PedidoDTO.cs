using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_Final_POO_C_.Source.DTOs
{
    // La razon de porque todos son string (con un ? para que sea nullable (permita ser nulo)) se desarrolla en cliente DTO
    public class PedidoDTO
    {
        public string? IdPedido { get; set; }
        public string? EmailCliente { get; set; }
        public string? Fecha { get; set; }
        public string? TipoPedido { get; set; }
        public string? IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string? CategoriaProducto { get; set; }
        public string? Cantidad { get; set; }
        public string? PrecioUnitario { get; set; }
    }
}
