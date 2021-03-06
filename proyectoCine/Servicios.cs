﻿using Microsoft.Win32;
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

        public static ObservableCollection<Pelicula> Peliculas { get => ObtenPeliculas(); }
        public static ObservableCollection<Sesion> Sesiones { get => ObtenSesiones(); }
        public static ObservableCollection<Salas> Salas { get => ObtenSalas(); }
        public static ObservableCollection<Ventas> Ventas { get => ObtenerVentas(); }
        public static ObservableCollection<OcupacionSalas> OcupacionSalas { get => ObtenOcupacionSalas(); }

        private static ObservableCollection<OcupacionSalas> ObtenOcupacionSalas()
        {
            return _DAOCine.ObtenOcupacion();
        }

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

        public static ObservableCollection<Ventas> ObtenerVentas()
        {
            return _DAOCine.ObtenVentas();
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
            sesionesInsertar.Add(new Sesion(1, Peliculas[0], Salas[1], "20:00"));
            sesionesInsertar.Add(new Sesion(2, Peliculas[3], Salas[2], "17:00"));
            sesionesInsertar.Add(new Sesion(3, Peliculas[1], Salas[0], "19:00"));

            _DAOCine.InsertaSesiones(sesionesInsertar);
        }

        public static void InsertaSala(Salas sala)
        {
            sala.Id = Salas[Salas.Count - 1].Id + 1;
            _DAOCine.InsertaSala(sala);
        }

        public static void InsertaSesion(Sesion sesion)
        {
            _DAOCine.InsertaSesion(sesion);
        }

        public static void ActualizaSala(Salas sala)
        {
            _DAOCine.ActualizaSala(sala);
        }

        public static void ActualizaSesion(Sesion sesion)
        {
            _DAOCine.ActualizaSesion(sesion);
        }

        public static void EliminarSesion(Sesion sesion)
        {
            _DAOCine.EliminaSesion(sesion);
        }

        public static void InsertaVenta(Ventas venta)
        {
            _DAOCine.InsertaVenta(venta);
        }
    }
}
