namespace Proyecto_Final_POO_C_.Source.Modelo
{
    public class ClienteEmpresarial : Cliente
    {
        public ClienteEmpresarial(string id, string nombre, string email, string ciudad)
            : base(id, nombre, email, ciudad)
        {
        }

        public override bool EsFrecuente(int cantidadCompras, decimal totalInvertido)
        {
            return totalInvertido > 50000000m;
        }
    }
}