using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace proyectoCine
{
    class MainWindowVM
    {
        public ObservableCollection<Pelicula> Peliculas { get; private set; }
        public ObservableCollection<Salas> Salas { get; set; }
        public ObservableCollection<Sesion> Sesiones { get; set; }
        public ObservableCollection<Ventas> Ventas { get; set; }
        public Salas SalaSeleccionada { get; set; }
        public Sesion SesionSeleccionada { get; set; }

        public MainWindowVM()
        {
            ActualizaVista();
        }

        public void ActualizaVista()
        {
            Peliculas = Servicios.Peliculas;
            Salas = Servicios.Salas;
            Sesiones = Servicios.Sesiones;
            Ventas = Servicios.Ventas;
        }
    }
}
