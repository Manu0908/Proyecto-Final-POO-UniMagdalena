using System;
using Xunit;
using Proyecto_Final_POO_C_.Source.Modelo;

namespace Proyecto_Final_POO_C_.Tests
{
    public class PruebasDominio
    {
        [Fact]
        public void ValidacionEmailInvalido_LanzaExcepcion()
        {
            // Act & Assert
            var excepcion = Assert.Throws<ClienteInvalidoException>(() =>
                new ClienteNatural("123", "Juan Perez", "email_invalido", "Santa Marta")
            );

            Assert.Contains("El formato del email", excepcion.Message);
        }

        [Fact]
        public void CalcularImpuestoPedidoNacional_RetornaDiecinuevePorciento()
        {
            // Arrange
            var producto = new Producto(100000m, "P1", "Celular", "Tecnologia");
            var pedido = new PedidoNacional("PED-N-01", DateTime.Now, "juan@email.com");
            var item = new PedidoItem(producto, 2, 100000m);
            pedido.Items.Add(item);

            // Act
            decimal subtotal = pedido.CalcularValorSinImpuestos();
            decimal impuesto = pedido.CalcularImpuestoAplicado();
            decimal total = pedido.CalcularValorTotalConImpuestos();

            // Assert
            Assert.Equal(200000m, subtotal);
            Assert.Equal(38000m, impuesto);
            Assert.Equal(238000m, total);
        }

        [Fact]
        public void CalcularImpuestoPedidoInternacional_RetornaTreintaPorciento()
        {
            // Arrange
            var producto = new Producto(100000m, "P1", "Celular", "Tecnologia");
            var pedido = new PedidoInternacional("PED-I-01", DateTime.Now, "juan@email.com");
            var item = new PedidoItem(producto, 2, 100000m);
            pedido.Items.Add(item);

            // Act
            decimal subtotal = pedido.CalcularValorSinImpuestos();
            decimal impuesto = pedido.CalcularImpuestoAplicado();
            decimal total = pedido.CalcularValorTotalConImpuestos();

            // Assert
            Assert.Equal(200000m, subtotal);
            Assert.Equal(60000m, impuesto);
            Assert.Equal(260000m, total);
        }

        [Theory]
        [InlineData(5, false)]
        [InlineData(6, true)]
        public void EsFrecuenteClienteNatural_DependeDeCantidadDeCompras(int cantidadCompras, bool esperadoFrecuente)
        {
            // Arrange
            var cliente = new ClienteNatural("1", "Carlos", "carlos@email.com", "Medellin");

            // Act
            bool esFrecuente = cliente.EsFrecuente(cantidadCompras, 100000m);

            // Assert
            Assert.Equal(esperadoFrecuente, esFrecuente);
        }

        [Theory]
        [InlineData(50000000, false)]
        [InlineData(50000001, true)]
        public void EsFrecuenteClienteEmpresarial_DependeDeTotalInvertido(decimal totalInvertido, bool esperadoFrecuente)
        {
            // Arrange
            var cliente = new ClienteEmpresarial("2", "E-Corp", "info@ecorp.com", "Bogota");

            // Act
            bool esFrecuente = cliente.EsFrecuente(3, totalInvertido);

            // Assert
            Assert.Equal(esperadoFrecuente, esFrecuente);
        }
    }
}
