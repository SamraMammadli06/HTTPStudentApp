using ClientApp.Classes;
using Shared.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
 
    public partial class MainWindow : Window
    {
        private readonly HttpClient httpClient = HTTPClientInstance.Instance;
        private List<Student> students;
        public MainWindow()
        {
            InitializeComponent();
            LoadStudents();
        }

        private async void LoadStudents()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost/students/getall");
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                students = JsonSerializer.Deserialize<List<Student>>(responseJson);
                StudentsListView.ItemsSource = students;
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Failed to fetch students from the server.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Открыть окно для добавления студента
            AddStudentWindow addStudentWindow = new AddStudentWindow();
            addStudentWindow.ShowDialog();

            // Обновить список студентов после закрытия окна добавления
            LoadStudents();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбран ли студент для изменения
            if (StudentsListView.SelectedItem == null)
            {
                MessageBox.Show("Please select a student to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Получение выбранного студента
            Student selectedStudent = (Student)StudentsListView.SelectedItem;

            // Открыть окно для изменения студента
            //EditStudentWindow editStudentWindow = new EditStudentWindow(selectedStudent);
            //editStudentWindow.ShowDialog();

            // Обновить список студентов после закрытия окна изменения
            LoadStudents();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбран ли студент для удаления
            if (StudentsListView.SelectedItem == null)
            {
                MessageBox.Show("Please select a student to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Получение выбранного студента
            Student selectedStudent = (Student)StudentsListView.SelectedItem;

            try
            {
                // Отправить запрос на удаление студента по его ID
                HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost/students/delete/{selectedStudent.id}");
                response.EnsureSuccessStatusCode();

                // Показать сообщение об успешном удалении
                MessageBox.Show($"Student '{selectedStudent.name} {selectedStudent.surname}' deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновить список студентов после удаления
                LoadStudents();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"Failed to delete student '{selectedStudent.name} {selectedStudent.surname}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                // Если поле поиска пустое, показываем всех студентов
                StudentsListView.ItemsSource = students;
            }
            else
            {
                // Фильтруем список студентов по имени (или другим полям, если нужно)
                List<Student> filteredStudents = students.FindAll(s => s.name.ToLower().Contains(searchText.ToLower()));
                StudentsListView.ItemsSource = filteredStudents;
            }
        }
        private void StudentsListView_SelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Обработчик события изменения выбранного студента (при необходимости)
            // Здесь можно показать дополнительную информацию о студенте или выполнить другие действия.
        }
    }
}
