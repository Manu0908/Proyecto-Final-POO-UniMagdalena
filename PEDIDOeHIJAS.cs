// Proyecto Final POO Clase Padre e Hijas: Pedido , PedidoNacional y PedidoInternacional
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EcommercePipeline.Dominio
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

    public class PedidoNacional : Pedido
    {
        public PedidoNacional() : base() { }

        public PedidoNacional(string idpedido, DateTime fechacompra, string emailcliente) 
            : base(idpedido, fechacompra, emailcliente)
        {
        }

        public override decimal CalcularImpuestoAplicado()
        {
            return CalcularValorSinImpuestos() * 0.19m;
        }

        public override decimal CalcularValorTotalConImpuestos()
        {
            return CalcularValorSinImpuestos() * 1.19m;
        }
    }

    public class PedidoInternacional : Pedido
    {
        public PedidoInternacional() : base() { }

        public PedidoInternacional(string idpedido, DateTime fechacompra, string emailcliente) 
            : base(idpedido, fechacompra, emailcliente)
        {
        }

        public override decimal CalcularImpuestoAplicado()
        {
            return CalcularValorSinImpuestos() * 0.30m;
        }

        public override decimal CalcularValorTotalConImpuestos()
        {
            return CalcularValorSinImpuestos() * 1.30m;
        }
    }
}