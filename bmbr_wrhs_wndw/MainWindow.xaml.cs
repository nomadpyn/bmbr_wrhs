using bmbr_wrhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bmbr_wrhs_wndw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void load_all_button_Click(object sender, RoutedEventArgs e)
        {
            this.bumber_data_grid.ItemsSource = null;
            this.bumber_data_grid.ItemsSource = GetClass.getAllAutopartsFromDB();
        }

        private void load_not_null_button_Click(object sender, RoutedEventArgs e)
        {
            this.bumber_data_grid.ItemsSource = null;
            this.bumber_data_grid.ItemsSource = GetClass.getPartsNotNull();
        }
    }
}
