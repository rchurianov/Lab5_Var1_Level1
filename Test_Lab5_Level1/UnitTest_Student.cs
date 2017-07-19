using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab5_Var1_Level1;

namespace Test_Lab5_Level1
{
    [TestClass]
    public class UnitTest_Student
    {
        [TestMethod]
        public void Test_DeepCopy_S()
        {
            Student st = new Student();
            st.AddExams(new Exam());
            st.Credit_List.Add(new Credit());
            Student copy_st = st.DeepCopyThroughSerialize();
            Console.Write(copy_st.ToString());
        }

        [TestMethod]
        public void Test_Static_Save_Load()
        {
            Student stud = new Student(new Person("Farli", "Dorg", new DateTime()), Education.SecondEducation, 289);
            Student.Save("file.bin", stud);
            
            stud.Exam_List.Add(new Exam("Ecology", 3, new DateTime(1994, 12, 3)));
            stud.Credit_List.Add(new Credit());
            
            Student l_s = new Student();
            Student.Load("file.bin", out l_s);
            
            Assert.AreEqual(stud.Name, l_s.Name);
            Assert.AreNotEqual(l_s, stud);
            Console.WriteLine(l_s.ToString());
        }

        //[TestMethod]
        //public void Test_AddFromConsole()
        //{
        //    Student stud = new Student();
        //    //stud.AddFromConsole();
        //    Student stud_1 = new Student();
        //    Assert.AreNotEqual(stud, stud_1);
        //}
    }
}
