// Proyecto Final POO Clase Normal: Producto y Clase Componente: PedidoItem
using System;

namespace EcommercePipeline.Dominio 
{
    public class Producto
    {
        private decimal _preciounitario;
        private string _idproducto;
        private string _nombreproducto;
        private string _categoria;

        private Producto() {}

        public Producto(decimal preciounitario, string idproducto, string nombreproducto, string categoria)
        {   
            PrecioUnitario = preciounitario;
            IDProducto = idproducto;
            NombreProducto = nombreproducto;
            Categoria = categoria;
            NumeroVentas = 0;
        }

        public decimal PrecioUnitario
        {
            get { return _preciounitario; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "El precio por unidad debe ser mayor a cero.");
                }
                _preciounitario = value;
            }
        }

        public string IDProducto
        {
            get { return _idproducto; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La id del producto no puede estar vacia.");
                }
                _idproducto = value;
            }
        }

        public string NombreProducto
        {
            get { return _nombreproducto; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre del producto no puede estar vacio.");
                }
                _nombreproducto = value;
            }
        }

        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }

        public int NumeroVentas { get; set; }
    }

    public class PedidoItem
    {
        private int _cantidad;
        private decimal _preciounitario;

        private PedidoItem() {}

        public PedidoItem(Producto productoAsociado, int cantidad, decimal preciounitario)
        {
            ProductoAsociado = productoAsociado ?? throw new ArgumentNullException(nameof(productoAsociado), "El producto asociado no puede ser nulo.");

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
                    throw new ArgumentOutOfRangeException(nameof(value), "La cantidad comprada debe ser mayor a cero.");
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
                    throw new ArgumentOutOfRangeException(nameof(value), "El precio unitario del item debe ser mayor a cero.");
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