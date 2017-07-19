using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_Var1_Level1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 1
            // Create a Student object
            Student stud = new Student(new Person("Farli", "Dorg", new DateTime()), Education.SecondEducation, 289);
            // Add some Exams to the Student object
            stud.AddExams(new Exam("Composition", 4, new DateTime(1899, 3, 23)));
            // Create a copy using serialization
            Student copy_stud = stud.DeepCopyThroughSerialize();
            // Print both students to console
            Console.WriteLine("--------- Task 1 ---------\n");
            Console.WriteLine("Initial Student:\n");
            Console.WriteLine(stud.ToString());
            Console.WriteLine("Copy of the initial Student:\n");
            Console.WriteLine(copy_stud.ToString());

            // Task 2
            Console.WriteLine("--------- Task 2 -----------\n");
            Console.WriteLine("Please, enter file name: (Example: \"file_name\".bin)");
            string file_name = Console.ReadLine();
            Student load_stud = new Student();
            if (File.Exists(file_name))
            {
                load_stud.Load(file_name);
                Console.WriteLine(load_stud.ToString());
            }
            else
            {
                Console.WriteLine("There is no file with name \"{0}\".", file_name);
                FileStream fs = File.Create(file_name);
                fs.Close();
            }


            // Task 3
            Console.WriteLine("--------- Task 3 ---------\n");
            load_stud.AddFromConsole();
            load_stud.Save(file_name);
            Console.WriteLine(load_stud.ToString());

            // Task 4
            //Student.Load(file_name, load_stud);
            Student.Load(file_name, out load_stud);
            load_stud.AddFromConsole();
            Student.Save(file_name, load_stud);
            Console.WriteLine(load_stud.ToString());

            Console.ReadLine();

            //Student.Save("file.bin", stud);

            //stud.Exam_List.Add(new Exam("Ecology", 3, new DateTime(1994, 12, 3)));
            //stud.Credit_List.Add(new Credit());

            //Student l_s = new Student();
            //Student.Load("file.bin", out l_s);

            //Console.WriteLine(l_s.ToString());

            //Student stud_1 = new Student();
            //stud_1.AddFromConsole();
            //Console.WriteLine(stud_1.ToString());

            //Console.ReadLine();
        }
    }
}
