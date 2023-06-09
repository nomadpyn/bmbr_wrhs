# Склад крашенных деталей

 Имеется база данных с бамперами в цвет, на некоторые типы автомобилей. Приложение позволяет получать к ней доступ, и получать информацию о наименование, количестве и последней закупочной цене детали.

 База данных SQLite, строк подключения к ней находится в bmbr_wrhs в файле App.config (необходимо создать и добавить там строку подключения).

 Программа состоит из трех проектов:

 [bmbr_wrhs](https://github.com/nomadpyn/bmbr_wrhs/tree/master/bmbr_wrhs)

 Основная часть программы, содержит в себе модели данных (Models.cs) для подключения к базе через Entity Framework, а также методы работы с ними (GetClass.cs). В нем содержаться методы, которые используют для работы два других проекта. В файле CsvLoader.cs - методы для загрузки номенклатуры в базу данных из файла csv, который должен иметь следующую строку (тип детали, название ТС, цвет ТС, количество, цена).

 [bmbr_wrhs_wndw](https://github.com/nomadpyn/bmbr_wrhs/tree/master/bmbr_wrhs_wndw)

 Клиентское приложение для Windows на WPF, которое позволяет:

  * показать всю номенклатуру;
  * показать только наличие на складе;
  * найти деталь по параметрам;
  * списать деталь со склада (одну за раз);
  * провести приход с выводом сообщения о результатах прихода.

 Имеет в своем составе три окна (Основное окно, окно поиска, окно списания).

 [bmbr_wrhs_bot](https://github.com/nomadpyn/bmbr_wrhs/tree/master/bmbr_wrhs_bot)

 Клиентское приложения для выдачи информации о наличии и себестоимости конкретной детали для Telegram, с использование API TelegramBot. Позволяет разрешенным пользователям (по id) по ступенчатому поиску получить информацию о наличии и цене интересующей их детали. Поиск происходит следующим образом, по выпадающим кнопка на клавиатуре внутри Telegram:

   Бренд ТС => Марка ТС => Цвет ТС => Тип детали => Результат запроса.

 Также в каждом проекте есть свой файл App.config, который хранит строку подключения к БД, а в случае с TelegramBot он еще и хранит токен бота, для доступа к нему через API.  