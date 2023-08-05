using BackApp.Repositories;
using Shared.Classes;
using System.Net;
using System.Text.Json;

HttpListener httpListener = new HttpListener();
const int port = 80;
httpListener.Prefixes.Add($"http://*:{port}/");
httpListener.Start();

const string connectionString = "Data Source=.;Initial Catalog=StudentsDb;Integrated Security=True";
//const string connectionString = $"Server=localhost;Database=StudentsDb;User Id=admin;Password=admin;";

var studentsRepository = new StudentRepositoryDapper(connectionString);

while (true)
{
    HttpListenerContext context = await httpListener.GetContextAsync();

    string requestPath = context.Request.Url?.AbsolutePath!;

    var requestPathItems = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries)
        .Take(2);

    string responseJson = string.Empty;

    if (requestPath.Length == 0)
        break;
    switch (requestPathItems.First().ToLower())
    {
        case "students":
            {
                switch (requestPathItems.Last().ToLower())
                {
                    case "getall":
                        if (context.Request.HttpMethod.ToLower() == "get")
                        {
                            var allUsers = await studentsRepository.GetAllAsync();
                            responseJson = JsonSerializer.Serialize(allUsers);
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                    case "getbyname":
                        if (context.Request.HttpMethod.ToLower() == "get")
                        {
                            var name = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

                            if (name == null)
                            {
                                context.Response.StatusCode = 400;
                                break;
                            }
                            var users= await studentsRepository.FindByNameAsync(name);
                            responseJson = JsonSerializer.Serialize(users);
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                    case "getbyid":
                        if (context.Request.HttpMethod.ToLower() == "get")
                        {
                            using var reader = new StreamReader(context.Request.InputStream);
                            var requestJson = await reader.ReadToEndAsync();
                            var id = JsonSerializer.Deserialize<int>(requestJson);

                            if (id == null)
                            {
                                context.Response.StatusCode = 400;
                                break;
                            }
                            await studentsRepository.FindByIdAsync(id);
                            context.Response.StatusCode = 201;
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                    case "changeadress":
                        if (context.Request.HttpMethod.ToLower() == "put")
                        {
                            using var reader = new StreamReader(context.Request.InputStream);
                            var requestJson = await reader.ReadToEndAsync();
                            var adress = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                            var student = JsonSerializer.Deserialize<Student>(requestJson);

                            if (student == null)
                            {
                                context.Response.StatusCode = 400;
                                break;
                            }

                            await studentsRepository.ChangeAdressAsync(student,adress);
                            context.Response.StatusCode = 201;
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                    case "changegrade":
                        if (context.Request.HttpMethod.ToLower() == "put")
                        {
                            using var reader = new StreamReader(context.Request.InputStream);
                            var requestJson = await reader.ReadToEndAsync();
                            var gradeStr = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                            var grade = int.Parse(gradeStr);
                            var student = JsonSerializer.Deserialize<Student>(requestJson);

                            if (student == null)
                            {
                                context.Response.StatusCode = 400;
                                break;
                            }

                            await studentsRepository.ChangeGradeAsync(student, grade);
                            context.Response.StatusCode = 201;
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                    case "changeemail":
                        if (context.Request.HttpMethod.ToLower() == "put")
                        {
                            using var reader = new StreamReader(context.Request.InputStream);
                            var requestJson = await reader.ReadToEndAsync();
                            
                            var student = JsonSerializer.Deserialize<Student>(requestJson);

                            if (student == null)
                            {
                                context.Response.StatusCode = 400;
                                break;
                            }
                            var email = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                            await studentsRepository.ChangeEmailAsync(student, email);
                            context.Response.StatusCode = 201;
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                    case "create":
                        if (context.Request.HttpMethod.ToLower() == "post")
                        {
                            using var reader = new StreamReader(context.Request.InputStream);
                            var requestJson = await reader.ReadToEndAsync();
                            var student = JsonSerializer.Deserialize<Student>(requestJson);

                            if (student == null)
                            {
                                context.Response.StatusCode = 400;
                                break;
                            }

                            await studentsRepository.CreateAsync(student);
                            context.Response.StatusCode = 201;
                            responseJson = $"User '{student.name} {student.surname}' created successfully!";
                            context.Response.ContentType = "plain/text";
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;


                    case "delete":
                        if (context.Request.HttpMethod.ToLower() == "delete")
                        {
                            var strId = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                           int id = int.Parse(strId);
                            await studentsRepository.DeleteAsync(id);
                            context.Response.StatusCode = 201;
                            context.Response.ContentType = "plain/text";
                        }
                        else
                        {
                            responseJson = "Error: Invalid method type!";
                            context.Response.ContentType = "plain/text";
                            context.Response.StatusCode = 400;
                        }
                        break;

                }
                break;
            }
        default:
            break;
    }
    using var writer = new StreamWriter(context.Response.OutputStream);
    await writer.WriteLineAsync(responseJson);
    await writer.FlushAsync();
}
