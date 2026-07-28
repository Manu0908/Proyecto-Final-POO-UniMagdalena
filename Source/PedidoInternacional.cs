// Proyecto Final POO — Clase hija: PedidoInternacional
namespace Proyecto_Final_POO_C_.Source
{
    public class PedidoInternacional : Pedido
    {
        public PedidoInternacional() : base() { }

        public PedidoInternacional(string idpedido, DateTime fechacompra, string emailcliente)
            : base(idpedido, fechacompra, emailcliente)
        {
        }

        // Impuesto del 30% para pedidos con destino u origen internacional.
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