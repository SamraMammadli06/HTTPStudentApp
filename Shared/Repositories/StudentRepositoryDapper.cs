using Dapper;
using Shared.Classes;
using System.Data;
using System.Data.SqlClient;

namespace BackApp.Repositories
{
    public class StudentRepositoryDapper
    {
        private readonly IDbConnection connection;

        public StudentRepositoryDapper(string connectionString)
        {
             this.connection = new SqlConnection(connectionString);
        }

        #region Post
        public async Task CreateAsync(Student student)
        {
            await this.connection.ExecuteAsync(
         sql: @"INSERT INTO Students(Name, Surname, Age, Gender, Email, Address)
                VALUES(@Name, @Surname, @Age, @Gender, @Email, @Address, @Grades)",
         param: new { student.name, student.surname, student.age, student.gender,student.email,student.adress,student.grade });
    
        }
        #endregion

        #region Get
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
          var students =  await this.connection.QueryAsync<Student>("SELECT * FROM Students");
          return students;
        }
        public async Task<Student> FindByIdAsync(int id)
        {
            var student = await this.connection.QueryFirstAsync<Student>(
                sql: $@"select * from Students s
                    where s.Id = @id",
                param: new { id });

            return student;
        }

        public async Task<IEnumerable<Student>> FindByNameAsync(string name)
        {
            var students = await this.connection.QueryAsync<Student>(
                sql: $@"select * from Students s
                    where s.Name = @name",
                param: new { name });
            return students;
        }

        #endregion

        #region Put
        public async Task ChangeAdressAsync(Student student, string newAdress)
        {
            if(student.adress == newAdress) 
                return;
            await this.connection.ExecuteAsync(
                sql: @"update Students
                        set Adress = @newAdress
                        where Id = @id",
                param: new { newAdress,student.id });
            student.adress = newAdress;
        }
        public async Task ChangeEmailAsync(Student student, string newEmail)
        {
            if (student.email == newEmail)
                return;
            await this.connection.ExecuteAsync(
                sql: @"update Students
                        set Email = @newEmail
                        where Id = @id",
                param: new { newEmail, student.id });
            student.email = newEmail;
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(int id)
        {
            await this.connection.ExecuteAsync(
                sql: @"delete from Students 
                         where Id = @id",
                param: new { id });
        }
        #endregion
    }
}
