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
        MainWindowVM mainWindowVM;

        public MainWindow()
        {
            mainWindowVM = new MainWindowVM();
            InitializeComponent();

            DataContext = mainWindowVM;

        }

        private void ActualizarSesion_Click(object sender, RoutedEventArgs e)
        {
            actualizarSesion.IsEnabled = salasDataGrid.SelectedItem != null;
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
            mainWindowVM.ActualizaVista();
            DataContext = null;
            DataContext = mainWindowVM;
        }

        private void ActualizarSala_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = salasDataGrid != null && salasDataGrid.SelectedItem != null;
        }

        private void ActualizarSala_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSala actualizarSala = new AddUpdateSala("Actualizar Sala")
            {
                Numero = mainWindowVM.SalaSeleccionada.Numero,
                Capacidad = mainWindowVM.SalaSeleccionada.Capacidad,
                Disponible = mainWindowVM.SalaSeleccionada.Disponible
            };
            actualizarSala.ResizeMode = ResizeMode.NoResize;
            actualizarSala.ShowInTaskbar = false;

            if ((bool)actualizarSala.ShowDialog())
            {
                mainWindowVM.SalaSeleccionada.Capacidad = actualizarSala.Capacidad;
                mainWindowVM.SalaSeleccionada.Numero = actualizarSala.Numero;
                mainWindowVM.SalaSeleccionada.Disponible = actualizarSala.Disponible;

                Servicios.ActualizaSala(mainWindowVM.SalaSeleccionada);
            }
            mainWindowVM.ActualizaVista();
            DataContext = null;
            DataContext = mainWindowVM;


        }

        private void AddSesion_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSesion addUpdateSesion = new AddUpdateSesion("Añadir sesión");

            addUpdateSesion.Peliculas = mainWindowVM.Peliculas;
            addUpdateSesion.Salas = mainWindowVM.Salas;
            if ((bool)addUpdateSesion.ShowDialog())
            {
                int nuevoIdSesion = mainWindowVM.Sesiones[mainWindowVM.Sesiones.Count - 1].Id + 1;
                Sesion sesionNueva = new Sesion(nuevoIdSesion, addUpdateSesion.Pelicula.Id,addUpdateSesion.Sala.Id,addUpdateSesion.Hora);
                Servicios.InsertaSesion(sesionNueva);
            }
            mainWindowVM.ActualizaVista();
            DataContext = null;
            DataContext = mainWindowVM;
        }

        private void AddSesion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ActualizarSesion_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddUpdateSesion addUpdateSesion = new AddUpdateSesion("Actualizar sesión");

            addUpdateSesion.Peliculas = mainWindowVM.Peliculas;
            addUpdateSesion.Salas = mainWindowVM.Salas;
            addUpdateSesion.Sala = mainWindowVM.Salas[mainWindowVM.SesionSeleccionada.IdSala-1];
            addUpdateSesion.Pelicula = mainWindowVM.Peliculas[mainWindowVM.SesionSeleccionada.IdPelicula-1];
            addUpdateSesion.Hora = mainWindowVM.SesionSeleccionada.Hora;
            if ((bool)addUpdateSesion.ShowDialog())
            {

            }
        }

        private void ActualizarSesion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = sesionesDataGrid != null && sesionesDataGrid.SelectedItem != null;
        }

        private void EliminarSesion_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void EliminarSesion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = sesionesDataGrid != null && sesionesDataGrid.SelectedItem != null;
        }
    }
}
