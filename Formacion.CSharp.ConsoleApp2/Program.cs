using System;

namespace Formacion.CSharp.ConsoleApp2
{
    class Program
    {
        /// <summary>
        /// Los tipos de valor y los tipos de referencia son las dos categorías principales de tipos de C#. 
        /// Una variable de un tipo de valor contiene una instancia del tipo (una copia). 
        /// Una variable de un tipo de referencia, que contiene una referencia a una instancia(un puntero a la dirección de memoria). 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Variable de tipo referencia. Objeto definido con Class.
            var alumno = new Alumno() { Nombre = "Ana", Apellidos = "Sanz" };

            var alumnoA = new Alumno();
            var alumnoB = new Alumno("Borja", "Cabeza");

            int a = 10;

            Console.WriteLine($"{alumno.Nombre} {alumno.Apellidos} - {a}");
            Transformar(alumno, ref a);
            Console.WriteLine($"{alumno.Nombre} {alumno.Apellidos} - {a}" + Environment.NewLine);

            //Variable de tipo valor. Objeto definico con Struct.
            var alumno2 = new Alumno2() { nombre = "Ana", apellidos = "Sanz" };

            Console.WriteLine($"{alumno2.nombre} {alumno2.apellidos}");
            Transformar(alumno2);
            Console.WriteLine($"{alumno2.nombre} {alumno2.apellidos}" + Environment.NewLine);
        }

        /// <summary>
        /// Método que recibe dos parámetros
        /// </summary>
        /// <param name="a">Representa un objeto Alumno creado con Class, es de referencia</param>
        /// <param name="numero">Representa un número entero que es de tipo valor, se comporta como de referencia cuando añadimos ref</param>
        static void Transformar(Alumno a, ref int numero)
        {
            a.Nombre = "Borja";
            a.Apellidos = "Cabeza";
            numero = 1100;
        }

        /// <summary>
        /// Método que recibe un parámetros
        /// </summary>
        /// <param name="a">Representa un objeto Alumno creado con Struct, es de valor</param>
        static void Transformar(Alumno2 a)
        {
            a.nombre = "Borja";
            a.apellidos = "Cabeza";
        }

        static void Transformar()
        {
        }

        static void Transformar(Alumno a, Alumno2 b)
        {
        }
    }

    /// <summary>
    /// Objeto creado con CLASS, de tipo referencia
    /// </summary>
    public class Alumno
    {
        //Miembro: Variables
        private string nombre;
        private int edad;

        //Miembro: Propiedad asociada a una variable
        public string Nombre
        {
            //Retornamos la información despues de tranformarla.
            get
            {
                return nombre.Trim().ToLower();
            }

            //Tratamos la información antes de almacenarla en la variable.
            set
            {
                if (value.Length < 2) nombre = "";
                else nombre = value;
            }
        }

        //Miembro: Propiedad sin variable
        public string Apellidos { get; set; }

        //Miembro: Propiedad asociada a una variable
        public int Edad
        {
            get
            {
                return edad;
            }
            set
            {
                if (value < 1 || value > 130) edad = 0;
                else edad = value;
            }
        }

        //Miembro: Propiedad de solo lectura con Get
        public string NombreCompleto
        {
            get
            {
                return $"{this.nombre} {this.Apellidos}";
            }
        }

        //Miembro: Propiedad de solo escritura con Set
        public int CambiaEdad
        {
            set
            {
                edad = value;
            }
        }

        //Miembro: Métodos con tipo VOID, no retorna información
        public void MetodoUno()
        { }

        /// <summary>
        /// Miembros: Métodos con tipo que retornan información.
        /// Todos los parámetros opcionales tiene un valor por defecto y se escriben después de los obligatorios
        /// </summary>
        /// <param name="param1">Parámetro obligatorio</param>
        /// <param name="param2">Parámetro obligatorio</param>
        /// <param name="param3">Parámetro opcional</param>
        /// <param name="param4">Parámetro opcional</param>
        /// <returns></returns>
        public bool MetodoDos(int param1, string param2, float param3 = 0, string param4 = "valor por defecto")
        {
            return true;
        }

        //Miembros: Constructor, es público, no tiene tipo y se llama igual que la clase
        public Alumno()
        {

        }

        //Sobrecarga del método constructor.
        public Alumno(string nombre, string apellidos)
        {
            this.nombre = nombre;
            this.Apellidos = apellidos;
        }

        //Miembros: Destructores, no tiene tipo, comienza por ~ (Alt+0126 o AltGrap+4) más el nombre de la clase
        //No se le puede llamar, se ejecuta automáticamente
        //No tiene parámetros
        ~Alumno()
        {
            System.Diagnostics.Debug.WriteLine("Inicio del destructor de Alumno.");
            this.nombre = null;
            this.Apellidos = null;
        }
    }

    /// <summary>
    /// Objeto creado con STRUCT, de tipo valor
    /// </summary>
    public struct Alumno2
    {
        public string nombre;
        public string apellidos;
    }
}
