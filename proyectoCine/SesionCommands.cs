using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace proyectoCine
{
    public static class SesionCommands
    {
        public static readonly RoutedUICommand AddSesion = new RoutedUICommand
        (
            "AddSesion",
            "AddSesion",
            typeof(SesionCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.A,ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ActualizarSesion= new RoutedUICommand
        (
            "ActualizarSesion",
            "ActualizarSesion",
            typeof(SesionCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U,ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand EliminarSesion = new RoutedUICommand
        (
            "EliminarSesion",
            "EliminarSesion",
            typeof(SesionCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D,ModifierKeys.Control)
            }
        );

    }
}
