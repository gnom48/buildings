using BuildingSource.Models;
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

namespace BuildingSource
{
    /// <summary>
    /// Логика взаимодействия для RepairCalc.xaml
    /// </summary>
    public partial class RepairCalc : Window
    {
        private string currentReapirType;
        private string currentObjectType;
        private decimal currentS;
        private decimal currentWithDesigner = 0.0m;
        private decimal currentWithIngineer = 0.0m;
        private decimal currentFullPrice = 0.0m;
        public Dictionary<string, List<DataAboutServices>> DataDict { get; set; } = new Dictionary<string, List<DataAboutServices>>();

        public RepairCalc(Dictionary<string, List<DataAboutServices>> data)
        {
            InitializeComponent();
            DataDict = data;
        }

        private void HouseRadiobutton_Checked(object sender, RoutedEventArgs e)
        {
            switch(((RadioButton)sender).Content.ToString())
            {
                case "Дом":
                    {
                        currentObjectType = ((RadioButton)sender).Content.ToString()!;
                        RefreshFullPrice();
                        return;
                    }
                case "Квартира":
                    {
                        currentObjectType = ((RadioButton)sender).Content.ToString()!;
                        RefreshFullPrice();
                        return;
                    }
                case "Комната":
                    {
                        currentObjectType = ((RadioButton)sender).Content.ToString()!;
                        RefreshFullPrice();
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void RepairTypeCompoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(((ComboBoxItem)(RepairTypeCompoBox.SelectedItem)).Content.ToString())
            {
                case "Косметический":
                    {
                        currentReapirType = ((ComboBoxItem)(RepairTypeCompoBox.SelectedItem)).Content.ToString()!;
                        RefreshFullPrice();
                        return;
                    }
                case "Капитальный":
                    {
                        currentReapirType = ((ComboBoxItem)(RepairTypeCompoBox.SelectedItem)).Content.ToString()!;
                        RefreshFullPrice();
                        return;
                    }
                case "Дизайнерский":
                    {
                        currentReapirType = ((ComboBoxItem)(RepairTypeCompoBox.SelectedItem)).Content.ToString()!;
                        RefreshFullPrice();
                        return;
                    }
                default:
                    {
                        return;
                    }
            }

        }

        private void STextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentS = Decimal.Parse(((TextBox)sender).Text);
            RefreshFullPrice();
        }

        private void SSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentS = (decimal)((Slider)sender).Value;
            STextBox.Text = currentS.ToString();
            RefreshFullPrice();
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var address = ((TextBox)sender).Text;

        }

        private void RefreshFullPrice()
        {
            try
            {
                var currentReapir = DataDict[currentReapirType];

                currentFullPrice = currentReapir[0].M2price * currentS;

                if (WithDesigner.IsChecked == true)
                {
                    currentWithDesigner = currentReapir.First(x => x.ServiceName == "Выезд дизайнера").ServicePrice;
                }
                else
                {
                    currentWithDesigner = 0.0m;
                }

                if (WithIngeneer.IsChecked == true)
                {
                    currentWithIngineer = currentReapir.First(x => x.ServiceName == "Выезд бригадира/инженера").ServicePrice;
                }
                else
                {
                    currentWithIngineer = 0.0m;
                }

                WithIngeneerPrice.Text = currentWithIngineer.ToString();
                WithDesignerPrice.Text = currentWithDesigner.ToString();

                currentFullPrice += currentWithDesigner!;
                currentFullPrice += currentWithIngineer!;

                FullPriceTextBox.Text = currentFullPrice.ToString();
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void WithIngeneer_Checked(object sender, RoutedEventArgs e) => RefreshFullPrice();

        private void WithIngeneer_Unchecked(object sender, RoutedEventArgs e) => RefreshFullPrice();

        private void WithDesigner_Checked(object sender, RoutedEventArgs e) => RefreshFullPrice();

        private void WithDesigner_Unchecked(object sender, RoutedEventArgs e) => RefreshFullPrice();

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            (new Comparison()).Show();
        }

        private void AddToComparisonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(currentS >= 0 && currentObjectType != "" && currentReapirType != "" && AddressTextBox.Text != "" && currentFullPrice != 0))
            {
                MessageBox.Show("Заполните все поля!");

                return;
            }

            using(var ctx = new MasterContext())
            {
                var repairType = ctx.RepairTypes.First(x => x.Name == currentReapirType);
                var repairObject = ctx.ObjectTypes.First(x => x.Name == currentObjectType);

                Comparison.Favorites.Add(new DataToComparison
                {
                    RepairType = repairType,
                    RepairObject = repairObject,
                    Square = currentS,
                    Designer = ctx.Services.First(x => x.RepairType == repairType.Id && x.ServiceId == 1),
                    Engeneer = ctx.Services.First(x => x.RepairType == repairType.Id && x.ServiceId == 2),
                    FullPrice = currentFullPrice,
                    Address = AddressTextBox.Text,
                });

                MessageBox.Show("Добавлено");
            }
        }
    }
}
