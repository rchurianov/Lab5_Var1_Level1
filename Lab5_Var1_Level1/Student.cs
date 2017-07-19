using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab5_Var1_Level1
{
    [Serializable]
    public class Student : Person, IDateAndCopy, IEnumerable, INotifyPropertyChanged
    {
        private Education degree;
        private int group_number;
        private List<Credit> credit_list;
        private List<Exam> exam_list;

        public Student(Person p, Education e, int input_group_number) : base(p.Name, p.Last_Name, p.Birth_Date)
        {
            degree = e;
            group_number = input_group_number;

            credit_list = new List<Credit>();
            credit_list.Add(new Credit("Analysis", true));
            credit_list.Add(new Credit("Differential equations", true));

            exam_list = new List<Exam>();
            exam_list.Add(new Exam("History", 5, new DateTime(2000, 10, 10)));
            exam_list.Add(new Exam("Operation Systems", 5, new DateTime(2000, 10, 20)));
        }

        public Student() : base()
        {
            degree = Education.Bachelor;

            //Random rand = new Random();
            //group_number = rand.Next(101, 121);
            group_number = 111;

            credit_list = new List<Credit>();
            credit_list.Add(new Credit());
            credit_list.Add(new Credit());

            exam_list = new List<Exam>();
            exam_list.Add(new Exam());
            exam_list.Add(new Exam());
            //Console.WriteLine("Created new Student with default constructor.");
        }

        #region PropertyChanged events
        /* PropertyChanged event that occurs when one of Student properties change. 
         * Required by INotifyPropertyChanged interface.
         */
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        #endregion

        /*
         * This indexator return "true" if Student.degree == education_index,
         * and "false" otherwise.
         * Both Student.degree and education_index are of type Education.
         */
        public bool this[Education education_index] 
        {
            get
            {
                Console.WriteLine("Check if student has {0} education degree:", education_index);
                return this.degree == education_index;
            }
        }

        public Person Passport_Data
        { 
            get 
            {
                Person p = new Person(base.Name, base.Last_Name, base.Birth_Date);
                return p;
            }
            set 
            { 
                base.name = value.Name;
                base.last_name = value.Last_Name;
                base.birth_date = value.Birth_Date;
            }
        }

        /* Gets and sets Student.degree field.
         * Raises PropertyChanged event.
         */
        public Education Degree
        {
            get { return degree; }
            set
            {
                degree = value;
                PropertyChangedEventArgs args = new PropertyChangedEventArgs("Degree");
                OnPropertyChanged(args);
            }
        }

        /* Gets and sets Student.group_number field.
         * Raises PropertyChanged event.
         */
        public int Group_Number
        {
            get { return group_number; }
            set 
            { 
                if (value <= 100 || value > 599)
                {
                    throw new ArgumentOutOfRangeException("Assigned value", value, "Value should be in the interval [100, 599].");
                }
                else
                {
                    group_number = value;
                    PropertyChangedEventArgs args = new PropertyChangedEventArgs("Group_Number");
                    OnPropertyChanged(args);
                }
            }
        }

        public List<Exam> Exam_List
        {
            get { return exam_list; }
            set { exam_list = value; }
        }

        public List<Credit> Credit_List
        {
            get { return credit_list; }
            set { credit_list = value; }
        }

        /* In case exam_list == null
         * Student.AGP returns 0.0.
         * Otherwise, AGP return the average grade
         * of Exams in exam_list.
         */
        public double AGP
        {
            get
            {
                double average = 0.0;
                if (exam_list != null)
                {
                    for (int i = 0; i < exam_list.Count; i++)
                    {
                        average = average + exam_list[i].Grade;
                    }
                    average /= exam_list.Count;
                }
                return average;
            }
        }

        public void AddExams(params Exam[] input_exam_list)
        {
            if (input_exam_list != null)
            {
                if (exam_list == null)
                {
                    exam_list = new List<Exam>();
                    for (int i = 0; i < input_exam_list.Length; i++)
                    {
                        exam_list.Add(input_exam_list[i]);
                    }
                }
                else if (exam_list != null)
                {
                    for (int i = 0; i < input_exam_list.Length; i++)
                    {
                        exam_list.Add(input_exam_list[i]);
                    }
                }
                //Console.WriteLine("Added {0} Exam(s) to Student's exam_list.", input_exam_list.Length);
            }
        }

        /* A simple iterator to iterate through both
         * Credits and Exams in lists consequtively. Both credits and exams should be
         * passed. For Credit "passed" means Credit.Passed == true;
         * for exam "passed" means Exam.Grade > 2.
         */
        public IEnumerable Passed_Session_Iterator()
        {
            if (credit_list != null)
            {
                for (int i = 0; i < credit_list.Count; i++)
                {
                    if (credit_list[i].Credit_Passed)
                        yield return credit_list[i];
                }
            }
            if (exam_list != null)
            {
                for (int i = 0; i < exam_list.Count; i++)
                {
                    if (exam_list[i].Grade > 2)
                        yield return exam_list[i];
                }
            }
        }

        /* Iterates through passed Credits which have corresponding
         * Exam passed with grade > 2
         */
        public IEnumerable<Credit> Passed_Credit_Iterator()
        {
            IComparer comparer = new Credit_Exam_Comparer();
            exam_list.Sort();
            // will return Credit objects only if both credit and exam lists contain smth
            if (credit_list != null && exam_list != null)
            {
                for (int i = 0; i < credit_list.Count; i++)
                {
                    int found_exam_index = exam_list.FindIndex(x => x.Exam_Name == credit_list[i].Credit_Name); // new version
                    //int found_exam_index = exam_list.BinarySearch((Credit)credit_list[i], comparer); <- old version
                    if (found_exam_index != -1)
                    {
                        Exam found_exam = exam_list[found_exam_index];
                        if (credit_list[i].Credit_Passed && found_exam.Grade > 2)
                        {
                            yield return credit_list[i];
                        }
                    }
                }
            }
        }
        
        /* A simple iterator to iterate through both
         * credit and exam collections consequtively
         */
        public IEnumerable Session_Iterator()
        {
            if (credit_list != null)
            {
                for (int i = 0; i < credit_list.Count; i++)
                {
                    yield return credit_list[i];
                }
            }
            if (exam_list != null)
            {
                for (int i = 0; i < exam_list.Count; i++)
                {
                    yield return exam_list[i];
                }
            }
        }

        /* Iterator to iterate through exam_list.
         * Return Exams from the list one by one, but only those
         * Exams which have Exam.Grade > min_grade.
         * min_grade is a required parameter.
         */
        public IEnumerable<Exam> Exam_Iterator(int min_grade)
        {
            if (exam_list != null)
            {
                for (int i = 0; i < exam_list.Count; i++)
                {
                    if (exam_list[i].Grade > min_grade)
                    {
                        yield return exam_list[i];
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Name, Last Name, Birth Date, Degree, Group No:\n" +
                    base.ToString() + ", " + degree.ToString() + ", " +
                    group_number.ToString() + ",\n" +
                    "[Credit_Name, Passed]\n" +
                    Credit_List_ToString() +
                    "[Exam_Name, Grade, Date]\n" +
                    Exam_List_ToString();
        }

        private string Exam_List_ToString()
        {
            string s = "";
            if (exam_list != null)
            {
                for (int i = 0; i < exam_list.Count; i++)
                {
                    s = s + exam_list[i].ToString() + "\n";
                }
            }
            return s;
        }

        private string Credit_List_ToString()
        {
            string s = "";
            if (credit_list != null)
            {
                for (int i = 0; i < credit_list.Count; i++)
                {
                    s = s + credit_list[i].ToString() + "\n";
                }
            }
            return s;
        }

        /* Student.AGP property checks for exam_list == null
         * so we do not have to check for it in ToShortString().
         */
        public override string ToShortString()
        {
            return "Name, Last Name, Birth Date, Degree, Group No, AGP:\n" +
                   base.ToString() + ", " +
                   degree.ToString() + ", " +
                   group_number.ToString() + ", " +
                   AGP.ToString();
        }

        /*
         * Makes a copy of the calling object with MemoryStream as a mediator.
         * First, calling object is serialized to a MemoryStream.
         * Then deserialized from it.
         * */
        public Student DeepCopyThroughSerialize()
        {
            MemoryStream m_stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(m_stream, this);

            Student copy_student = new Student();
            // set MemoryStream pointer to beginning (0)
            m_stream.Seek(0, SeekOrigin.Begin);
            copy_student = (Student) formatter.Deserialize(m_stream);
            m_stream.Close();
            return copy_student;
        }

        protected override object DeepCopy()
        {
            Student student_copy = new Student();
            student_copy.name = this.name;
            student_copy.last_name = this.last_name;
            student_copy.birth_date = this.birth_date;
            student_copy.degree = this.degree;
            student_copy.group_number = this.group_number;
            
            student_copy.credit_list = new List<Credit>();
            for (int i = 0; i < this.credit_list.Count; i++)
            {
                student_copy.credit_list.Add(this.credit_list[i].DeepCopy());
            }
            
            student_copy.exam_list = new List<Exam>();
            for (int i = 0; i < this.exam_list.Count; i++)
            {
                student_copy.exam_list.Add(this.exam_list[i].DeepCopy());
            }
            return student_copy;
        }

        object IDateAndCopy.DeepCopy()
        {
            return this.DeepCopy();
        }

        DateTime IDateAndCopy.Date
        {
            get
            {
                return new DateTime();
            }
            set { }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            //if (System.Object.ReferenceEquals(this, obj))
            //    return true;

            Student s = obj as Student;
            if ((object)s == null)
                return false;
            Person p = obj as Person;
            if ((object)p == null)
                return false;

            return base.Name == s.Name &&
                   base.Last_Name == s.Last_Name &&
                   base.Birth_Date == s.Birth_Date &&
                   this.Degree.ToString() == s.Degree.ToString() &&
                   this.Group_Number.ToString() == s.Group_Number.ToString() &&
                   this.Exam_List_ToString() == s.Exam_List_ToString() &&
                   this.Credit_List_ToString() == s.Credit_List_ToString();
        }

        public static bool operator ==(Student s1, Student s2)
        {
            if (System.Object.ReferenceEquals(s1, s2))
                return true;
            if ((object)s1 == null || (object)s2 == null)
                return false;

            return s1.name == s2.name &&
                s1.last_name == s2.last_name &&
                s1.birth_date == s2.birth_date &&
                s1.degree == s2.degree &&
                s1.group_number == s2.group_number &&
                s1.credit_list == s2.credit_list &&
                s1.exam_list == s2.exam_list;
        }

        public static bool operator !=(Student s1, Student s2)
        { return !(s1 == s2); }

        public override int GetHashCode()
        {
            if (this.credit_list == null ||
                this.exam_list == null)
            {
                throw new NullReferenceException("One of the Student object's fields is null.");
            }
            else
            {
                unchecked
                {
                    int hash = 29;
                    hash = hash * 31 + base.GetHashCode();
                    hash = hash * 31 + (int)this.degree;
                    hash = hash * 31 + this.group_number;
                    hash = hash * 31 + Credit_List_GetHashCode();
                    hash = hash * 31 + Exam_List_GetHashCode();
                    return hash;
                }
            }
            /*
            try
            {
                
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("One of the Person object fields is null.");
                Console.WriteLine(nre.Message);
                return -1;
            }
             * */
        }

        private int Credit_List_GetHashCode()
        {
            unchecked
            {
                int hash = 29;
                foreach (Credit cr in credit_list)
                {
                    hash = hash * 31 + cr.GetHashCode();
                }
                return hash;
            }
        }

        private int Exam_List_GetHashCode()
        {
            unchecked
            {
                int hash = 29;
                foreach (Exam ex in exam_list)
                {
                    hash = hash * 31 + ex.GetHashCode();
                }
                return hash;
            }
        }

        /* StudentEnumerator - helper class, which implements
         * IEnumerator interface. This means, it has methods MoveNext()
         * and Reset() and public property Current.
         * 
         * StudentEnumerator should allow to iterate through a collection
         * of Strings. Where strings are names of Exams and Credits that
         * belong to both credit and exam lists.
         */
        private class StudentEnumerator : IEnumerator
        {
            /* ArrayList - to hold a collection course names.
             * Course names are strings. And courses are Credits or Exams
             * that belong to both credit and exam lists.
             */
            private ArrayList credit_intersect_exam;
            // current - holds current index in the collection
            private int current;

            public StudentEnumerator(Student input_s)
            {
                IComparer comparer = new Credit_Exam_Comparer();
                input_s.exam_list.Sort();
                credit_intersect_exam = new ArrayList();
                /*
                for (int i = 0; i < input_s.credit_list.Count; i++ )
                {
                    //Console.WriteLine("In the loop.");
                    //if (input_s.exam_list.BinarySearch(((Credit)input_s.credit_list[i]).Credit_Name, comparer) > 0)
                    if (input_s.exam_list.BinarySearch((Credit)input_s.credit_list[i], comparer) == 0 ||
                        input_s.exam_list.BinarySearch((Credit)input_s.credit_list[i], comparer) > 0)
                    {
                        credit_intersect_exam.Add(((Credit)input_s.credit_list[i]).Credit_Name);
                        //Console.WriteLine("Added Credit to StudentEnumerator.");
                    }
                }
                 * */
                this.current = -1;
            }

            // read-only field to get an object from credit_intersect_exam
            // with [current] index
            public object Current
            {
                get { return credit_intersect_exam[current]; }
            }

            // increases [current] by one.
            // returns false if [current] exceeds credit_intersect_exam length
            public bool MoveNext()
            {
                ++current;
                if (current < credit_intersect_exam.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                current = -1;
            }


            /* Credit_Exam_Comparer is a helper class which implements
             * IComparer interface. It implements Compare(obj, obj) method.
             * Object of this class is used by BinarySearch method from ArrayList class
             * to compare Exam and Credit objects based on thier names.
             */
            private class Credit_Exam_Comparer : IComparer
            {

                int IComparer.Compare(object x, object y)
                {
                    //return String.Compare(((Exam)x).Exam_Name, ((Credit)y).Credit_Name);
                    //return String.Compare(((Exam)x).Exam_Name, (String)y);
                    //Console.WriteLine("Comparing.");

                    string e_name = ((Exam)x).Exam_Name;
                    return e_name.CompareTo(((Credit)y).Credit_Name);
                }
            }
        }

        /* Credit_Exam_Comparer is a helper class which implements
             * IComparer interface. It implements Compare(obj, obj) method.
             * Object of this class is used by BinarySearch method from ArrayList class
             * to compare Exam and Credit objects based on thier names.
             */
        private class Credit_Exam_Comparer : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                //return String.Compare(((Exam)x).Exam_Name, ((Credit)y).Credit_Name);
                //return String.Compare(((Exam)x).Exam_Name, (String)y);
                //Console.WriteLine("Comparing.");

                string e_name = ((Exam)x).Exam_Name;
                return e_name.CompareTo(((Credit)y).Credit_Name);
            }
        }

        public class StudentAGPComparer : IComparer<Student>
        {
            int IComparer<Student>.Compare(Student x, Student y)
            {
                // TODO: consider when x == null and/or y == null cases.
                return (x.AGP).CompareTo(y.AGP);
            }
        }

        /* Student class implements IEnumerable interface,
         * so it should implement GetEnumerator() method.
         * GetEnumerator() should return an object, that
         * implements IEnumerator interface.
         */
        IEnumerator IEnumerable.GetEnumerator()
        {
            /* This method returns a new StudentEnumerator object.
             * StudentEnumerator class implements IEnumerator interface.
             */
            return new StudentEnumerator(this);
        }

        /* Method for sorting List<Exam> by Exam_Name */
        public void SortExamsByName()
        {
            /* As long as Exam implements IComparable,
             * Exam.CompareTo method will be used by List.Sort()
             * as a default comparer.
             */
            this.exam_list.Sort();
        }

        /* Method for sorting List<Exam> by Grade */
        public void SortExamsByGrade()
        {
            this.exam_list.Sort(delegate(Exam x, Exam y)
                                {
                                    return x.Compare(x, y);
                                });
        }

        /* Method for sorting List<Exam> by Date */
        public void SortExamsByDate()
        {
            Exam.ExamDateComparer edc = new Exam.ExamDateComparer();
            this.exam_list.Sort(edc);
        }

        public bool Save(string file_name)
        {
            try
            {
                FileStream f_stream = new FileStream(file_name, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(f_stream, this);
                f_stream.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool Load(string file_name)
        {
            FileStream f_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            Student st = (Student) formatter.Deserialize(f_stream);

            this.Passport_Data = st.Passport_Data;
            this.Degree = st.Degree;
            this.Group_Number = st.Group_Number;
            this.Exam_List = st.Exam_List;
            this.Credit_List = st.Credit_List;
            
            f_stream.Close();
            return true;
        }

        public bool AddFromConsole()
        {
            Console.WriteLine("Input data for a new Exam");
            Console.WriteLine("Exam name, Exam Date (day/month/year, Grade");
            Console.WriteLine("Delimiters: ;, \"space\", -");

            char[] delimiters = { ';', ' ', '-' };

            string input = Console.ReadLine();

            string[] string_data = input.Split(delimiters);

            Exam exam = new Exam();
            exam.Exam_Name = string_data[0];
            try
            {
                DateTime d_t = Convert.ToDateTime(string_data[1]);
                exam.Exam_Date = d_t;
            }
            catch (Exception e)
            {
                //do nothing for now
                //throw;
            }
            int input_grade = 0;
            try
            {
                int.TryParse(string_data[2], out input_grade);
                exam.Grade = input_grade;
            }
            catch (Exception e)
            {
                // do nothing for now
            }
            this.AddExams(exam);
            return true;
        }

        public static bool Save(string file_name, Student obj)
        {
            try
            {
                FileStream f_stream = new FileStream(file_name, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(f_stream, obj);
                f_stream.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        // TODO: if output object initialization did not succeded,
        // Load mehod should keep the old values unchanged.
        public static bool Load(string file_name, out Student obj)
        {
            FileStream f_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            obj = (Student) formatter.Deserialize(f_stream);

            //this.Passport_Data = st.Passport_Data;
            //this.Degree = st.Degree;
            //this.Group_Number = st.Group_Number;
            //this.Exam_List = st.Exam_List;
            //this.Credit_List = st.Credit_List;

            f_stream.Close();
            return true;
        }
    }
}
