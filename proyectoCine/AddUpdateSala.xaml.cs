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
using System.Windows.Shapes;

namespace proyectoCine
{
    public partial class AddUpdateSala : Window
    {
        //Utilizo la misma ventana para añadir y para actualizar porque la ventana hace la misma función en los dos casos
        public string Numero { get; set; }
        public int Capacidad { get; set; }
        public bool Disponible { get; set; }
        public AddUpdateSala(string tituloVenta)
        {
            InitializeComponent();
            Title = tituloVenta;
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Numero = numeroTextBox.Text;
            Capacidad = int.Parse(capacidadTextBox.Text);
            Disponible = (bool)disponibleCheckBox.IsChecked;
            DialogResult = true;
        }
    }
}
