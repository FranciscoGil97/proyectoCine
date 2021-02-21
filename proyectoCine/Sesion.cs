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
        private int idPelicula;
        private int idSala;
        private string hora;
        private string tituloPelicula;

        private string nombreSala;

        public string NombreSala
        {
            get { return nombreSala; }
            set
            {
                nombreSala = value;
                NotifyPropertyChanged("NombreSala");
            }
        }


        public string TituloPelicula
        {
            get { return tituloPelicula; }
            set
            {
                tituloPelicula = value;
                NotifyPropertyChanged("TituloPelicula");
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


        public int IdPelicula
        {
            get { return idPelicula; }
            set
            {
                idPelicula = value;
                NotifyPropertyChanged("IdPelicula");
            }
        }


        public int IdSala
        {
            get { return idSala; }
            set
            {
                idSala = value;
                NotifyPropertyChanged("IdSala");
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
                      int idPelicula,
                      int idSala,
                      string hora)
        {
            Id = id;
            IdPelicula = idPelicula;
            IdSala = idSala;
            Hora = hora;
        }

        public Sesion(int id,
                      int idPelicula,
                      int idSala,
                      string hora,
                      string nombreSala,
                      string tituloPelicula)
        {
            Id = id;
            IdPelicula = idPelicula;
            IdSala = idSala;
            Hora = hora;
            NombreSala = nombreSala;
            TituloPelicula = tituloPelicula;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
