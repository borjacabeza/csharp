using Formacion.CSharp.ConsoleAppLINQ.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Formacion.CSharp.ConsoleAppLINQ
{
    class Program
    {
        /// <summary>
        /// Inicio de la aplicación
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("".PadRight(56, '*'));
                Console.WriteLine("*  DEMOS CON LINQ".PadRight(55) + "*");
                Console.WriteLine("".PadRight(56, '*'));
                Console.WriteLine("*".PadRight(55) + "*");
                Console.WriteLine("*  1. Consultas Básicas DataList".PadRight(55) + "*");
                Console.WriteLine("*  2. Consultas con ADO.NET".PadRight(55) + "*");
                Console.WriteLine("*  3. Operaciones con EntityFramework".PadRight(55) + "*");
                Console.WriteLine("*  4. Ejercicios 30/03/2022".PadRight(55) + "*");
                Console.WriteLine("*  5. Ejercicios 31/03/2022".PadRight(55) + "*");
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
                        ConsultasBasicas();
                        break;
                    case 2:
                        ConsultaConADONET();
                        break;
                    case 3:
                        OperatividadConEF();
                        break;
                    case 4:
                        Ejercicios();
                        break;
                    case 5:
                        Ejercicios2();
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
        /// Consultas básicas con LINQ y DataList
        /// </summary>
        static void ConsultasBasicas()
        {
            // Equivalente a: SELECT * FROM ListaProductos

            var resultado1a = DataLists.ListaProductos
                .ToList();

            var resultado1b = from r in DataLists.ListaProductos
                              select r;


            // Equivalente a: SELECT * FROM ListaProductos WHERE precio > 2

            var resultado2a = DataLists.ListaProductos
                .Where(x => x.Precio > 2)
                .ToList();

            var resultado2b = from r in DataLists.ListaProductos
                              where r.Precio > 2
                              select r;


            // Equivalente a: SELECT * FROM ListaProductos WHERE precio > 2 ORDER BY precio DESC

            var resultado3a = DataLists.ListaProductos
                .Where(x => x.Precio > 2)
                .OrderByDescending(x => x.Precio)
                .ToList();

            var resultado3b = from r in DataLists.ListaProductos
                              where r.Precio > 2
                              orderby r.Precio descending
                              select r;


            // Equivalente a: SELECT Descripcion, Precio FROM ListaProductos WHERE precio > 2 ORDER BY precio DESC
            var resultado4a = DataLists.ListaProductos
                .Where(x => x.Precio > 2)
                .OrderByDescending(x => x.Precio)
                .Select(x => new { x.Descripcion, x.Precio })
                .ToList();

            var resultado4b = from r in DataLists.ListaProductos
                              where r.Precio > 2
                              orderby r.Precio descending
                              select new { r.Descripcion, r.Precio };

            foreach (var item in resultado1a)
            {
                Console.WriteLine($"{item.Descripcion}  {item.Precio}");
            }
        }

        /// <summary>
        /// Ejecutar una consulta contra una base de datos utilizando ADO.NET
        /// </summary>
        static void ConsultaConADONET()
        {
            /////////////////////////////////////////////////////////////////////////////////
            // ADO.NET Access Data Object (manejamos la base de datos con Transat-SQL)
            /////////////////////////////////////////////////////////////////////////////////
            // Consulta de Datos - SELECT
            // Equivalente a: SELECT * FROM Customers
            /////////////////////////////////////////////////////////////////////////////////

            //Creamos un objeto para definir la cadena de conexión
            var connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = "LOCALHOST",
                InitialCatalog = "NORTHWIND",
                UserID = "",
                Password = "",
                IntegratedSecurity = true,
                ConnectTimeout = 60
            };

            //Muestra la cadena de conexión resultante con los datos introducidos
            Console.WriteLine("Cadena de Conexión: {0}", connectionString.ToString());

            //Creamos un objeto conexión, representa la conexión con la base de datos
            var connect = new SqlConnection()
            {
                ConnectionString = connectionString.ToString()
            };

            //Comprobamos el estado de la conexión antes y después de conectarnos con la Base de Datos
            Console.WriteLine($"Estado: {connect.State.ToString()}");
            connect.Open();
            Console.WriteLine($"Estado: {connect.State.ToString()}");

            //Creamos un objeto Command que nos permite lanzar comando contra la base de datos
            var command = new SqlCommand()
            {
                Connection = connect,
                CommandText = "SELECT * FROM dbo.Customers"
            };

            //Creamos un objeto que funcione como curso, permitiendo recorrer los datos retornados por la base de datos
            var reader = command.ExecuteReader();

            if (reader.HasRows == false) Console.WriteLine("Registros no encontrados.");
            else
            {
                while (reader.Read() == true)
                {
                    Console.WriteLine($"ID: {reader["CustomerID"]}");
                    Console.WriteLine($"Empresa: {reader.GetValue(1)}");
                    Console.WriteLine($"Pais: {reader["Country"]}" + Environment.NewLine);
                }
            }

            //Cerramos conexiones y destruimos variables
            reader.Close();
            command.Dispose();
            connect.Close();
            connect.Dispose();
        }

        /// <summary>
        /// Ejercutas consultas, inserciones, actualizaciones y borrado de datos utilizando EntityFrameworkCore
        /// </summary>
        static void OperatividadConEF()
        {
            /////////////////////////////////////////////////////////////////////////////////
            // EntityFramework (manejamos las base de datos como colecciones)
            /////////////////////////////////////////////////////////////////////////////////
            
            //Declaración de la variable de contexto
            var context = new ModelNorthwind();


            /////////////////////////////////////////////////////////////////////////////////
            // Consulta de Datos - SELECT
            // Equivalente a: SELECT * FROM Customers
            /////////////////////////////////////////////////////////////////////////////////
            
            var clientes = context.Customers
                .ToList();

            var clientes2 = from c in context.Customers
                            select c;


            //Equivalente a: SELECT * FROM Customers WHERE Country = 'Spain' ORDER BY City

            var clientes3 = context.Customers
                .Where(r => r.Country == "Spain")
                .OrderBy(r => r.City)
                .ToList();

            var clientes4 = from c in context.Customers
                            where c.Country == "Spain"
                            orderby c.City
                            select c;

            foreach (var c in clientes3)
            {
                Console.WriteLine($"ID: {c.CustomerID}");
                Console.WriteLine($"Empresa: {c.CompanyName}");
                Console.WriteLine($"Pais: {c.Country}" + Environment.NewLine);
            }

            /////////////////////////////////////////////////////////////////////////////////
            // Insertar Datos - INSERT
            // Equivalente a: INSERT INTO Customers VALUES(..., ..., )
            /////////////////////////////////////////////////////////////////////////////////

            var cliente = new Customer();

            cliente.CustomerID = "DEMO1";
            cliente.CompanyName = "Empresa Uno, SL";
            cliente.ContactName = "Borja Cabeza";
            cliente.ContactTitle = "Gerente";
            cliente.Address = "Avenida del Mediterraneo, 10";
            cliente.PostalCode = "28010";
            cliente.City = "Madrid";
            cliente.Country = "Spain";
            cliente.Phone = "910 000 001";
            cliente.Fax = "910 000 002";

            context.Customers.Add(cliente);
            context.SaveChanges();


            /////////////////////////////////////////////////////////////////////////////////
            // Modificar Datos - UPDATE
            // Equivalente a: UPDATE Customers SET CompanyName = 'nuevo valor' WHERE CustomerID = 'DEMO1'
            /////////////////////////////////////////////////////////////////////////////////

            var cliente2a = context.Customers
                .Where(r => r.CustomerID == "DEMO1")
                .FirstOrDefault();

            cliente2a.CompanyName = "Empresa Uno Dos y Tres, SL";
            cliente2a.PostalCode = "28014";


            var cliente2b = (from c in context.Customers
                             where c.CustomerID == "DEMO1"
                             select c).FirstOrDefault();

            cliente2b.CompanyName = "Empresa Uno Dos y Tres, SL";
            cliente2b.PostalCode = "28014";

            context.SaveChanges();


            /////////////////////////////////////////////////////////////////////////////////
            // Eliminar Datos - DELETE
            // Equivalente a: DELETE Customers WHERE CustomerID = 'DEMO1'
            /////////////////////////////////////////////////////////////////////////////////

            var cliente3a = context.Customers
                .Where(r => r.CustomerID == "DEMO1")
                .FirstOrDefault();

            context.Customers.Remove(cliente3a);
            context.SaveChanges();

            //Elimina el registro con CustomerID igual a DEMO1
            context.Customers.Remove(context.Customers.Where(r => r.CustomerID == "DEMO1").FirstOrDefault());
            context.SaveChanges();

            //Elimina todos los registros donde País es igual a Spain
            context.Customers.RemoveRange(context.Customers.Where(r => r.Country == "Spain").ToList());
            context.SaveChanges();

        }

        /// <summary>
        /// Ejercicios realizados el 30/03/2022
        /// </summary>
        static void Ejercicios()
        {
            /////////////////////////////////////////////////////////////////////////////////
            // Clientes nacidos entre 1980 y 1990
            /////////////////////////////////////////////////////////////////////////////////

            var clientes1 = DataLists.ListaClientes
                .Where(x => x.FechaNac.Year >= 1980 && x.FechaNac.Year < 1990)
                .ToList();

            Console.WriteLine("CLIENTES: nacidos entre 1980 y 1990");
            Console.WriteLine("==================================================================");
            foreach (var cliente in clientes1) Console.WriteLine($"{cliente.Nombre}");
            Console.WriteLine("");


            /////////////////////////////////////////////////////////////////////////////////
            // Clientes mayores de 25 años
            /////////////////////////////////////////////////////////////////////////////////
            
            var clientes2 = DataLists.ListaClientes
                .Where(x => x.FechaNac.AddYears(25) <= DateTime.Now)
                .ToList();

            var clientes2b = DataLists.ListaClientes
                .Where(x => DateTime.Now.Subtract(x.FechaNac).Days / 365 > 25)
                .ToList();

            var clientes2c = DataLists.ListaClientes
                .Where(x => DateTime.Now.Year - x.FechaNac.Year > 25)
                .ToList();

            Console.WriteLine("CLIENTES: mayores de 25 años");
            Console.WriteLine("==================================================================");
            foreach (var cliente in clientes2) Console.WriteLine($"{cliente.Nombre}");
            Console.WriteLine("");


            /////////////////////////////////////////////////////////////////////////////////
            // Producto con el precio más alto
            /////////////////////////////////////////////////////////////////////////////////
            
            // SELECT MAX(Precio) FROM ListaProductos

            var precioMax = DataLists.ListaProductos
                .Max(r => r.Precio);

            Console.WriteLine($"Precio Máximo: {precioMax.ToString("N2")}" + Environment.NewLine);

            // SELECT TOP(1) * FROM ListaProductos WHERE Precio = 12.54

            var producto = DataLists.ListaProductos
                .Where(r => r.Precio == precioMax)
                .FirstOrDefault();

            Console.WriteLine($"Precio máximo: {producto.Descripcion} - {producto.Precio.ToString("N2")}" + Environment.NewLine);

            // SELECT * FROM ListaProductos WHERE Precio = (SELECT MAX(Precio) FROM ListaProductos)

            var productos = DataLists.ListaProductos
                .Where(r => r.Precio == DataLists.ListaProductos.Max(r => r.Precio))
                .ToList();

            // SELECT * FROM ListaProductos WHERE Precio = 12.54

            var productos2 = DataLists.ListaProductos
                .Where(r => r.Precio == precioMax)
                .ToList();

            Console.WriteLine($"{productos.Count} productos con precio máximo" + Environment.NewLine);


            /////////////////////////////////////////////////////////////////////////////////
            // Precio medio de todos los productos
            /////////////////////////////////////////////////////////////////////////////////
           
            // SELECT AVG(Precio) FROM ListaProductos

            var precioMedio = DataLists.ListaProductos
                .Select(r => r.Precio)
                .Average();

            var precioMedio2 = DataLists.ListaProductos
                .Average(r => r.Precio);

            Console.WriteLine($"Precio Medio: {precioMedio.ToString("N2")}" + Environment.NewLine);


            /////////////////////////////////////////////////////////////////////////////////
            //Productos con un precio inferior a la media
            /////////////////////////////////////////////////////////////////////////////////
            
            // SELECT * FROM ListaProductos WHERE Precio = (SELECT AVG(Precio) FROM ListaProductos)

            var productos3 = DataLists.ListaProductos
               .Where(r => r.Precio < DataLists.ListaProductos.Average(r => r.Precio))
               .ToList();

            // SELECT * FROM ListaProductos WHERE Precio = 2.54

            var productos4 = DataLists.ListaProductos
               .Where(r => r.Precio == precioMedio)
               .ToList();

            Console.WriteLine($"PRODUCTOS: inferiores a {DataLists.ListaProductos.Average(r => r.Precio).ToString("N2")}");
            Console.WriteLine("==================================================================");
            foreach (var item in productos3) Console.WriteLine($"{item.Descripcion} {item.Precio.ToString("N2")}");
            Console.WriteLine("");
        }

        /// <summary>
        /// Ejercicios realizados el 31/03/2022
        /// </summary>
        static void Ejercicios2()
        {
            var context = new ModelNorthwind();

            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Clientes que residen en USA
            /////////////////////////////////////////////////////////////////////////////////
            
            var r1 = context.Customers
                .Where(r => r.Country == "USA")
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Proveedores (Suppliers) de Berlin
            /////////////////////////////////////////////////////////////////////////////////
            
            var r2 = context.Suppliers
                .Where(r => r.City == "Berlin")
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Empleados con identificadores 3, 5 y 8
            /////////////////////////////////////////////////////////////////////////////////

            // SELECT * FROM dbo.Employees WHERE EmployeeID IN (3, 5, 8)

            int[] employeesIds = new int[] { 3, 5, 8 };

            var r3a = context.Employees
                .Where(r => employeesIds.Contains(r.EmployeeID))
                .ToList();

            // SELECT * FROM dbo.Employees WHERE EmployeeID IN (3, 5, 8)

            var r3b = context.Employees
                .Where(r => new int[] { 3, 5, 8 }.Contains(r.EmployeeID))
                .ToList();

            // SELECT * FROM dbo.Employees WHERE EmployeeID = 3 OR EmployeeID = 5 OR EmployeeID = 8 

            var r3c = context.Employees
                .Where(r => r.EmployeeID == 3 || r.EmployeeID == 5 || r.EmployeeID == 8)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Productos con stock mayor de cero
            /////////////////////////////////////////////////////////////////////////////////

            var r4 = context.Products
                .Where(r => r.UnitsInStock > 0)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Listado de Productos con stock mayor de cero de los proveedores con identificadores 1, 3 y 5
            /////////////////////////////////////////////////////////////////////////////////////////////////
            
            int?[] suppliersIds = new int?[] { 1, 3, 5 };

            var r5 = context.Products
                .Where(r => suppliersIds.Contains(r.SupplierID) && r.UnitsInStock > 0)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Productos con precio mayor de 20 y menor 90
            /////////////////////////////////////////////////////////////////////////////////

            var r6 = context.Products
                .Where(r => r.UnitPrice > 20 && r.UnitPrice < 90)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Pedidos entre 01/01/1997 y 15/07/1997
            /////////////////////////////////////////////////////////////////////////////////
            
            var r7 = context.Orders
                .Where(r => r.OrderDate >= new DateTime(1997, 1, 1) && r.OrderDate <= new DateTime(1997, 7, 15))
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Listado de Pedidos registrados por los empleados con identificador 1, 3, 4 y 8 en 1997
            /////////////////////////////////////////////////////////////////////////////////////////////////

            int?[] employeesIds2 = new int?[] { 1, 3, 4, 8 };

            var r8 = context.Orders
                .Where(r => r.OrderDate.Value.Year == 1997 && employeesIds2.Contains(r.EmployeeID))
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Pedidos de abril de 1996
            /////////////////////////////////////////////////////////////////////////////////

            var r9 = context.Orders
                .Where(r => r.OrderDate.Value.Year == 1996 && r.OrderDate.Value.Month == 4)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Pedidos del realizado los dia uno de cada mes del año 1998
            /////////////////////////////////////////////////////////////////////////////////

            var r10 = context.Orders
                .Where(r => r.OrderDate.Value.Year == 1998 && r.OrderDate.Value.Day == 1)
                .ToList();

            var r10b = context.Orders
                .Where(r =>
                    (r.OrderDate.HasValue ? r.OrderDate.Value.Year : -1) == 1998 &&
                    (r.OrderDate.HasValue ? r.OrderDate.Value.Day : -1) == 1)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Clientes que no tiene fax
            /////////////////////////////////////////////////////////////////////////////////

            var r11 = context.Customers
                .Where(r => r.Fax == null)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de los 10 productos más baratos
            /////////////////////////////////////////////////////////////////////////////////

            var r12 = context.Products
                .OrderBy(r => r.UnitPrice)
                .Take(10)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de los 10 productos más caros con stock
            /////////////////////////////////////////////////////////////////////////////////
            
            var r13 = context.Products
                .Where(r => r.UnitsInStock > 0)
                .OrderByDescending(r => r.UnitPrice)
                .Take(10)
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Cliente de UK y nombre de empresa que comienza por B
            /////////////////////////////////////////////////////////////////////////////////
            
            var r14 = context.Customers
                .Where(r => r.CompanyName.StartsWith("B") && r.Country == "Uk")
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Productos de identificador de categoria 3 y 5
            /////////////////////////////////////////////////////////////////////////////////

            var r15 = context.Products
                .Where(r => new int?[] { 3, 5 }.Contains(r.CategoryID))
                .ToList();


            /////////////////////////////////////////////////////////////////////////////////
            // Importe total del stock
            /////////////////////////////////////////////////////////////////////////////////

            // SELECT SUM(UnitInStock * UnitPrice) FROM Products

            var r16 = context.Products
                .Sum(r => r.UnitsInStock * r.UnitPrice);


            /////////////////////////////////////////////////////////////////////////////////
            // Listado de Pedidos de los clientes de Argentina
            /////////////////////////////////////////////////////////////////////////////////
            
            var argentinaIds = context.Customers
                    .Where(s => s.Country == "Argentina")
                    .Select(s => s.CustomerID)
                    .ToList();

            var r17 = context.Orders
                .Where(r => argentinaIds.Contains(r.CustomerID))
                .ToList();


            var r17a = context.Orders
                .Where(r => 
                    context.Customers
                        .Where(s => s.Country == "Argentina")
                        .Select(s => s.CustomerID)
                        .ToList()
                .Contains(r.CustomerID))
                .ToList();
        }
    }
}
