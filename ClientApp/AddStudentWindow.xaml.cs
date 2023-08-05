using Azure;
using ClientApp.Classes;
using Shared.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public partial class AddStudentWindow : Window
    {

        private readonly HttpClient httpClient = HTTPClientInstance.Instance;
        public AddStudentWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)//async buttonclick?
        {
            // Проверка полей на пустоту
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AdressTextBox.Text) || string.IsNullOrWhiteSpace(AgeTextBox.Text) ||
               GenderCombobox.SelectedItem == null)
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

            string gender = GenderCombobox.SelectedItem.ToString().Substring(37);
            Student newStudent = new Student
            {
                name = NameTextBox.Text,
                surname = SurnameTextBox.Text,
                adress = AdressTextBox.Text,
                email = EmailTextBox.Text,
                age = int.Parse(AgeTextBox.Text),
                gender = gender
             
            };
            HttpContent content =JsonContent.Create(newStudent);
            try
            {
                HttpResponseMessage response = httpClient.PostAsync("http://localhost/students/create", content).Result;//await
                if (response.IsSuccessStatusCode)
                {
                    MessageTextBlock.Text = "Student registered succesfully!";
                }
                else
                {
                    MessageTextBlock.Text = "Register error.";
                }
            }
                catch (Exception ex)
            {
                MessageTextBlock.Text = "An error occurred: " + ex.Message;

            }
            
        }

      
    }
}
