using BuildingSource.Auth;
using BuildingSource.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

namespace BuildingSource
{
    /// <summary>
    /// Логика взаимодействия для Comparison.xaml
    /// </summary>
    public partial class Comparison : Window
    {
        public static List<DataToComparison> Favorites = new List<DataToComparison>();
        public static DataToComparison? Order;

        public Comparison()
        {
            InitializeComponent();

            ShowCompareTableRows();
        }

        private void ShowCompareTableRows()
        {
            RepairTypeStackPanel.Children.Clear();
            RepairTypeStackPanel.Children.Add(new TextBlock { Text = "Название" });
            ObjectTypeStackPanel.Children.Clear();
            ObjectTypeStackPanel.Children.Add(new TextBlock { Text = "Объект" });
            SquareStackPanel.Children.Clear();
            SquareStackPanel.Children.Add(new TextBlock { Text = "Площадь" });
            DesignerStackPanel.Children.Clear();
            DesignerStackPanel.Children.Add(new TextBlock { Text = "Дизайнер" });
            EngeneerStackPanel.Children.Clear();
            EngeneerStackPanel.Children.Add(new TextBlock { Text = "Инженер" });
            FullPriceStackPanel.Children.Clear();
            FullPriceStackPanel.Children.Add(new TextBlock { Text = "Цена" });
            OrderStackPanel.Children.Clear();
            DeleteStackPanel.Children.Clear();

            for (int i = 0; i < Favorites.Count; i++)
            {
                var item = Favorites[i];
                RepairTypeStackPanel.Children.Add(new TextBlock { Text = item.RepairType.Name, Background = new SolidColorBrush(Colors.LightGray) });
                ObjectTypeStackPanel.Children.Add(new TextBlock { Text = item.RepairObject.Name, Background = new SolidColorBrush(Colors.LightGray) });
                SquareStackPanel.Children.Add(new TextBlock { Text = item.Square.ToString(), Background = new SolidColorBrush(Colors.LightGray) });
                DesignerStackPanel.Children.Add(new TextBlock { Text = (item.Designer != null ? item.Designer.Price : 0).ToString(), Background = new SolidColorBrush(Colors.LightGray) });
                EngeneerStackPanel.Children.Add(new TextBlock { Text = (item.Engeneer != null ? item.Engeneer.Price : 0).ToString(), Background = new SolidColorBrush(Colors.LightGray) });
                FullPriceStackPanel.Children.Add(new TextBlock { Text = item.FullPrice.ToString(), Background = new SolidColorBrush(Colors.LightGray) });
                var orderButton = new Button { Content = "Заказать", Tag = i };
                orderButton.Click += OnOrderRow;
                OrderStackPanel.Children.Add(orderButton);
                var delButton = new Button { Content = "X", Tag = i };
                delButton.Click += OnDeleteRow;
                DeleteStackPanel.Children.Add(delButton);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnDeleteRow(object sender, RoutedEventArgs e)
        {
            Favorites.RemoveAt((int)((Button)sender).Tag);
            ShowCompareTableRows();
        }

        private void OnOrderRow(object sender, RoutedEventArgs e)
        {
            Order = Favorites[(int)((Button)sender).Tag];

            (new SignIn()).ShowDialog();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(@"C:\Users\Egorc\Desktop\report.xlsx")))
                {
                    var sheet = package.Workbook.Worksheets.Add("Report");

                    sheet.Cells["A1:G1"].Merge = true;
                    sheet.Cells["A1:G1"].Value = "Сравнение ремонтов";

                    sheet.Cells["B3"].Value = "Название";
                    sheet.Cells["C3"].Value = "Объект";
                    sheet.Cells["D3"].Value = "Площадь";
                    sheet.Cells["E3"].Value = "Дизайнер";
                    sheet.Cells["F3"].Value = "Инженер";
                    sheet.Cells["G3"].Value = "Итог";

                    var tableShift = 4;

                    for (int i = 0; i < Favorites.Count; i++)
                    {
                        var item = Favorites[i];

                        sheet.Cells[$"A{i + tableShift}"].Value = i + 1;
                        sheet.Cells[$"B{i + tableShift}"].Value = item.RepairType.Name;
                        sheet.Cells[$"C{i + tableShift}"].Value = item.RepairObject.Name;
                        sheet.Cells[$"D{i + tableShift}"].Value = item.Square.ToString();
                        sheet.Cells[$"E{i + tableShift}"].Value = item.Designer.Price.ToString();
                        sheet.Cells[$"F{i + tableShift}"].Value = item.Engeneer.Price.ToString();
                        sheet.Cells[$"G{i + tableShift}"].Value = item.FullPrice.ToString();
                    }

                    package.Save();
                }

                MessageBox.Show("Отчет на рабочем столе");
            }
            catch (Exception er)
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
