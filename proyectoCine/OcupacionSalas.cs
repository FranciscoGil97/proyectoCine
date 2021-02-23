using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    class OcupacionSalas : INotifyPropertyChanged
    {
        private Sesion sesion;
        private int disponibles;
        private int vendidas;

        public int Vendidas
        {
            get { return vendidas; }
            set { vendidas = value;
                NotifyPropertyChanged("Vendidas");
            }
        }

        public int Disponibles
        {
            get { return disponibles; }
            set
            {
                disponibles = value;
                NotifyPropertyChanged("Disponibles");
            }
        }

        public Sesion Sesion
        {
            get { return sesion; }
            set
            {
                sesion = value;
                NotifyPropertyChanged("Sesion");
            }
        }

        public OcupacionSalas(int vendidas, int disponibles, Sesion sesion)
        {
            Vendidas = vendidas;
            Disponibles = disponibles;
            Sesion = sesion;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
