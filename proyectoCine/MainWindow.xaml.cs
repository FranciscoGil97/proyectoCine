using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace proyectoCine
{
    public partial class MainWindow : Window
    {
        MainWindowVM MainWindowVM;

        public MainWindow()
        {
            MainWindowVM = new MainWindowVM();
            InitializeComponent();

            DataContext = MainWindowVM;
        }

        public void ActualizaVista()
        {
            MainWindowVM.ActualizaVista();
            DataContext = null;
            DataContext = MainWindowVM;
        }

        private void AddSala_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddSala_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSala addSala = new AddUpdateSala("Añadir Sala");

            addSala.ResizeMode = ResizeMode.NoResize;
            addSala.ShowInTaskbar = false;

            if ((bool)addSala.ShowDialog())
            {
                Servicios.InsertaSala(new Salas(0, addSala.Disponible, addSala.Capacidad, addSala.Numero));
            }
            ActualizaVista();
        }

        private void ActualizarSala_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = salasDataGrid != null && salasDataGrid.SelectedItem != null;
        }

        private void ActualizarSala_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSala actualizarSala = new AddUpdateSala("Actualizar Sala")
            {
                Numero = MainWindowVM.SalaSeleccionada.Numero,
                Capacidad = MainWindowVM.SalaSeleccionada.Capacidad,
                Disponible = MainWindowVM.SalaSeleccionada.Disponible
            };
            actualizarSala.ResizeMode = ResizeMode.NoResize;
            actualizarSala.ShowInTaskbar = false;

            if ((bool)actualizarSala.ShowDialog())
            {
                MainWindowVM.SalaSeleccionada.Capacidad = actualizarSala.Capacidad;
                MainWindowVM.SalaSeleccionada.Numero = actualizarSala.Numero;
                MainWindowVM.SalaSeleccionada.Disponible = actualizarSala.Disponible;

                Servicios.ActualizaSala(MainWindowVM.SalaSeleccionada);
            }
            ActualizaVista();
        }

        private void AddSesion_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSesion addSesion = new AddUpdateSesion("Añadir sesión")
            {
                Peliculas = MainWindowVM.Peliculas,
                Salas = MainWindowVM.Salas
            };

            addSesion.Peliculas = MainWindowVM.Peliculas;
            addSesion.Salas = MainWindowVM.Salas;
            if ((bool)addSesion.ShowDialog())
            {
                int nuevoIdSesion = MainWindowVM.Sesiones[MainWindowVM.Sesiones.Count - 1].Id + 1;
                Sesion sesionNueva = new Sesion(nuevoIdSesion, addSesion.Pelicula, addSesion.Sala, addSesion.Hora);
                Servicios.InsertaSesion(sesionNueva);
            }
            ActualizaVista();
        }

        private void AddSesion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ActualizarSesion_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSesion updateSesion = new AddUpdateSesion("Actualizar sesión")
            {
                Peliculas = MainWindowVM.Peliculas,
                Salas = MainWindowVM.Salas,
                Sala = MainWindowVM.Salas[MainWindowVM.SesionSeleccionada.Sala.Id-1],
                Pelicula = MainWindowVM.Peliculas[MainWindowVM.SesionSeleccionada.Pelicula.Id-1],
                Hora = MainWindowVM.SesionSeleccionada.Hora

            };



            if ((bool)updateSesion.ShowDialog())
            {
                MainWindowVM.SesionSeleccionada.Pelicula = updateSesion.Pelicula;
                MainWindowVM.SesionSeleccionada.Sala = updateSesion.Sala;
                MainWindowVM.SesionSeleccionada.Hora = updateSesion.Hora;
                Servicios.ActualizaSesion(MainWindowVM.SesionSeleccionada);
                ActualizaVista();
            }
        }

        private void ActualizarSesion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = sesionesDataGrid != null && sesionesDataGrid.SelectedItem != null;
        }

        private void EliminarSesion_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Servicios.EliminarSesion(MainWindowVM.SesionSeleccionada);
            ActualizaVista();
        }

        private void EliminarSesion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = sesionesDataGrid != null && sesionesDataGrid.SelectedItem != null;
        }
    }
}
