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
        public ObservableCollection<Pelicula> Peliculas { get; private set; }
        public ObservableCollection<Salas> Salas { get; set; }
        public ObservableCollection<Sesion> Sesiones { get; set; }
        private Salas salaSeleccionada;

        public Salas SalaSeleccionada
        {
            get { return salaSeleccionada; }
            set { salaSeleccionada = value; }
        }


        public MainWindowVM()
        {
            Peliculas = Servicios.Peliculas;
            Salas = Servicios.Salas;
            Sesiones = Servicios.Sesiones;
        }

    }
}
