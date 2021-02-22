using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    public class Pelicula : INotifyPropertyChanged
    {
        private int id;
        private string titulo;
        private string cartel;
        private int año;
        private string genero;
        private string calificacion;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string Titulo
        {
            get { return titulo; }
            set
            {
                titulo = value;
                NotifyPropertyChanged("Titulo");
            }
        }
        public string Cartel
        {
            get { return cartel; }
            set
            {
                cartel = value;
                NotifyPropertyChanged("Cartel");
            }
        }
        public int Año
        {
            get { return año; }
            set
            {
                año = value;
                NotifyPropertyChanged("Año");
            }
        }
        public string Genero
        {
            get { return genero; }
            set
            {
                genero = value;
                NotifyPropertyChanged("Genero");
            }
        }
        public string Calificacion
        {
            get { return calificacion; }
            set
            {
                calificacion = value;
                NotifyPropertyChanged("Calificacion");
            }
        }

        public Pelicula(int id,
                        string titulo,
                        string cartel,
                        int año,
                        string genero,
                        string calificacion)
        {
            Id = id;
            Titulo = titulo;
            Cartel = cartel;
            Año = año;
            Genero = genero;
            Calificacion = calificacion;
        }

        public Pelicula(){}

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
