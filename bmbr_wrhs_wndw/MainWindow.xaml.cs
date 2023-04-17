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
        public int searchCarId { get; set; }
        public int searchColorId { get;set; }
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

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            this.searchCarId = 0;
            this.searchColorId = 0;
            this.bumber_data_grid.ItemsSource = null;
            SearchWindow nw = new();
            nw.Owner = this;
            nw.ShowDialog();

            if(this.searchCarId>0 && this.searchColorId > 0)
            {
                var data = GetClass.getAutoPartsbySearch(this.searchCarId, this.searchColorId);
                this.bumber_data_grid.ItemsSource = data;
            }
        }

        private void sell_button_Click(object sender, RoutedEventArgs e)
        {
            PutAwayWindow nw = new();
            nw.Owner = this;
            nw.ShowDialog();
        }
    }
}
