using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_Var1_Level1
{
    [Serializable]
    public class Person : IDateAndCopy, IComparable, IComparer<Person>
    {
        protected string name;
        protected string last_name;
        protected System.DateTime birth_date;

        public Person()
        {
            this.name = "John";
            this.last_name = "Johnson";
            this.birth_date = new System.DateTime(1990, 2, 2);
            //Console.WriteLine("Created new Person with default constructor.");
        }

        public Person(string input_name, string input_last_name, System.DateTime input_birth_date) {
            this.name = input_name;
            this.last_name = input_last_name;
            this.birth_date = input_birth_date;
            //Console.WriteLine("Created new Person with given parameters.");
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Last_Name
        {
            get { return last_name; }
            set { last_name = value; }
        }

        public System.DateTime Birth_Date
        {
            get { return birth_date; }
            set { birth_date = value; }
        }

        public int Int_Birth_Date
        {
            get
            {
                return birth_date.Year * 10000 + birth_date.Month * 100 + birth_date.Day;
            }
            set
            {
                int year = (int)(value / 10000);
                int month = (int)((value - year * 10000) / 100);
                int day = (int)(value - year * 10000 - month * 100);
                this.birth_date = new System.DateTime(year, month, day);
            }
        }

        public override string ToString()
        {
            return this.name + ", " + this.last_name + ", " + this.birth_date.ToString("d");
        }

        public virtual string ToShortString()
        {
            return this.name + ", " + this.last_name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // if parameter obj can not be cast to Person return null
            Person p = obj as Person;
            if((System.Object)p == null)
            {
                return false;
            }

            return this.name == p.name &&
                   this.last_name == p.last_name &&
                   this.birth_date == p.birth_date;
        }

        //public bool Equals(Person p)
        //{
        //    if (p == null)
        //    {
        //        return false;
        //    }

        //    return this.name == p.name &&
        //           this.last_name == p.last_name &&
        //           this.birth_date == p.birth_date;
        //}

        public static bool operator ==(Person p1, Person p2)
        {
            if (System.Object.ReferenceEquals(p1, p2))
            {
                return true;
            }

            if ((object)p1 == null || (object)p2 == null)
            { return false; }

            return p1.name == p2.name &&
                   p1.last_name == p2.last_name &&
                   p1.birth_date == p2.birth_date;
        }

        public static bool operator !=(Person p1, Person p2)
        {
            return !(p1 == p2);
        }

        public override int GetHashCode()
        {
            if (this.name == null ||
                this.last_name == null ||
                this.birth_date == null)
            {
                throw new NullReferenceException("One of the Person object's fields is null.");
            }
            else
            {
                unchecked
                {
                    int hash = 29;
                    for (int i = 0; i < name.Length; i++)
                    {
                        hash = hash * 31 + (int)name[i];
                    }
                    for (int i = 0; i < last_name.Length; i++)
                    {
                        hash = hash * 31 + (int)last_name[i];
                    }
                    hash = hash * Int_Birth_Date;
                    return hash;
                }
            }
        }

        protected virtual object DeepCopy()
        {
            Person person_copy = new Person();
            person_copy.name = this.name;
            person_copy.last_name = this.last_name;
            person_copy.birth_date = this.birth_date;
            return person_copy;
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

        /* Implementation of IComparable interface.
         * CompareTo(object obj) compares two Person objects based
         * on their last names.
         */
        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Person another_person = (Person)obj;
            if (another_person != null)
                return this.last_name.CompareTo(another_person.last_name);
            else
                throw new ArgumentException("Passed object in not a Person.");
        }


        /* Implementation of IComparer<Person> interface.
         * Compare(Person x, Person y) compares two Person objects
         * based on their birth dates.
         */
        int IComparer<Person>.Compare(Person x, Person y)
        {
            return x.Birth_Date.CompareTo(y.Birth_Date);
        }
    }
}
