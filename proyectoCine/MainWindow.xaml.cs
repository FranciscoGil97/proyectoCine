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

        private void SalasDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void AñadirSesion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ActualizarSesion_Click(object sender, RoutedEventArgs e)
        {
            actualizarSesion.IsEnabled = salasDataGrid.SelectedItem != null;
        }
    }
}
