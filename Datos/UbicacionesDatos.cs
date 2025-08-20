using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class UbicacionesDatos
    {
        private readonly string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";

        // Obtener Países
        public List<Ubicacion> ObtenerPaises()
        {
            List<Ubicacion> paises = new List<Ubicacion>();
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("Luis.ObtenerPaises", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    paises.Add(new Ubicacion
                    {
                        Id = Convert.ToInt32(reader["id_ubicacion"]),
                        Nombre = reader["nombre"].ToString()
                    });
                }
            }
            return paises;
        }

        // Obtener Provincias por País
        public List<Ubicacion> ObtenerProvincias(int paisId)
        {
            List<Ubicacion> provincias = new List<Ubicacion>();
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("Luis.ObtenerProvincias", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaisId", paisId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    provincias.Add(new Ubicacion
                    {
                        Id = Convert.ToInt32(reader["id_ubicacion"]),
                        Nombre = reader["nombre"].ToString()
                    });
                }
            }
            return provincias;
        }

        // Obtener Cantones por Provincia
        public List<Ubicacion> ObtenerCantones(int provinciaId)
        {
            List<Ubicacion> cantones = new List<Ubicacion>();
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("Luis.ObtenerCantones", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProvinciaId", provinciaId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cantones.Add(new Ubicacion
                    {
                        Id = Convert.ToInt32(reader["id_ubicacion"]),
                        Nombre = reader["nombre"].ToString()
                    });
                }
            }
            return cantones;
        }

        // Obtener Distritos por Cantón
        public List<Ubicacion> ObtenerDistritos(int cantonId)
        {
            List<Ubicacion> distritos = new List<Ubicacion>();
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("Luis.ObtenerDistritos", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CantonId", cantonId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    distritos.Add(new Ubicacion
                    {
                        Id = Convert.ToInt32(reader["id_ubicacion"]),
                        Nombre = reader["nombre"].ToString()
                    });
                }
            }
            return distritos;
        }
    }

    public class Ubicacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
