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

        public async Task CreateAsync(Student student)
        {
            await this.connection.ExecuteAsync(
         sql: @"INSERT INTO Students(Name, Surname, Age, Gender, Email, Address)
                VALUES(@Name, @Surname, @Age, @Gender, @Email, @Address, @Grades)",
         param: new { student.name, student.surname, student.age, student.gender,student.email,student.adress,student.grade });
    
        }
        

    }
}
