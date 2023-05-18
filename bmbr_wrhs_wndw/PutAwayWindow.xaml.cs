using bmbr_wrhs;
using System.Windows;
using System.Windows.Controls;

namespace bmbr_wrhs_wndw
{
    // Окно списания детали
    public partial class PutAwayWindow : Window
    {
        // поле для хранения id детали
        private int AutoPartId;

        // Конструктор по умолчанию

        public PutAwayWindow()
        {
            InitializeComponent();
            var data = GetClass.getAllCars();
            autoBox.ItemsSource = data;
        }

        // обработка изменения состояния autoBox в окне

        private void autoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorBox.ItemsSource = null;
            typeBox.ItemsSource = null;
            var data = GetClass.getCarColor((int)autoBox.SelectedValue);
            colorBox.ItemsSource = data;
        }

        // обработка изменения состояния colorBox в окне

        private void colorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            typeBox.ItemsSource = null;

            // если цвет ТС выбран, то заполняются типы детали

            if (colorBox.ItemsSource != null)
            {
                var data = GetClass.getAutoPartsbySearch((int)autoBox.SelectedValue, (int)colorBox.SelectedValue);
                typeBox.ItemsSource = data;
                typeBox.DisplayMemberPath = "PartType";
            }
        }

        // обработка изменения состояние typeBox в окне

        private void typeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // если выбрано ТС, цвет и тип детали, то выводится деталь по поиску

            if (autoBox.ItemsSource != null && colorBox.ItemsSource != null && typeBox.ItemsSource != null)
            {                
                var data = GetClass.getAutoPartsbySearch((int)autoBox.SelectedValue, (int)colorBox.SelectedValue);
                this.partCount.Text = "Количество на складе: " + data[0].Count;

                // если количество деталей больше 0, то возможно ее списание

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

        // Обработка нажатия кнопки "Убрать", с предупреждением в MessageBox

        private void putButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите убрать деталь со склада. Вы уверены?","Предупреждение",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                GetClass.putAwayPart(this.AutoPartId);
            }
            this.Close();
        }
    }
}