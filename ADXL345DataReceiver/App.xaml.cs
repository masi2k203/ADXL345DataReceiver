using ADXL345DataReceiver.ViewModels;
using ADXL345DataReceiver.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ADXL345DataReceiver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var w = new MainDataView();
            var vm = new MainDataViewModel();
            w.DataContext = vm;
            w.Show();
        }
    }
}
