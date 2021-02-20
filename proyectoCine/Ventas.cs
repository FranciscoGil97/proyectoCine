using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoCine
{
    class Ventas : INotifyPropertyChanged
    {
        private int id;
        private Sesion sesion;
        private int cantidad;
        private string pago;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
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
        public int Cantidad
        {
            get { return cantidad; }
            set
            {
                cantidad = value;
                NotifyPropertyChanged("Cantidad");
            }
        }
        public string Pago
        {
            get { return pago; }
            set { pago = value;
                NotifyPropertyChanged("Pago");
            }
        }

        public Ventas(int id,
                      Sesion sesion,
                      int cantidad,
                      string pago)
        {
            Id = id;
            Sesion = sesion;
            Cantidad = cantidad;
            Pago = pago;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
