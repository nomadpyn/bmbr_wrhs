using System;
using System.Windows;
using System.Windows.Controls;
using bmbr_wrhs;

namespace bmbr_wrhs_wndw
{
    // Окно поиска детали
    public partial class SearchWindow : Window
    {

        // Конструктор по умолчанию

        public SearchWindow()
        {
            InitializeComponent();
            var data = GetClass.getAllCars();
            autoBox.ItemsSource = data;
        }

        // Обработка изменения состония autoBox в окне

        private void autoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorBox.ItemsSource = null;
            var data = GetClass.getCarColor((int)autoBox.SelectedValue); 
            colorBox.ItemsSource = data;
            
        }

        // Обработка нажатия кнопки "Найти"

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int carId = (int)autoBox.SelectedValue;
                int colorId = (int)colorBox.SelectedValue;

                // если данные больше 0, тогда возвращаем их в основую форму и там происходит вывод информации в DataGrid

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