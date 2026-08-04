using System;

namespace Proyecto_Final_POO_C_.Source.Modelo
{
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