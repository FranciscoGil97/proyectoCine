using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace proyectoCine
{
    public static class SalasCommands
    {
        public static readonly RoutedUICommand AddSala = new RoutedUICommand
        (
            "AddSala",
            "AddSala",
            typeof(SalasCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.A,ModifierKeys.Control)
            }
        );
        public static readonly RoutedUICommand ActualizarSala = new RoutedUICommand
        (
            "ActualizarSala",
            "ActualizarSala",
            typeof(SalasCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U,ModifierKeys.Control)
            }
        );
    }
}
