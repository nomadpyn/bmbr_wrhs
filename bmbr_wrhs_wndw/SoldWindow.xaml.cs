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

        private void searchDateButton_Click(object sender, RoutedEventArgs e)
        {
            sold_data_grid.ItemsSource = null;
            sold_data_grid.ItemsSource = (startDatePicker.SelectedDate == null && endDatePicker.SelectedDate == null) ? getAllSoldParts() : getSoldPartsByDate();
            if (sold_data_grid.Items.Count == 0)
            {
                MessageBox.Show("Нет проданых деталей за это период");
            }
        }

        private List<SoldPart> getSoldPartsByDate()
        {
            DateTime startDate = startDatePicker.SelectedDate == null ? DateTime.Now.Date : startDatePicker.SelectedDate.Value;
            DateTime endDate = endDatePicker.SelectedDate == null ? DateTime.Now.Date : endDatePicker.SelectedDate.Value;
            return GetClass.getSoldPartsByDate(startDate, endDate);
        }
        
        private List<SoldPart> getAllSoldParts()
        {
            return GetClass.getAllSoldParts();
        }
    }
}
