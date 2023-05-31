using bmbr_wrhs;
using Microsoft.Win32;
using System;
using System.Windows;

namespace bmbr_wrhs_wndw
{
    // Начальное окно программы 

    public partial class MainWindow : Window
    {
        // поля для получения id ТС и цвета из окна поиска
        public int searchCarId { get; set; }
        public int searchColorId { get;set; }

        // Конструктор по умолчанию

        public MainWindow()
        {
            InitializeComponent();
            this.addDateNow();
        }

        // Обработка нажатия кнопки "Загрузить все", для загрузки всей номенклатуры из БД

        private void load_all_button_Click(object sender, RoutedEventArgs e)
        {
            this.bumber_data_grid.ItemsSource = null;
            this.bumber_data_grid.ItemsSource = GetClass.getAllAutopartsFromDB();
            this.fillStatusBar();
        }

        // Обработка нажатия кнопки "Загрузить наличие", для загрузки всех деталей в наличии

        private void load_not_null_button_Click(object sender, RoutedEventArgs e)
        {
            this.bumber_data_grid.ItemsSource = null;
            this.bumber_data_grid.ItemsSource = GetClass.getPartsNotNull();
            this.fillStatusBar();
        }

        // Обработка нажатия кнопки "Найти деталь", вызов окна SearchWindow

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            this.searchCarId = 0;
            this.searchColorId = 0;
            this.bumber_data_grid.ItemsSource = null;

            // Вызов нового окна поиска

            SearchWindow nw = new();
            nw.Owner = this;
            nw.ShowDialog();

            // нахождение детали по двум id, если все успешно

            if(this.searchCarId>0 && this.searchColorId > 0)
            {
                var data = GetClass.getAutoPartsbySearch(this.searchCarId, this.searchColorId);
                this.bumber_data_grid.ItemsSource = data;
            }
            this.fillStatusBar();
        }

        // Обработка нажатия кнопик "Списать деталь", вызов окна PutAwayWindow

        private void sell_button_Click(object sender, RoutedEventArgs e)
        {

            //Вызов окна списания

            PutAwayWindow nw = new();
            nw.Owner = this;
            nw.ShowDialog();
        }

        // Обработка нажатия кнопки "Провести приход", с помощью CsvLoader

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            // Создаем OpenFileDialog по поиску csv

            OpenFileDialog ofd = new();
            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "csv files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;

            // если все успешно, то загружаем данные в БД

            if(ofd.ShowDialog() == true)
            {
                string FilePath = ofd.FileName;

                var result = CsvLoader.getPartsFromCSV(FilePath);
                
                // если загрузка прошла успешно, выводим MessageBox с информацией о загруженных позициях, в противном случае код ошибки

                if (result[0] == "ok")
                {
                    string message = result[2] + "\n" + result[3] + "\n" + result[4] + "\n" + result[5] + "\n" + result[6];
                    MessageBox.Show(message, result[1]);
                }
                else
                {
                    MessageBox.Show(result[1], "Ошибка");
                }
            }
        }

        // Добавление текущей даты к имени главного окна при каждом запуске

        private void addDateNow()
        {
            DateTime date = DateTime.Now;
            this.Title += $" - Сегодня {date.Day}.{date.Month}.{date.Year}"; 
        }

        // добавление информации о загруженных в datagrid позициях в statusbar

        private void fillStatusBar()
        {
            int allCount = 0;
            foreach(AutoPart part in this.bumber_data_grid.Items){
                allCount += part.Count;
            }
            this.statusBarLoadData.Text = $" Загружено номенклатуры {this.bumber_data_grid.Items.Count}. Всего позиций на складе {allCount}";
            
        }

        private void check_sold_button_Click(object sender, RoutedEventArgs e)
        {
            SoldWindow nw = new();
            nw.Owner = this;
            nw.ShowDialog();
        }
    }
}