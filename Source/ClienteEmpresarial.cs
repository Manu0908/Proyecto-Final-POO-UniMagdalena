// Proyecto Final POO — Clase hija: ClienteEmpresarial
namespace Proyecto_Final_POO_C_
{
    public class ClienteEmpresarial : Cliente
    {
        public ClienteEmpresarial(string id, string nombre, string email, string ciudad)
            : base(id, nombre, email, ciudad)
        {
        }

        // Es frecuente si el total acumulado de sus compras supera los $50.000.000 COP.
        public override bool EsFrecuente(int cantidadCompras, decimal totalInvertido)
        {
            return totalInvertido > 50000000m;
        }
    }
}