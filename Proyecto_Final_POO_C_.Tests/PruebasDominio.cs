using System;
using Xunit;
using Proyecto_Final_POO_C_.Source;

namespace Proyecto_Final_POO_C_.Tests
{
    public class PruebasDominio
    {
        // 1. Validación de dato de dominio inválido: Email inválido lanza excepción
        [Fact]
        public void ValidacionEmailInvalido_LanzaExcepcion()
        {
            // Act & Assert
            var excepcion = Assert.Throws<ArgumentException>(() =>
                new ClienteNatural("123", "Juan Perez", "email_invalido", "Santa Marta")
            );

            Assert.Contains("El formato del email", excepcion.Message);
        }

        // 2. Cálculo de negocio: Impuesto del 19% para Pedido Nacional
        [Fact]
        public void CalcularImpuestoPedidoNacional_RetornaDiecinuevePorciento()
        {
            // Arrange
            var producto = new Producto(100000m, "P1", "Celular", "Tecnologia");
            var pedido = new PedidoNacional("PED-N-01", DateTime.Now, "juan@email.com");
            var item = new PedidoItem(producto, 2, 100000m); // Total sin imp. = 200,000
            pedido.Items.Add(item);

            // Act
            decimal subtotal = pedido.CalcularValorSinImpuestos();
            decimal impuesto = pedido.CalcularImpuestoAplicado();
            decimal total = pedido.CalcularValorTotalConImpuestos();

            // Assert
            Assert.Equal(200000m, subtotal);
            Assert.Equal(38000m, impuesto); // 19% of 200,000
            Assert.Equal(238000m, total);
        }

        // 3. Cálculo de negocio: Impuesto del 30% para Pedido Internacional
        [Fact]
        public void CalcularImpuestoPedidoInternacional_RetornaTreintaPorciento()
        {
            // Arrange
            var producto = new Producto(100000m, "P1", "Celular", "Tecnologia");
            var pedido = new PedidoInternacional("PED-I-01", DateTime.Now, "juan@email.com");
            var item = new PedidoItem(producto, 2, 100000m); // Total sin imp. = 200,000
            pedido.Items.Add(item);

            // Act
            decimal subtotal = pedido.CalcularValorSinImpuestos();
            decimal impuesto = pedido.CalcularImpuestoAplicado();
            decimal total = pedido.CalcularValorTotalConImpuestos();

            // Assert
            Assert.Equal(200000m, subtotal);
            Assert.Equal(60000m, impuesto); // 30% of 200,000
            Assert.Equal(260000m, total);
        }

        // 4. Comportamiento intercambiable: Cliente Natural es frecuente con más de 5 compras
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

        // 5. Comportamiento intercambiable: Cliente Empresarial es frecuente con inversión > $50,000,000 COP
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
