using bmbr_wrhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace bmbr_wrhs_wndw
{
    // окно просмотра списанных деталей по дате или всех
    public partial class SoldWindow : Window
    {
        public SoldWindow()
        {
            InitializeComponent();
        }

        // обработка нажатия кнопки "Найти", в соответствии с заполненим дат ищет списанные детали в БД
        private void searchDateButton_Click(object sender, RoutedEventArgs e)
        {
            sold_data_grid.ItemsSource = null;
            sold_data_grid.ItemsSource = (startDatePicker.SelectedDate == null && endDatePicker.SelectedDate == null) ? getAllSoldParts() : getSoldPartsByDate();
            if (sold_data_grid.Items.Count == 0)
            {
                MessageBox.Show("Нет проданых деталей за это период");
            }
        }

        // метод поиска в БД деталей в диапазоне дат
        private List<SoldPart> getSoldPartsByDate()
        {
            // если дата не выбрана, то ставится по умолчанию текущая дата

            DateTime startDate = startDatePicker.SelectedDate == null ? DateTime.Now.Date : startDatePicker.SelectedDate.Value;
            DateTime endDate = endDatePicker.SelectedDate == null ? DateTime.Now.Date : endDatePicker.SelectedDate.Value;
            return GetClass.getSoldPartsByDate(startDate, endDate);
        }


        // метод поиска в БД всех списанных деталей
        private List<SoldPart> getAllSoldParts()
        {
            return GetClass.getAllSoldParts();
        }
    }
}
