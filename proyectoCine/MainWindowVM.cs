using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    class MainWindowVM
    {
        public ObservableCollection<Pelicula> Peliculas { get;}
        DAOCine _DAOCine = new DAOCine();
        public MainWindowVM()
        {
            string fechaActual = DateTime.Now.ToString().Split(' ')[0];
            if (Properties.Settings.Default.FechaUltimaActualizacion != fechaActual)
            {
                Properties.Settings.Default.FechaUltimaActualizacion = fechaActual;
                Properties.Settings.Default.Save();
                Servicios.RenuevaPeliculas();
            }

            Peliculas = Servicios.Peliculas;
            _DAOCine.InsertaPelicula(Peliculas);
        }
    }
}
