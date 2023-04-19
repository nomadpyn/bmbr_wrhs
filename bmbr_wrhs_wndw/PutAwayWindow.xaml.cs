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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace bmbr_wrhs_wndw
{
    /// <summary>
    /// Interaction logic for PutAwayWindow.xaml
    /// </summary>
    public partial class PutAwayWindow : Window
    {
        private int AutoPartId;
        public PutAwayWindow()
        {
            InitializeComponent();
            var data = GetClass.getAllCars();
            autoBox.ItemsSource = data;
        }

        private void autoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorBox.ItemsSource = null;
            typeBox.ItemsSource = null;
            var data = GetClass.getCarColor((int)autoBox.SelectedValue);
            colorBox.ItemsSource = data;
        }

        private void colorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            typeBox.ItemsSource = null;
            if (colorBox.ItemsSource != null)
            {
                var data = GetClass.getAutoPartsbySearch((int)autoBox.SelectedValue, (int)colorBox.SelectedValue);
                typeBox.ItemsSource = data;
                typeBox.DisplayMemberPath = "PartType";
            }
        }

        private void typeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (autoBox.ItemsSource != null && colorBox.ItemsSource != null && typeBox.ItemsSource != null)
            {                
                var data = GetClass.getAutoPartsbySearch((int)autoBox.SelectedValue, (int)colorBox.SelectedValue);
                this.partCount.Text = "Количество на складе: " + data[0].Count;
                if (data[0].Count > 0)
                {
                    this.putButton.IsEnabled = true;
                    this.AutoPartId = data[0].Id;
                }
            }
            else
            {
                this.putButton.IsEnabled = false;
                this.partCount.Text = "Количество на складе:";
            }
        }

        private void putButton_Click(object sender, RoutedEventArgs e)
        {
            GetClass.putAwayPart(this.AutoPartId);
            this.Close();
        }
    }
}
