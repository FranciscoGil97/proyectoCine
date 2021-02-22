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
        static DAOCine _DAOCine = new DAOCine();
        private static ObservableCollection<Sesion> sesiones;
        private static ObservableCollection<Pelicula> peliculas;
        private static ObservableCollection<Salas> salas;
        
        public static ObservableCollection<Pelicula> Peliculas { get { return ObtenPeliculas(); } set => peliculas = value; }
        public static ObservableCollection<Sesion> Sesiones { get => ObtenSesiones(); set => sesiones = value; }
        public static ObservableCollection<Salas> Salas { get { return ObtenSalas(); } set => salas = value; }

        static ObservableCollection<Pelicula> ObtenPeliculas()
        {
            // Mejor obtener los datos desde la BD y desde un fichero
            ObservableCollection<Pelicula> peliculasBD = new ObservableCollection<Pelicula>();
            try
            {
                string fechaActual = DateTime.Now.ToString().Split(' ')[0]; // formato fecha (dd/mm/aaaa HH:MM:SS)
                if (Properties.Settings.Default.FechaUltimaActualizacion != fechaActual)
                {
                    Properties.Settings.Default.FechaUltimaActualizacion = fechaActual;
                    Properties.Settings.Default.Save();
                    RenuevaPeliculas();
                }

                if (!_DAOCine.ExistenPeliculas())
                {
                    RenuevaPeliculas();
                }
                
                peliculasBD = _DAOCine.ObtenPeliculas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Obten películas: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                peliculasBD = new ObservableCollection<Pelicula>();
            }
            return peliculasBD;
        }

        public static void RenuevaPeliculas()
        {
            try
            {
                var cliente = new RestClient(Properties.Settings.Default.urlApi);
                var request = new RestRequest(Method.GET);
                var response = cliente.Execute(request);

                JsonConvert.SerializeObject(response.Content);

                ObservableCollection<Pelicula> peliculasJson = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(response.Content);
                _DAOCine.InsertaPelicula(peliculasJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Renueva peliculas: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void InsertaSalas()
        {
            ObservableCollection<Salas> salasInsertar = new ObservableCollection<Salas>();
            salasInsertar.Add(new Salas(1, true, 90, "A1"));
            salasInsertar.Add(new Salas(2, true, 60, "B2"));
            salasInsertar.Add(new Salas(3, true, 100, "A3"));
            salasInsertar.Add(new Salas(4, false, 80, "A4"));
            salasInsertar.Add(new Salas(5, true, 95, "C5"));

            _DAOCine.InsertaSalas(salasInsertar);
        }

        public static void InsertaSesiones()
        {
            ObservableCollection<Sesion> sesionesInsertar = new ObservableCollection<Sesion>();
            sesionesInsertar.Add(new Sesion(1, Peliculas[0].Id, Salas[0].Id, "20:00"));
            sesionesInsertar.Add(new Sesion(2, Peliculas[2].Id, Salas[2].Id, "18:30"));
            sesionesInsertar.Add(new Sesion(3, Peliculas[1].Id, Salas[3].Id, "19:45"));

            _DAOCine.InsertaSesiones(sesionesInsertar);
        }

        public static void InsertaSala(Salas sala)
        {
            sala.Id = Salas[Salas.Count - 1].Id + 1;
            _DAOCine.InsertaSala(sala);
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
