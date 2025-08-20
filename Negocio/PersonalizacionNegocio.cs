using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PersonalizacionNegocio
    {
        private readonly PersonalizacionDatos datos = new();

        public void GuardarPersonalizacion(int idUsuario, int idProducto, string colorPrincipal, string colorSecundario, string colorCordones, decimal precioFinal)
        {
            datos.InsertarPersonalizacion(idUsuario, idProducto, colorPrincipal, colorSecundario, colorCordones, precioFinal);
        }
    }

}
