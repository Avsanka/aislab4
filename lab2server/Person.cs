using System;
using System.Collections.Generic;
using System.Text;

namespace classPerson
{
    class Person
    {
        private int id;

        public int Id { get { return id; } set { id = value; } }

        private string name;

        public string Name { get { return name; } set { name = value; } }


        private string surname;
        public string Surname { get { return surname; } set { surname = value; } }


        private bool sex;
        public bool Sex { get { return sex; } set { sex = value; } }


        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value < 0)
                    age = 0;
                else
                    age = value;
            }
        }

    }
}
