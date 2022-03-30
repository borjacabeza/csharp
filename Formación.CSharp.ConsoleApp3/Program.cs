using System;

namespace Formacion.CSharp.ConsoleApp3
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Clear();
                Calculadora calculadora = new Calculadora(65, 0);

                calculadora.Suma();
                Console.WriteLine($"Suma: {calculadora.Resultado}");

                calculadora.Resta();
                Console.WriteLine($"Resta: {calculadora.Resultado}");

                calculadora.Multiplicacion();
                Console.WriteLine($"Multiplicación: {calculadora.Resultado}");

                //Se produce una excepción al intentar divivir un 65 entre 0
                calculadora.Division();
                Console.WriteLine($"División: {calculadora.Resultado}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Main: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Objeto Calculadora
    /// </summary>
    public class Calculadora
    {
        public int Numero1 { get; set; }
        public int Numero2 { get; set; }
        public int Resultado { get; set; }

        /// <summary>
        /// Realiza la suma de Numero1 y Numero2, dejando el resultado en Resultado
        /// </summary>
        public void Suma()
        {
            Resultado = Numero1 + Numero2;
        }

        /// <summary>
        /// Realiza la resta de Numero1 y Numero2, dejando el resultado en Resultado
        /// </summary>
        public void Resta()
        {
            Resultado = Numero1 - Numero2;
        }

        /// <summary>
        /// Realiza la multiplicación de Numero1 y Numero2, dejando el resultado en Resultado
        /// </summary>
        public void Multiplicacion()
        {
            Resultado = Numero1 * Numero2;
        }

        /// <summary>
        /// Realiza la división de Numero1 y Numero2, dejando el resultado en Resultado
        /// </summary>
        /// <exception cref="Exception">Representa el objeto Exception</exception>
        public void Division()
        {
            ///////////////////////////////////////////////////////////////
            //
            //  Control de Excepciones mediante los bloques
            //  try/catch/finally
            //
            ///////////////////////////////////////////////////////////////

            //En el bloque TRY escribimos el código que pueda producir una excepción
            //Los bloques CATCH se ejecutan cuando se produce una excepción al ejecutar las sentencias del bloque TRY
            //El bloque FINALLY se ejecuta después de finalizar el bloque TRY o el bloque CATCH
            try
            {
                Resultado = Numero1 / Numero2;
            }
            catch (DivideByZeroException e)
            {
                Resultado = 0;
                Console.WriteLine($"Info: Error al dividir entre cero. {e.Message}");

                //THROW envia la excepción al nivel superior, el blo Try\Catch del Main
                throw;
                throw e;

                //THROW envia una nueva excepción al nivel superior, el blo Try\Catch del Main
                throw new Exception($"Error al dividir entre cero, Número1={Numero1}  Número2={Numero2}.");
            }
            catch (Exception e)
            {
                Resultado = 0;
                Console.WriteLine($"Info: Error al dividir. {e.Message}");
            }
            finally
            {
                Console.WriteLine("Info: Método división finalizado.");
            }
        }

        /// <summary>
        /// Constructo
        /// </summary>
        public Calculadora()
        {
            Numero1 = 0;
            Numero2 = 0;
            Resultado = 0;
        }

        /// <summary>
        /// Constructo
        /// </summary>
        /// <param name="numero1">Valor para inicializar Numero1</param>
        /// <param name="numero2">Valor para inicializar Numero2</param>
        public Calculadora(int numero1, int numero2)
        {
            Numero1 = numero1;
            Numero2 = numero2;
            Resultado = 0;
        }
    }

}
