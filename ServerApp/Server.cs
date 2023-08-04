using BackApp.Repositories;
using System.Net;

HttpListener httpListener = new HttpListener();
const int port = 80;
httpListener.Prefixes.Add($"http://*:{port}/");
httpListener.Start();

const string connectionString = "Data Source=.;Initial Catalog=StudentsDb;Integrated Security=True";
var usersRepository = new StudentRepositoryDapper(connectionString);

while (true)
{
    HttpListenerContext context = await httpListener.GetContextAsync();

}
