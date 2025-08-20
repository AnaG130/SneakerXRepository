using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PagosNegocio
    {
        private PagosDatos datos = new PagosDatos();

        // Realiza una transacción SINPE entre dos teléfonos
        public void RealizarPagoSINPE(string telefonoEmisor, string telefonoReceptor, decimal monto, string detalle)
        {
            // Validaciones adicionales podrían ir aquí, por ejemplo:
            if (monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.");

            // Llamamos al método de PagosDatos para realizar la transacción
            datos.RealizarSINPE(telefonoEmisor, telefonoReceptor, monto, detalle);
        }

        // Obtiene las tarjetas asociadas a la cédula del usuario
        public DataTable ObtenerTarjetasPorCedula(string cedulaDueno)
        {
            // Validación de la cédula
            if (string.IsNullOrWhiteSpace(cedulaDueno))
                throw new ArgumentException("La cédula no puede estar vacía.");

            // Llamamos al método de PagosDatos para obtener las tarjetas
            return datos.ObtenerTarjetasPorCedula(cedulaDueno);
        }

        // Realiza una compra con tarjeta
        public void RealizarCompraConTarjeta(string numeroTarjeta, decimal monto, string detalle)
        {
            // Validaciones de los parámetros
            if (monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(numeroTarjeta))
                throw new ArgumentException("El número de tarjeta no puede estar vacío.");

            // Llamamos al método de PagosDatos para realizar la compra
            datos.RealizarCompraTarjeta(numeroTarjeta, monto, detalle);
        }
    }
}
