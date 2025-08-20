using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PedidoNegocio
    {
        private PedidoDatos datos = new PedidoDatos();

        public void AgregarProductoAlPedido(int idUsuario, int idProducto)
        {
            int idPedido = datos.ObtenerPedidoPendiente(idUsuario);
            if (idPedido == 0)
            {
                idPedido = datos.CrearPedidoPendiente(idUsuario);
            }

            datos.AgregarDetallePedido(idPedido, idProducto);
        }

        public void AprobarPedido(int idUsuario)
        {
            datos.AprobarPedido(idUsuario);
        }

        public void EliminarDetalle(int idUsuario, string nombreProducto)
        {
            datos.EliminarDetallePedido(idUsuario, nombreProducto);
        }

    }

}
