using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    public class DAOCine
    {
        private SqliteConnection conexion;
        private SqliteCommand comando;
        private readonly string crearTablaPeliculas = @"CREATE TABLE IF NOT EXISTS peliculas(
                                                idPelicula INTEGER PRIMARY KEY,
                                                titulo          TEXT, 
                                                cartel          TEXT, 
                                                año             TEXT, 
                                                genero          TEXT, 
                                                calificacion    TEXT)";
        private readonly string crearTablaSalas = @"CREATE TABLE IF NOT EXISTS salas(
                                            idSala      INTEGER PRIMARY KEY AUTOINCREMENT,
                                            numero      TEXT, 
                                            capacidad   INTEGER,
                                            disponible  BOOLEAN DEFAULT true)";
        private readonly string crearTablaSesiones = @"CREATE TABLE IF NOT EXISTS sesiones(
                                            idSesion    INTEGER PRIMARY KEY AUTOINCREMENT,
                                            pelicula    INTEGER,
                                            sala        INTEGER,
                                            hora        TEXT
                                            FOREIGN KEY (pelicula) REFERENCES peliculas (idPelicula)
                                            FOREIGN KEY (sala) REFERENCES salas (idSala))";
        private readonly string crearTablaVentas = @"CREATE TABLE IF NOT EXISTS sesiones(
                                            idVenta    INTEGER PRIMARY KEY AUTOINCREMENT,
                                            sesion      INTEGER,
                                            cantidad    INTEGER,
                                            pago        TEXT
                                            FOREIGN KEY (sesion) REFERENCES sesiones (idSesion))";

        public DAOCine()
        {
            conexion = new SqliteConnection("Data source=cine.db");
            comando = conexion.CreateCommand();
            CrearTablas();
        }

        private void CrearTablas()
        {
            comando.CommandText = crearTablaPeliculas;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaSalas;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaSesiones;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaVentas;
            comando.ExecuteNonQuery();
        }

        public void InsertaPelicula()
        {

        }

    }
}
