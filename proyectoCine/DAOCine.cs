using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace proyectoCine
{
    class DAOCine
    {
        private readonly SqliteConnection conexion;
        private SqliteCommand comando;
        private readonly string crearTablaPeliculas = @"CREATE TABLE IF NOT EXISTS peliculas(
                                                idPelicula      INTEGER PRIMARY KEY,
                                                titulo          TEXT, 
                                                cartel          TEXT, 
                                                año             INTEGER, 
                                                genero          TEXT, 
                                                calificacion    TEXT)";
        private readonly string crearTablaSalas = @"CREATE TABLE IF NOT EXISTS salas(
                                            idSala      INTEGER PRIMARY KEY AUTOINCREMENT,
                                            numero      TEXT, 
                                            capacidad   INTEGER,
                                            disponible  BOOLEAN DEFAULT (true))";
        private readonly string crearTablaSesiones = @"CREATE TABLE IF NOT EXISTS sesiones(
                                            idSesion INTEGER PRIMARY KEY AUTOINCREMENT,
                                            pelicula INTEGER REFERENCES peliculas (idPelicula),
                                            sala     INTEGER REFERENCES salas (idSala),
                                            hora     TEXT)";
        private readonly string crearTablaVentas = @"CREATE TABLE IF NOT EXISTS sesiones(
                                            idVenta  INTEGER PRIMARY KEY AUTOINCREMENT,
                                            sesion   INTEGER REFERENCES sesiones (idSesion),
                                            cantidad INTEGER,
                                            pago     TEXT)";

        public DAOCine()
        {
            conexion = new SqliteConnection("Data source=cine");
            CrearTablas();
        }

        private void CrearTablas()
        {
            conexion.Open();
            comando = conexion.CreateCommand();

            comando.CommandText = crearTablaPeliculas;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaSalas;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaSesiones;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaVentas;
            comando.ExecuteNonQuery();

            conexion.Close();
        }

        public void InsertaPelicula(ObservableCollection<Pelicula> peliculas)
        {
            try
            {

                conexion.Open();
                comando = conexion.CreateCommand();
                comando.CommandText = "DELETE FROM peliculas";
                comando.ExecuteNonQuery();
                comando.Parameters.Add("@id", SqliteType.Integer);
                comando.Parameters.Add("@titulo", SqliteType.Text);
                comando.Parameters.Add("@cartel", SqliteType.Text);
                comando.Parameters.Add("@año", SqliteType.Integer);
                comando.Parameters.Add("@genero", SqliteType.Text);
                comando.Parameters.Add("@calificacion", SqliteType.Text);
                foreach (Pelicula pelicula in peliculas)
                {
                    comando.CommandText = "INSERT INTO peliculas VALUES(@id,@titulo, @cartel,@año,@genero,@calificacion)";


                    comando.Parameters["@id"].Value = pelicula.Id;
                    comando.Parameters["@titulo"].Value = pelicula.Titulo;
                    comando.Parameters["@cartel"].Value = pelicula.Cartel;
                    comando.Parameters["@año"].Value = pelicula.Año;
                    comando.Parameters["@genero"].Value = pelicula.Genero;
                    comando.Parameters["@calificacion"].Value = pelicula.Calificacion;
                    comando.ExecuteNonQuery();
                }


                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
