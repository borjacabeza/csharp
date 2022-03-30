using System;
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
                Console.WriteLine("*  1. Consultas Básicas".PadRight(55) + "*");
                Console.WriteLine("*  2. Ejercicios 30/03/2022".PadRight(55) + "*");
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
                        Ejercicios();
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
        /// Consultas básicas con LINQ
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
    }
}
