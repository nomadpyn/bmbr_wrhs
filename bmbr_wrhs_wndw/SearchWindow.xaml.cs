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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using bmbr_wrhs;

namespace bmbr_wrhs_wndw
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            var data = GetClass.getAllCars();
            autoBox.ItemsSource = data;
        }

        private void autoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorBox.ItemsSource = null;
            var data = GetClass.getCarColor((int)autoBox.SelectedValue); 
            colorBox.ItemsSource = data;
            
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int carId = (int)autoBox.SelectedValue;
                int colorId = (int)colorBox.SelectedValue;
                if (carId > 0 && colorId > 0)
                {
                    MainWindow main = this.Owner as MainWindow;
                    main.searchCarId = carId;
                    main.searchColorId = colorId;
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Вы не выбрали");
            }
            finally
            {
                this.Close();
            }
        }
    }
}
