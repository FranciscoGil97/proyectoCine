using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace proyectoCine
{
    static class Servicios
    {
        public static ObservableCollection<Pelicula> Peliculas => ObtenPeliculas();
        public static ObservableCollection<Sesion> Sesiones => ObtenSesiones();

        public static ObservableCollection<Salas> Salas => ObtenSalas();
        static DAOCine _DAOCine = new DAOCine();

        static ObservableCollection<Pelicula> ObtenPeliculas()
        {
            // Mejor obtener los datos desde la BD y desde un fichero
            ObservableCollection<Pelicula> peliculas = null;
            try
            {
                string fechaActual = DateTime.Now.ToString().Split(' ')[0]; // formato fecha (dd/mm/aaaa HH:MM:SS)
                if (Properties.Settings.Default.FechaUltimaActualizacion != fechaActual)
                {
                    Properties.Settings.Default.FechaUltimaActualizacion = fechaActual;
                    Properties.Settings.Default.Save();
                    RenuevaPeliculas();
                }

                peliculas = _DAOCine.ObtenPeliculas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Obten películas: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                peliculas = new ObservableCollection<Pelicula>();
            }
            return peliculas;
        }

        public static void RenuevaPeliculas()
        {
            try
            {
                var cliente = new RestClient(Properties.Settings.Default.urlApi);
                var request = new RestRequest(Method.GET);
                var response = cliente.Execute(request);

                JsonConvert.SerializeObject(response.Content);

                ObservableCollection<Pelicula> peliculas = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(response.Content);
                InsertaPeliculas(peliculas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Renueva peliculas: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void InsertaPeliculas(ObservableCollection<Pelicula> peliculas)
        {
            _DAOCine.InsertaPelicula(Peliculas);
        }

        public static void InsertaSalas()
        {
            ObservableCollection<Salas> salas = new ObservableCollection<Salas>();
            salas.Add(new Salas(1, true, 90, "A1"));
            salas.Add(new Salas(2, true, 60, "B2"));
            salas.Add(new Salas(3, true, 100, "A3"));
            salas.Add(new Salas(4, false, 80, "A4"));
            salas.Add(new Salas(5, true, 95, "C5"));

            _DAOCine.InsertaSalas(salas);
        }

        public static void InsertaSesiones()
        {
            ObservableCollection<Sesion> sesiones = new ObservableCollection<Sesion>();
            sesiones.Add(new Sesion(1, Peliculas[0].Id, Salas[0].Id, "20:00"));
            sesiones.Add(new Sesion(2, Peliculas[2].Id, Salas[2].Id, "18:30"));
            sesiones.Add(new Sesion(3, Peliculas[1].Id, Salas[3].Id, "19:45"));

            _DAOCine.InsertaSesiones(sesiones);
        }

        private static ObservableCollection<Salas> ObtenSalas()
        {
            if (!_DAOCine.ExistenSalas())// si todavia no se han creado las salas se crean
                InsertaSalas();

            return _DAOCine.ObtenSalas();
        }

        private static ObservableCollection<Sesion> ObtenSesiones()
        {
            if (!_DAOCine.ExistenSesiones())// si todavia no se han creado las salas se crean
                InsertaSesiones();

            return _DAOCine.ObtenSesiones();
        }

        public static void ActualizaSala(Salas sala)
        {
            _DAOCine.ActualizaSala(sala);
        }
    }
}
