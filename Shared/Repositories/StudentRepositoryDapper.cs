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
        public async Task<Student> FindAsync(int id)
        {
            var student = await this.connection.QueryFirstAsync<Student>(
                sql: $@"select * from Students s
                    where s.Id = @id",
                param: new { id });

            return student;
        }

        public async Task CreateAsync(Student student)
        {
            await this.connection.ExecuteAsync(
         sql: @"INSERT INTO Students(Name, Surname, Age, Gender, Email, Address)
                VALUES(@Name, @Surname, @Age, @Gender, @Email, @Address)",
         param: new { student.name, student.surname });
    
        }
    }
}
