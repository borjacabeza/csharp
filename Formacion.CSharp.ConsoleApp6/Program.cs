using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Formacion.CSharp.Objects;

namespace Formacion.CSharp.ConsoleApp6
{
    internal class Program
    {
        private static HttpClient http = new HttpClient();

        /// <summary>
        /// Inicio del programa
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            http.BaseAddress = new Uri("http://school.labs.com.es/api/students/");

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("".PadRight(56, '*'));
                Console.WriteLine("*  DEMO Y EJERCICIOS".PadRight(55) + "*");
                Console.WriteLine("".PadRight(56, '*'));
                Console.WriteLine("*".PadRight(55) + "*");
                Console.WriteLine("*  1. Ejemplo GET".PadRight(55) + "*");
                Console.WriteLine("*  2. Ejemplo GET abreviado".PadRight(55) + "*");
                Console.WriteLine("*  3. Ejemplo POST".PadRight(55) + "*");
                Console.WriteLine("*  4. Ejemplo POST abreviado".PadRight(55) + "*");
                Console.WriteLine("*  5. Ejemplo PUT".PadRight(55) + "*");
                Console.WriteLine("*  6. Ejemplo PUT abreviado".PadRight(55) + "*");
                Console.WriteLine("*  7. Ejemplo DELETE".PadRight(55) + "*");
                Console.WriteLine("*  8. Ejemplo ECO".PadRight(55) + "*");
                Console.WriteLine("*  9. Salir".PadRight(55) + "*");
                Console.WriteLine("*".PadRight(55) + "*");
                Console.WriteLine("".PadRight(56, '*'));

                Console.WriteLine(Environment.NewLine);
                Console.Write("   Opción: ");

                Console.ForegroundColor = ConsoleColor.Cyan;

                int.TryParse(Console.ReadLine(), out int opcion);
                switch (opcion)
                {
                    case 1:
                        GetStudent();
                        break;
                    case 2:
                        GetStudent2();
                        break;
                    case 3:
                        PostStudent();
                        break;
                    case 4:
                        PostStudent2();
                        break;
                    case 5:
                        PutStudent();
                        break;
                    case 6:
                        PutStudent2();
                        break;
                    case 7:
                        DeleteStudent();
                        break;
                    case 8:
                        Eco();
                        break;
                    case 9:
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Environment.NewLine + $"La opción {opcion} no es valida.");
                        break;
                }

                Console.WriteLine(Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Pulsa una tecla para continuar...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// GET, leer y mostrar los datos de un alumno mediante su ID
        /// </summary>
        static void GetStudent()
        {
            Console.Clear();
            Console.Write("ID Estudiante: ");

            // Se realiza la llamada GET al APIRest 
            var response = http.GetAsync(Console.ReadLine()).Result;

            // Se analiza el código de estado de la respuesta (HttpStatusCode.OK equivalente a 200)
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Leer los datos de la respuesta con formato JSON mediante -> response.Content.ReadAsStringAsync().Result
                // Deserializamos para tranformar los datos JSON a un objeto
                var data = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);

                // Mostramos la información directamente desde el objeto
                Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                Console.WriteLine($"     Clase: {data.ClassId}");
            }
            else Console.WriteLine($"Error: {response.StatusCode}.");
        }

        /// <summary>
        /// GET, leer y mostrar los datos de un estudiante mediante su ID
        /// Utilizamos el método .GetFromJsonAsync() que necesita de la depedencia Microsoft.AspNet.WebApi.Client
        /// </summary>
        static void GetStudent2()
        {
            Console.Clear();
            Console.Write("ID Estudiante: ");

            try
            {
                var data = http.GetFromJsonAsync<Student>(Console.ReadLine()).Result;
                if (data != null)
                {
                    Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                    Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                    Console.WriteLine($"     Clase: {data.ClassId}");
                }
                else Console.WriteLine($"Estudiante no encontrado.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Estudiante no encontrado.");
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// POST, insertar los datos de un nuevo estudiante
        /// </summary>
        static void PostStudent()
        {
            // Creamos un objeto Student y damos valores a sus propiedades
            var student = new Student();

            Console.Clear();
            Console.Write("Nombre: ");
            student.Firstname = Console.ReadLine();
            Console.Write("Apellidos: ");
            student.Lastname = Console.ReadLine();
            Console.Write("Fecha de Nacimiento: ");
            student.DateOfBirth = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Curso: ");
            student.ClassId = Convert.ToInt32(Console.ReadLine());

            // Serializamos para convertir el objeto en JSON
            var studentJSON = JsonConvert.SerializeObject(student);

            // Creamos el contenido del mensaje HTTP, especificado el Encoding y el Content-Type
            var content = new StringContent(studentJSON, Encoding.UTF8, "application/json");

            // Se realiza la llamada POST al APIRest 
            var response = http.PostAsync("", content).Result;

            // Se analiza el código de estado de la respuesta (HttpStatusCode.Created equivalente a 201)
            if (response.StatusCode == HttpStatusCode.Created)
            {
                // Las llamadas POST suele responder con el objeto creado, donde podemos ver el Id asignado al
                // nuevo registro del estudiante

                // Leer los datos de la respuesta con formato JSON mediante -> response.Content.ReadAsStringAsync().Result
                // Deserializamos para tranformar los datos JSON a un objeto
                var data = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);

                // Mostramos la información directamente desde el objeto
                Console.WriteLine($"        ID: {data.Id}");
                Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                Console.WriteLine($"     Clase: {data.ClassId}");
            }
            else Console.WriteLine($"Error: {response.StatusCode}.");
        }

        /// <summary>
        /// POST, insertar los datos de un nuevo estudiante
        /// Utilizamos el método .PostAsJsonAsync() que necesita de la depedencia Microsoft.AspNet.WebApi.Client
        /// </summary>
        static void PostStudent2()
        {
            var student = new Student();

            Console.Clear();
            Console.Write("Nombre: ");
            student.Firstname = Console.ReadLine();
            Console.Write("Apellidos: ");
            student.Lastname = Console.ReadLine();
            Console.Write("Fecha de Nacimiento: ");
            student.DateOfBirth = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Curso: ");
            student.ClassId = Convert.ToInt32(Console.ReadLine());

            try
            {
                var response = http.PostAsJsonAsync<Student>("", student).Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var data = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);

                    Console.WriteLine($"        ID: {data.Id}");
                    Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                    Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                    Console.WriteLine($"     Clase: {data.ClassId}");
                }
                else Console.WriteLine($"Error: {response.StatusCode}.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al insertar un alumno.");
                Console.WriteLine($"Error: {e}.");
            }
        }

        /// <summary>
        /// PUT, modificar los datos de un estudiante
        /// </summary>
        static void PutStudent()
        {
            Console.Clear();
            Console.Write("ID Estudiante: ");

            try
            {
                // Cargamos los datos del estudiante que necesitamos modificar
                var data = http.GetFromJsonAsync<Student>(Console.ReadLine()).Result;

                Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                Console.WriteLine($"     Clase: {data.ClassId}");

                // Recogemos las modificaciones
                Student student = new Student();
                string temp;

                student.Id = data.Id;
                Console.Write("Nombre: ");
                student.Firstname = (temp = Console.ReadLine()) == "" ? data.Firstname : temp;
                Console.Write("Apellidos: ");
                student.Lastname = (temp = Console.ReadLine()) == "" ? data.Lastname : temp;
                Console.Write("Fecha de Nacimiento: ");
                student.DateOfBirth = (temp = Console.ReadLine()) == "" ? data.DateOfBirth : Convert.ToDateTime(temp);
                Console.Write("Curso: ");
                student.ClassId = (temp = Console.ReadLine()) == "" ? data.ClassId : Convert.ToInt32(temp);

                // Serializamos para convertir el objeto en JSON
                var studentJSON = JsonConvert.SerializeObject(student);

                // Creamos el contenido del mensaje HTTP, especificado el Encoding y el Content-Type
                var content = new StringContent(studentJSON, Encoding.UTF8, "application/json");

                // Se realiza la llamada PUT al APIRest 
                var response = http.PutAsync(student.Id.ToString(), content).Result;

                // Se analiza el código de estado de la respuesta (HttpStatusCode.NoContent equivalente a 204)
                if (response.StatusCode == HttpStatusCode.NoContent) Console.WriteLine($"Registro {data.Id} modificado correctamente.");
                else Console.WriteLine($"Error: {response.StatusCode}.");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// PUT, modificar los datos de un estudiante
        /// Utilizamos el método .PutAsJsonAsync() que necesita de la depedencia Microsoft.AspNet.WebApi.Client
        /// </summary>
        static void PutStudent2()
        {
            Console.Clear();
            Console.Write("ID Estudiante: ");

            try
            {
                var data = http.GetFromJsonAsync<Student>(Console.ReadLine()).Result;

                Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                Console.WriteLine($"     Clase: {data.ClassId}");

                Student student = new Student();
                string temp;

                student.Id = data.Id;
                Console.Write("Nombre: ");
                student.Firstname = (temp = Console.ReadLine()) == "" ? data.Firstname : temp;
                Console.Write("Apellidos: ");
                student.Lastname = (temp = Console.ReadLine()) == "" ? data.Lastname : temp;
                Console.Write("Fecha de Nacimiento: ");
                student.DateOfBirth = (temp = Console.ReadLine()) == "" ? data.DateOfBirth : Convert.ToDateTime(temp);
                Console.Write("Curso: ");
                student.ClassId = (temp = Console.ReadLine()) == "" ? data.ClassId : Convert.ToInt32(temp);

                var response = http.PutAsJsonAsync<Student>(student.Id.ToString(), student).Result;
                if (response.StatusCode == HttpStatusCode.NoContent) Console.WriteLine($"Registro {data.Id} modificado correctamente.");
                else Console.WriteLine($"Error: {response.StatusCode}.");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// DELETE, eliminar los datos de un estudiante
        /// </summary>
        static void DeleteStudent()
        {
            Console.Clear();
            Console.Write("ID Estudiante: ");

            try
            {
                // Cargamos los datos del estudiante que necesitamos eliminar
                var data = http.GetFromJsonAsync<Student>(Console.ReadLine()).Result;

                Console.WriteLine($"    Nombre: {data.Firstname} {data.Lastname}");
                Console.WriteLine($"Nacimiento: {data.DateOfBirth}");
                Console.WriteLine($"     Clase: {data.ClassId}");
                Console.WriteLine("¿ Desea eliminarlo ? S/N");

                if (Console.ReadKey().KeyChar.ToString().ToLower() == "s")
                {
                    // Se realiza la llamada DELETE al APIRest 
                    var response = http.DeleteAsync(data.Id.ToString()).Result;
                    Console.WriteLine(Environment.NewLine);

                    // Se analiza el código de estado de la respuesta (HttpStatusCode.OK equivalente a 200)
                    if (response.StatusCode == HttpStatusCode.OK) Console.WriteLine($"Eliminado {data.Firstname} {data.Lastname}");
                    else Console.WriteLine($"Error: {response.StatusCode}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Ejemplo de una llamada POST con cabeceras
        /// </summary>
        static void Eco()
        {
            http = new HttpClient();
            http.BaseAddress = new Uri("https://postman-echo.com/");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Headers
            /////////////////////////////////////////////////////////////////////////////////////////////////////////           

            // Conjunto de clave/valor, datos que enviamos en la cabecera del mensaje
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("x-param-1", "D E M O");
            http.DefaultRequestHeaders.Add("User-Agent", "Ejercicio Demo");

            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Body
            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Objeto anónimo, datos que enviamos como contenido del mensaje en el body
            var obj = new
            {
                PostalCode = "28014",
                Country = "Spain",
                CountryCode = "SP",
                Places = new dynamic[]
                {
                    new { Name = "Madrid", State = "Madrid", StateCode = "M" },
                    new { Name = "Madrid", State = "Madrid", StateCode = "M" }
                }
            };

            // Creamos el contenido del mensaje HTTP, especificado el Encoding y el Content-Type
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");


            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Llamada al API y procesamiento de la respuesta
            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Se realiza la llamada POST al APIRest 
            var response = http.PostAsync("post?param1=demo1&para2=demo2", content).Result;

            // Se analiza el código de estado de la respuesta (HttpStatusCode.OK equivalente a 200)
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Leer los datos de la respuesta con formato JSON mediante -> response.Content.ReadAsStringAsync().Result
                var data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(data);
            }
            else Console.WriteLine($"Error: {response.StatusCode}");

            Console.ReadKey();
        }
    }
}

namespace Formacion.CSharp.Objects
{
    /// <summary>
    /// Objeto Student para los ejercicios de demostración
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ClassId { get; set; }
    }
}

