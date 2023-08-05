using ClientApp.Classes;
using Shared.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window
    {
        private readonly HttpClient httpClient = HTTPClientInstance.Instance;
        Student student { get; set; }
        public EditStudentWindow(Student student)
        {
            InitializeComponent();
            this.NameTextBox.Text = student.name;
            this.SurnameTextBox.Text = student.surname;
            this.AgeTextBox.Text = student.age.ToString();
            this.GenderTextBox.Text = student.gender;
            this.GradeTextBox.Text = student.grade.ToString();
            this.AdressTextBox.Text = student.adress;
            this.EmailTextBox.Text = student.email;
            this.student = student;
        }
        private void EmailChange_Click(object sender, RoutedEventArgs e)
        {
            HttpContent content = JsonContent.Create(student);
            HttpResponseMessage response = httpClient.PutAsync((@$"http://localhost/students/changeemail/{this.EmailTextBox.Text}"),content).Result;
            response.EnsureSuccessStatusCode();

        }

        private void AdressCahnge_Click(object sender, RoutedEventArgs e)
        {
            HttpContent content = JsonContent.Create(student);
            HttpResponseMessage response = httpClient.PutAsync((@$"http://localhost/students/changeadress/{this.AdressTextBox.Text}"), content).Result;
            response.EnsureSuccessStatusCode();
        }

        private void GradeCahge_Click(object sender, RoutedEventArgs e)
        {
            HttpContent content = JsonContent.Create(student);
            HttpResponseMessage response = httpClient.PutAsync((@$"http://localhost/students/changegrade/{this.GradeTextBox.Text}"), content).Result;
            response.EnsureSuccessStatusCode();
        }
    }
}
