using System.Collections.Generic;
using Datos;

namespace Negocio
{
    public class UbicacionesNegocio
    {
        private readonly UbicacionesDatos datos = new UbicacionesDatos();

        public List<Ubicacion> ObtenerPaises()
        {
            return datos.ObtenerPaises();
        }

        public List<Ubicacion> ObtenerProvincias(int paisId)
        {
            return datos.ObtenerProvincias(paisId);
        }

        public List<Ubicacion> ObtenerCantones(int provinciaId)
        {
            return datos.ObtenerCantones(provinciaId);
        }

        public List<Ubicacion> ObtenerDistritos(int cantonId)
        {
            return datos.ObtenerDistritos(cantonId);
        }
    }
}
