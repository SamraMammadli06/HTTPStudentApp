using Azure;
using ClientApp.Classes;
using Shared.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApp
{
    public partial class RegisterWindow : Window
    {

        private readonly HttpClient httpClient = HTTPClientInstance.Instance;
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)//async buttonclick?
        {
            // Проверка полей на пустоту
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) || string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(AdressTextBox.Text) || string.IsNullOrWhiteSpace(AgeTextBox.Text) ||
                string.IsNullOrWhiteSpace(GenderTextBox.Text))
            {
                MessageTextBlock.Text = "Fill all empty boxes!";
                return;
            }
            // Проверка возраста на числовое значение
            if (!int.TryParse(AgeTextBox.Text, out int age))
            {
                MessageTextBlock.Text = "Non correct age!";
                return;
            }

            Student newStudent = new Student
            {
                name = NameTextBox.Text,
                surname = SurnameTextBox.Text,
                adress = AdressTextBox.Text,
                email = EmailTextBox.Text,
                age = int.Parse(AgeTextBox.Text),
                gender = GenderTextBox.Text
            };
           
            HttpContent content =JsonContent.Create(newStudent);
            HttpResponseMessage response = httpClient.PostAsync("http://localhost/students/create", content).Result;//await
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Student registered succesfully!");
                // переход на другую страницу после успешной регистрации.
            }
            else
            {
                MessageTextBlock.Text = "Register error.";
            }
        }

      
    }
}
