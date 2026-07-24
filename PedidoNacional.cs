// Proyecto Final POO Clase Hija: PedidoNacional
namespace Proyecto_Final_POO_C_{
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
}
