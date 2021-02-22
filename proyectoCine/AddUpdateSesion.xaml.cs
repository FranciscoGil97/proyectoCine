using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Lógica de interacción para AddUpdateSesion.xaml
    /// </summary>
    public partial class AddUpdateSesion : Window
    {

        public string Hora { get; set; }

        private ObservableCollection<Pelicula> peliculas;
        private ObservableCollection<Salas> salas;

        public Pelicula Pelicula { get; set; }

        public Salas Sala { get; set; }

        public ObservableCollection<Pelicula> Peliculas
        {
            get { return peliculas; }
            set
            {
                peliculas = value;
                peliculaComboBox.ItemsSource = peliculas;
            }
        }

        public ObservableCollection<Salas> Salas
        {
            get { return salas; }
            set
            {
                salas = value;
                salaComboBox.ItemsSource = salas;
            }
        }

        public AddUpdateSesion(string tituloVenta)
        {
            Hora = "";
            InitializeComponent();
            Title = tituloVenta;

            DataContext = this;

            peliculaComboBox.ItemsSource = peliculas;
            peliculaComboBox.DisplayMemberPath = "Titulo";

            salaComboBox.ItemsSource = salas;
            salaComboBox.DisplayMemberPath = "Numero";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string patronHora = @"^(([1]{1}[2-9]{1})|([2]{1}[0-3]{1})|(00)):[0-5]{1}[0-9]{1}$";
            if (!Regex.IsMatch(Hora, patronHora))
            {
                string mensaje = "Hora introducida incorrecta.\nEl formato de la hora debe ser (HH:MM) y las horas posibles son 12:00-00:00";
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }

            if (Pelicula == null)
                MessageBox.Show("Para insertar una sesión debes seleccionar una película", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            

            if (Sala == null)
                MessageBox.Show("Para insertar una sesión debes seleccionar una sala", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);


            //No cierro la ventana cuando se le da al botón de aceptar 
            //si el estado de los campos no es correcto
            if (Regex.IsMatch(Hora, patronHora) && Pelicula != null && Sala != null)
                DialogResult = true;
        }
    }
}
