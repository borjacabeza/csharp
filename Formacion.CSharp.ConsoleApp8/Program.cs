using System;
using Formacion.CSharp.Objects;


namespace Formacion.CSharp.ConsoleApp8
{
    public delegate void Demo();
    public delegate void Formula(int a, int b);


    public class Program
    {
        /// <summary>
        /// Inicio del programa
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //////////////////////////////////////////////////////////////////////////////////////
            // Uso de delegados, creamos variables que almacenan métodos
            //////////////////////////////////////////////////////////////////////////////////////

            Demo demo = Demos.MetodoA;
            demo();

            demo = () => { Console.WriteLine("Método Anónimo"); };
            demo();


            //////////////////////////////////////////////////////////////////////////////////////
            // Uso de delegados, pasamos código a método mediante variables de tipo delegado
            // La funcionalidad del método cambia en función de código contenido en el delegado
            //////////////////////////////////////////////////////////////////////////////////////
            
            Calcular(150, 25, Demos.Addition);
            Calcular(150, 25, Demos.Subtraction);
            Calcular(150, 25, Demos.Multiply);
            Calcular(150, 25, Demos.Divide);

            Calcular(150, 25, (x, y) => { Console.WriteLine($"Resultado: {(x/2) * (y/2)}"); });


            Console.ReadKey();
        }
        
        /// <summary>
        /// El resultado de calcular depende de la formula recibida mediante un delegado
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="f1"></param>
        static void Calcular(int n1, int n2, Formula f1)
        {
            f1(n1, n2);
        }
    }
}

namespace Formacion.CSharp.Objects
{
    // Delegado
    public delegate void DemoEventHandler(Object sender, StudentEventArgs e);

    /// <summary>
    /// Objeto utilizado para enviar argumentos
    /// </summary>
    public class StudentEventArgs
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Objeto estudiante. Tiene un evento creado con el delegado DemoEventHandler que utiliza como argumento el objeto StudentEventArgs
    /// </summary>
    public class Student
    {
        public event DemoEventHandler FullName;

        private string firstname;
        private string lastname;

        public string Firstname
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;

                if (firstname != null && firstname.Trim() != "" && lastname != null && lastname != "")
                    FullName?.Invoke(this, new StudentEventArgs() { Date = DateTime.Now, Name = $"{firstname} {lastname}" });
            }
        }
        public string Apellidos
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;

                if (firstname != null && firstname.Trim() != "" && lastname != null && lastname != "")
                    FullName?.Invoke(this, new StudentEventArgs() { Date = DateTime.Now, Name = $"{firstname} {lastname}" });
            }
        }
    }

    /// <summary>
    /// Objeto estudiante. Tiene un evento creado con el delegado genérico EventHandler
    /// </summary>
    public class Student2
    {
        public event EventHandler<string> FullName;

        private string firstname;
        private string lastname;

        public string Firstname
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;

                if (firstname != null && firstname.Trim() != "" && lastname != null && lastname != "")
                    FullName?.Invoke(this, $"{firstname} {lastname}");
            }
        }
        public string Apellidos
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;

                if (firstname != null && firstname.Trim() != "" && lastname != null && lastname != "")
                    FullName?.Invoke(this, $"{firstname} {lastname}");
            }
        }
    }

    /// <summary>
    /// Objeto para demostraciones
    /// </summary>
    public static class Demos
    {
        public static void MetodoA()
        {
            Console.WriteLine("Metodo A");
        }

        public static void Addition(int n1, int n2)
        {
            Console.WriteLine($"{n1} + {n2} = {n1 + n2}");
        }

        public static void Subtraction(int n1, int n2)
        {
            Console.WriteLine($"{n1} - {n2} = {n1 - n2}");
        }
        public static void Multiply(int n1, int n2)
        {
            Console.WriteLine($"{n1} x {n2} = {n1 * n2}");
        }
        public static void Divide(int n1, int n2)
        {
            Console.WriteLine($"{n1} ÷ {n2} = {n1 / n2}");
        }
    }
}