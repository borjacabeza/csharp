using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Reflection;

namespace Formacion.CSharp.ConsoleApp9
{
    internal class Program
    {
        /// <summary>
        /// Inicio del programa
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /*********************************************************************************************************************************
             *                                                                                                                               *
             *  Reflection es una función eficaz que le permite inspeccionar y realizar una manipulación dinámica de emsamblados, tipos y    *
             *  miembros en tiempo de ejecución.                                                                                             *
             *                                                                                                                               *
             *  .NET Framework proporciona el espacio de nombres System.Reflection, que contiene clases que le permiten aprovechar la        * 
             *  reflexión en sus aplicaciones. La siguiente lista describe algunas de estas clases:                                          *
             *                                                                                                                               *
             *   - Assembly.. Esta clase le permite cargar e inspeccionar los metadatos y los tipos en un ensamblaje físico.                 *
             *   - TypeInfo. Esta clase le permite inspeccionar las características de un tipo.                                              *
             *   - ParameterInfo. Esta clase le permite inspeccionar las características de cualquier parámetro que acepte un miembro.       *
             *   - ConstructorInfo. Esta clase le permite inspeccionar el constructor del tipo.                                              *
             *   - FieldInfo. Esta clase le permite inspeccionar las características de los campos que se definen dentro de un tipo.         *
             *   - MemberInfo. Esta clase le permite inspeccionar los miembros que expone un tipo. Información de la propiedad.              *
             *     Esta clase le permite inspeccionar las características de las propiedades que se definen dentro de un tipo.               *
             *   - MethodInfo. Esta clase le permite inspeccionar las características de los métodos que se definen dentro de un tipo.       *
             *                                                                                                                               *
             *   El espacio de nombres System incluye la clase Type, que también expone una selección de miembros que encontrará útiles      *
             *   cuando use la reflexión. Por ejemplo, el método de instancia GetFields le permite obtener una lista de objetos FieldInfo,   *
             *   que representan los campos que están definidos dentro de un tipo.                                                           *   
             *                                                                                                                               *
             *********************************************************************************************************************************/

            var assembly = Assembly.LoadFrom("Formacion.CSharp.ClassLibrary1.dll");

            Console.WriteLine($"======================================================================================================");
            Console.WriteLine($" Nombre: {assembly.FullName}");
            Console.WriteLine($"======================================================================================================");
            Console.WriteLine($" Clases o Tipos: ");

            foreach (var type in assembly.GetTypes())
            {
                Console.WriteLine($"   > {type.FullName}");
                Console.WriteLine($"      Constructores:");
                foreach (var ctor in type.GetConstructors()) Console.WriteLine($"       > {ctor.Name}:");
                Console.WriteLine($"      Propiedades:");
                foreach (var prop in type.GetProperties()) Console.WriteLine($"       > {prop.Name}:");
                Console.WriteLine($"      Métodos:");
                foreach (var meth in type.GetMethods()) Console.WriteLine($"       > {meth.Name}:");
                Console.WriteLine(Environment.NewLine);
            }

            // Crear una instancia del objeto
            var studentType = assembly.GetType("Formacion.CSharp.ClassLibrary1.Student");
            object studentInstance = Activator.CreateInstance(studentType);

            // get info about property: public string Firstname
            PropertyInfo FirstnamePropertyInfo = studentType.GetProperty("Firstname");

            // get value of property: public string Firstname
            string value = (string)FirstnamePropertyInfo.GetValue(studentInstance, null);
            Console.WriteLine($"Firstname: {value}");

            // set value of property: string Firstname
            FirstnamePropertyInfo.SetValue(studentInstance, "Borja", null);

            Console.WriteLine($"Firstname: {FirstnamePropertyInfo.GetValue(studentInstance, null)}");
            Console.WriteLine(Environment.NewLine);



            /*********************************************************************************************************************************
             *                                                                                                                               *
             *  .NET Framework incluye un mecanismo denominado Code Document Object Model (CodeDOM) que permite que los desarrolladores de   *
             *  programas que emiten código, generen código fuente en varios lenguajes de programación en tiempo de ejecución, en función de *
             *  un único modelo que representa el código que se va a representar.                                                            *
             *                                                                                                                               *
             *  .NET Framework incluye los generadores de código y compiladores de código para CSharpCodeProvider, JScriptCodeProvider       *
             *  y VBCodeProvider.                                                                                                            *
             *                                                                                                                               *
             *********************************************************************************************************************************/
            
            CodeNamespace nameSpace = new CodeNamespace("Demos");

            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Text"));

            CodeTypeDeclaration cls = new CodeTypeDeclaration();
            cls.Name = "Calculadora";
            cls.IsClass = true;
            cls.Attributes = MemberAttributes.Public;
            nameSpace.Types.Add(cls);

            // Extender el Object
            CodeMemberMethod newMethod = new CodeMemberMethod();

            // Generate Method signatures.  
            newMethod.Name = "Suma";
            newMethod.ReturnType = new CodeTypeReference(typeof(System.Int32));
            newMethod.Attributes = MemberAttributes.Public;

            // Generar parámetros del método
            CodeParameterDeclarationExpression methodParam1 = new CodeParameterDeclarationExpression(typeof(Int32), "X");
            CodeParameterDeclarationExpression methodParam2 = new CodeParameterDeclarationExpression(typeof(Int32), "Y");
            newMethod.Parameters.AddRange(new CodeParameterDeclarationExpression[] { methodParam1, methodParam2 });

            // Generear definición del método (código)
            CodeSnippetExpression codeSnippet = new CodeSnippetExpression("return X + Y");
            newMethod.Statements.Add(codeSnippet);

            // Añadir el método a la clase
            cls.Members.Add(newMethod);

            // Compilación y generación del fichero de csharp
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            compileUnit.Namespaces.Add(nameSpace);
            CSharpCodeProvider csharpcodeprovider = new CSharpCodeProvider();

            CSharpCodeProvider provider = new CSharpCodeProvider();

            using (StreamWriter sw = new StreamWriter(@"TestFile.cs", false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
                tw.Close();
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            // Representa un objeto cuyos miembros se pueden agregar y quitar de forma dinámica en tiempo de ejecución.
            // Implementa INotifyPropertyChanged que notifica cambios en la propiedades como sucede en los proyectos WPF 
            dynamic obj = new ExpandoObject();

            ((INotifyPropertyChanged)obj).PropertyChanged += new PropertyChangedEventHandler((sender, e) => { Console.WriteLine("Property Changed"); });

            // Encapsula un método que tiene un parámetro y devuelve un valor del tipo especificado por el parámetro
            // Existen combinaciones de hasta 16 parámetros y un retorno
            obj.Name = "Borja";

            Func<int, int, int> func = (x, y) => y + x;
            obj.Calculate = func;

            Console.WriteLine($"Valor: {obj.Calculate(123)}");

            obj.Message = (Func<String>)(() => { return $"Os saluda {obj.Name}."; });
            Console.WriteLine($"Valor: {obj.Message()}");

            obj.Calculate = null;
            //Console.WriteLine($"Valor: {obj.Calculate(123)}");
        }
    }
}
