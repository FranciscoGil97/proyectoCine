using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    class Sesion : INotifyPropertyChanged
    {
        private int id;
        private Pelicula pelicula;
        private Salas sala;
        private int hora;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
            }
        }


        public Pelicula Pelicula
        {
            get { return pelicula; }
            set
            {
                pelicula = value;
                NotifyPropertyChanged("Pelicula");
            }
        }


        public Salas Sala
        {
            get { return sala; }
            set
            {
                sala = value;
                NotifyPropertyChanged("Sala");
            }
        }


        public int Hora
        {
            get { return hora; }
            set
            {
                hora = value;
                NotifyPropertyChanged("Hora");
            }
        }

        public Sesion(int id,
                      Pelicula pelicula,
                      Salas sala,
                      int hora)
        {
            Id = id;
            Pelicula = pelicula;
            Sala = sala;
            Hora = hora;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
