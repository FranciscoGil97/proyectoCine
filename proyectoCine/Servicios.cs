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

        static ObservableCollection<Pelicula> ObtenPeliculas()
        {
            ObservableCollection<Pelicula> peliculasJson = new ObservableCollection<Pelicula>();
            try
            {
                using (StreamReader jsonStream = File.OpenText("..\\..\\peliculas.json"))
                {
                    var json = jsonStream.ReadToEnd();
                    peliculasJson = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(json);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            return peliculasJson;
        }

        public static void RenuevaPeliculas()
        {
            try
            {
                string urlArchivo = "peliculas.json";

                var cliente = new RestClient(Properties.Settings.Default.urlApi);
                var request = new RestRequest(Method.GET);
                var response = cliente.Execute(request);

                JsonConvert.SerializeObject(response.Content);


                string personasJson = JsonConvert.SerializeObject(response.Content);
                File.WriteAllText(urlArchivo, personasJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
