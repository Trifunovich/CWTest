using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using CWTest.Core.DataManipulation;
using DataServiceProvider.TestBench.Dtos;
using DataServiceProvider.TestBench.Services;

namespace ExcelDataTransfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IComponentsService _service;

        public MainWindow()
        {
            InitializeComponent();
            _service = App.Container.Resolve<IComponentsService>();
        }

        private string FilePath { get; set; }

        private IList ExcelData { get; set; }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        { 
            var a = await _service.GetPage(new PagingParameters(1,110));
            ExcelData = new ObservableCollection<ComponentsDto>(a);
            DataGrid.ItemsSource = ExcelData;
        }
    }
}
