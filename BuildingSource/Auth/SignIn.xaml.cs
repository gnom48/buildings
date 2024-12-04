using BuildingSource.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.Linq;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;
using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BuildingSource.Auth
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public User? account { get; private set; }

        public SignIn()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            using(var ctx = new MasterContext())
            {
                try
                {
                    account = ctx.Users.FirstOrDefault(x => x.Login == LoginTextBox.Text && x.Password == PasswordTextBox.Text);

                    var res = FillConract(Comparison.Order);

                    if (res)
                    {
                        var request = new UsersRequest()
                        {
                            UserId = account.Id,
                            ObjectTypeId = Comparison.Order.RepairObject.Id,
                            RepairTypeId = Comparison.Order.RepairType.Id,
                            Square = (float?)Comparison.Order.Square
                        };
                        ctx.UsersRequests.Add(request);
                        ctx.SaveChanges();

                        if (Comparison.Order.Designer != null)
                        {
                            ctx.RequestOptions.Add(new RequestOption
                            {
                                Datetime = null,
                                ServiceId = Comparison.Order.Designer.Id,
                                UserRequestId = request.Id
                            });
                        }
                        if (Comparison.Order.Engeneer != null)
                        {
                            ctx.RequestOptions.Add(new RequestOption
                            {
                                Datetime = null,
                                ServiceId = Comparison.Order.Engeneer.Id,
                                UserRequestId = request.Id
                            });
                        }
                        ctx.SaveChanges();

                        MessageBox.Show("Успех");
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("Ошибка");
                }

            }
        }

        private void GoToSignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            (new SignUp()).Show();
        }

        public bool FillConract(DataToComparison data)
        {
            var app = new Word.Application();
            try
            {
                var dict = new Dictionary<string, string>()
                {
                    { "<DATE>", DateTime.Now.ToString() },
                    { "<FIO>", account.Fname + " " + account.Surname + " " + account.Patronumic },
                    { "<PASSPORT>", account.Passport },
                    { "<PHONE>", account.Phone },
                    { "<EMAIL>", account.Email },
                    { "<REPAIR_TYPE>", data.RepairType.Name },
                    { "<OBJECT_TYPE>", data.RepairObject.Name },
                    { "<SQUARE>", data.Square.ToString() },
                    { "<DESIGNER>", data.Designer.Price.ToString() },
                    { "<INGENEER>", data.Engeneer.Price.ToString() },
                    { "<FULL_PRICE>", data.FullPrice.ToString() },
                    { "<ADDRESS>", data.Address }
                };

                var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ДОГОВОР НА РЕМОНТ.docx");
                var newFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ДОГОВОР.docx");

                // Копирование шаблона на рабочий стол
                File.Copy(templatePath, newFilePath, true);

                var missing = Type.Missing;

                var doc = app.Documents.Open(newFilePath);

                foreach(var item in dict)
                {
                    var find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    var wrap = Word.WdFindWrap.wdFindContinue;
                    var replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(
                        FindText: item.Key,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: false,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: item.Value,
                        Replace: replace
                    );
                }
                doc.Save();
                doc.Close(false, missing, missing);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                app.Quit();
            }
        }
    }
}
