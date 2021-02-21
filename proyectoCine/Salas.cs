using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    class Salas : INotifyPropertyChanged
    {
        private int id;
        private bool disponible;
        private int capacidad;
        private string numero;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public bool Disponible
        {
            get { return disponible; }
            set
            {
                disponible = value;
                NotifyPropertyChanged("Disponible");
            }
        }
        public int Capacidad
        {
            get { return capacidad; }
            set
            {
                capacidad = value;
                NotifyPropertyChanged("Capacidad");
            }
        }
        public string Numero
        {
            get { return numero; }
            set
            {
                numero = value;
                NotifyPropertyChanged("Numero");
            }
        }

        public Salas(int id,
                     bool disponible,
                     int capacidad,
                     string numero)
        {
            Id = id;
            Disponible = disponible;
            Capacidad = capacidad;
            Numero = numero;
        }

        public Salas() { }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
