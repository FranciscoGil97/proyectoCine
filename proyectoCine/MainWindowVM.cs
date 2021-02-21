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
        public Salas salaSeleccionada;
        public Sesion sesionSeleccionada;

        public Salas SalaSeleccionada
        {
            get { return salaSeleccionada; }
            set
            {
                if (salaSeleccionada!=null && salaSeleccionada != value)
                    Servicios.ActualizaSala(salaSeleccionada);
                salaSeleccionada = value;

            }
        }


        public MainWindowVM()
        {
            Peliculas = Servicios.Peliculas;
            Salas = Servicios.Salas;
            Sesiones = Servicios.Sesiones;
        }

        public void ActualizaVista()
        {
            Peliculas = Servicios.Peliculas;
            Salas = Servicios.Salas;
            Sesiones = Servicios.Sesiones;
        }
    }
}
