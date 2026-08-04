using System;

namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public class PedidoItem
    {
        private int _cantidad;
        private decimal _preciounitario;

        private PedidoItem() { }

        public PedidoItem(Producto productoAsociado, int cantidad, decimal preciounitario)
        {
            ProductoAsociado = productoAsociado
                ?? throw new PedidoInvalidoException("El producto asociado no puede ser nulo.", nameof(productoAsociado));

            Cantidad = cantidad;
            PrecioUnitario = preciounitario;
        }

        public Producto ProductoAsociado { get; set; }

        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                if (value <= 0)
                {
                    throw new PedidoInvalidoException("La cantidad comprada debe ser mayor a cero.", nameof(value));
                }
                _cantidad = value;
            }
        }

        public decimal PrecioUnitario
        {
            get { return _preciounitario; }
            set
            {
                if (value <= 0)
                {
                    throw new PedidoInvalidoException("El precio unitario del item debe ser mayor a cero.", nameof(value));
                }
                _preciounitario = value;
            }
        }

        public decimal CalcularSubtotalItem()
        {
            return Cantidad * _preciounitario;
        }
    }
}