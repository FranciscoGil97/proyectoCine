using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class AddUpdateSesion : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Pelicula> peliculas;
        private ObservableCollection<Salas> salas;
        private Salas sala;
        private Pelicula pelicula;

        public string Hora { get; set; }
        public Pelicula Pelicula
        {
            get => pelicula; set
            {
                pelicula = value;
                NotifyPropertyChanged("Pelicula");
            }
        }
        public Salas Sala
        {
            get => sala; set
            {
                sala = value;
                NotifyPropertyChanged("Sala");
            }
        }


        public ObservableCollection<Pelicula> Peliculas
        {
            get => peliculas; set
            {
                peliculas = value;
                NotifyPropertyChanged("Peliculas");
            }
        }
        public ObservableCollection<Salas> Salas
        {
            get => salas; set
            {
                salas = value;
                NotifyPropertyChanged("Salas");
            }
        }
        public string Titulo { get; set; }
        public AddUpdateSesion(string tituloVenta)
        {
            InitializeComponent();
            Titulo = tituloVenta;
            Hora = "";

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Regex patronHora = new Regex(@"^(([1]{1}[2-9]{1})|([2]{1}[0-3]{1})|(00)):[0-5]{1}[0-9]{1}$");
            if (!patronHora.IsMatch(Hora))
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
            if (patronHora.IsMatch(Hora) && Pelicula != null && Sala != null)
                DialogResult = true;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
