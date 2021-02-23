using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class VenderEntradas : Window
    {

        public ObservableCollection<Ventas> Ventas { get; set; }
        public ObservableCollection<Sesion> Sesiones { get; set; }
        public Sesion SesionSeleccionada
        {
            get => sesionSeleccionada; set
            {
                sesionSeleccionada = value;
                //todo esto es para que el formulario de vender entradas salga bien
                EntradasLibres = sesionSeleccionada.Sala.Capacidad;
                foreach (Ventas v in Ventas)
                    if (sesionSeleccionada.Id == v.Sesion.Id)
                        EntradasLibres -= v.Cantidad;
                               
                Venta = new Ventas(Ventas.Count + 1, sesionSeleccionada, 0, "");
            }
        }
        public Ventas Venta
        {
            get => venta; set
            {
                venta = value;
                DataContext = null;
                DataContext = this;
            }
        }

        public string[] FormaPago { get; private set; }
        private Ventas venta;
        private Sesion sesionSeleccionada;

        public int EntradasLibres { get; set; }
        public VenderEntradas()
        {
            FormaPago = new string[] { "efectivo", "tarjeta", "bizum" };
            InitializeComponent();
            DataContext = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numeroEntradasTextBox.Text, out int numeroEntradas))
            {
                //Me aseguro que el número de entradas que se venden es correcto
                Venta.Cantidad = numeroEntradas;
                if ((EntradasLibres - numeroEntradas) >= 0)
                    DialogResult = true;
                else
                    MessageBox.Show("No puedes vender más entradras que las disponibles.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                MessageBox.Show("Las entradas deben ser un número entero.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

        }


    }
}
