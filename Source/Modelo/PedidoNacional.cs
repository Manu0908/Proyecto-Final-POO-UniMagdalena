// Proyecto Final POO — Clase hija: PedidoNacional
namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public class PedidoNacional : Pedido
    {
        public PedidoNacional() : base() { }

        public PedidoNacional(string idpedido, DateTime fechacompra, string emailcliente)
            : base(idpedido, fechacompra, emailcliente)
        {
        }

        // Impuesto del 19% para pedidos dentro del territorio nacional.
        public override decimal CalcularImpuestoAplicado()
        {
            return CalcularValorSinImpuestos() * 0.19m;
        }

        public override decimal CalcularValorTotalConImpuestos()
        {
            return CalcularValorSinImpuestos() * 1.19m;
        }
    }
}
