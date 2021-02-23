using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    public class Sesion : INotifyPropertyChanged
    {
        private int id;
        private string hora;
        private Pelicula pelicula;
        private Salas sala;

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

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
            }
        }

        public string Hora
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
                      string hora)
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
