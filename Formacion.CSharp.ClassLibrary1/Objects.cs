using System;

namespace Formacion.CSharp.ClassLibrary1
{
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ClassId { get; set; }

        public override string ToString()
        {
            return $"{Firstname} {Lastname}";
        }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
