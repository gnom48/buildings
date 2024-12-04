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

namespace BuildingSource.Auth
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new MasterContext())
            {
                try
                {
                    var account = new User
                    {
                        Address = AddressTextBox.Text,
                        Login = LoginTextBox.Text,
                        Password = PasswordTextBox.Text,
                        Passport = PasswordTextBox.Text,
                        Fname = FIOTextBox.Text,
                        Surname = "",
                        Patronumic = "",
                        Email = EmailTextBox.Text,
                        Phone = PhoneTextBox.Text
                    };
                    
                    ctx.Users.Add(account);
                    ctx.SaveChanges();

                    MessageBox.Show("Успех");

                    this.Close();
                    (new SignIn()).ShowDialog();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }
    }
}
