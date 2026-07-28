// Proyecto Final POO Clase Padre: Pedido
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Proyecto_Final_POO_C_.Source 
{
    public abstract class Pedido
    {   
        private const string PatronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private string _idpedido;
        private string _emailcliente;

        protected Pedido()
        {
            Items = new List<PedidoItem>();
        }

        protected Pedido(string idpedido, DateTime fechacompra, string emailcliente)
        {
            IDPedido = idpedido;
            FechaCompra = fechacompra;
            EmailCliente = emailcliente;
            Items = new List<PedidoItem>();
        }

        public string IDPedido
        {
            get { return _idpedido; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La ID del pedido no puede estar vacía.");
                }
                _idpedido = value;
            }
        }

        public DateTime FechaCompra { get; set; }

        public string EmailCliente
        {
            get { return _emailcliente; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El email del cliente no puede estar vacío.");
                }

                else if (!Regex.IsMatch(value, PatronEmail))
                {
                    throw new ArgumentException("El formato del email no es válido.");
                }

                _emailcliente = value;
            }
        }

        public List<PedidoItem> Items { get; set; }

        public decimal CalcularValorSinImpuestos()
        {
            decimal acumulado = 0m;
            foreach (var item in Items)
            {
                if (item != null)
                {
                    acumulado += item.CalcularSubtotalItem();
                }
            }
            return acumulado;
        }

        public abstract decimal CalcularImpuestoAplicado();
        public abstract decimal CalcularValorTotalConImpuestos();
    }
}