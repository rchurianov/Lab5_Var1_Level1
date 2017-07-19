using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_Var1_Level1
{
    [Serializable]
    public class Credit
    {
        public Credit(string name, bool passed)
        {
            this.Credit_Name = name;
            this.Credit_Passed = passed;
        }

        public Credit()
        {
            this.Credit_Name = "Linear Algebra";
            this.Credit_Passed = true;
        }

        public string Credit_Name
        { get; set; }

        public bool Credit_Passed
        { get; set; }

        public override string ToString()
        {
            return Credit_Name + " passed: " + Credit_Passed.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 29;
                for (int i = 0; i < Credit_Name.Length; i++)
                {
                    hash = hash * 31 + (int)Credit_Name[i];
                }

                if (Credit_Passed)
                    hash += 37;
                else
                    hash += 47;
                
                return hash;
            }
        }

        public Credit DeepCopy()
        {
            Credit credit_copy = new Credit();
            credit_copy.Credit_Name = this.Credit_Name;
            credit_copy.Credit_Passed = this.Credit_Passed;
            return credit_copy;
        }
    }
}
