using BuildingSource.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BuildingSource
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, List<DataAboutServices>> DataDict { get; set; } = new Dictionary<string, List<DataAboutServices>>();

        public MainWindow()
        {
            InitializeComponent();
            var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

            using(var ctx = MasterContext.GetInstance())
            {
                var query = from rt in ctx.RepairTypes
                            join s in ctx.Services on rt.Id equals s.RepairType
                            join st in ctx.ServiceTypes on s.ServiceId equals st.Id
                            orderby rt.Name, st.Name
                            select new DataAboutServices
                            {
                                RepairName = rt.Name,
                                M2price = (decimal)rt.M2price,
                                ServicePrice = (decimal)s.Price,
                                ServiceName = st.Name
                            };

                var dopServices = query.ToList();

                var dict = new Dictionary<string, List<DataAboutServices>> { };
                foreach(var x in dopServices)
                {
                    if (!dict.ContainsKey(x.RepairName))
                    {
                        dict[x.RepairName] = new List<DataAboutServices>();
                    }
                    dict[x.RepairName].Add(x);
                }

                DataDict = dict;

                foreach (var key in dict.Keys)
                {
                    var sp = new StackPanel() { Orientation = Orientation.Vertical };

                    sp.Children.Add(new TextBlock { Text = key });

                    var list = new StackPanel() { Orientation = Orientation.Vertical };
                    list.Children.Add(new TextBlock { Text = "Работы", FontWeight = FontWeights.Bold });
                    foreach (var item in dict[key])
                    {
                        list.Children.Add(new TextBlock { Text = item.ServiceName });
                    }
                    list.Background = new SolidColorBrush(Colors.LightGray);

                    sp.Children.Add(list);
                    var row = new StackPanel() { Orientation = Orientation.Vertical };
                    row.Children.Add(new TextBlock { Text = "Стоимость" });
                    row.Children.Add(new TextBlock { Text = dict[key][0].M2price.ToString(), Background = new SolidColorBrush(Colors.LightGray) });
                    row.Children.Add(new TextBlock { Text = "Руб/м2" });
                    sp.Children.Add(row);
                    stackPanel.Children.Add(sp);
                }
            }

            Grid.SetColumn(stackPanel, 0);
            Grid.SetRow(stackPanel, 1);
            Root.Children.Add(stackPanel);
        }

        private void GoToCalc_Click_1(object sender, RoutedEventArgs e)
        {
            var calc = new RepairCalc(DataDict);
            calc.ShowDialog();
        }
    }
}
