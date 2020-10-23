using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieMall
{
    class Location
    {
        public string name;        
        public List<Item> items = new List<Item>();
        public List<Person> residents = new List<Person>();

        public Location(string nickname)
        {
            this.name = nickname;
        }

        public void AddItem(Item item)
        {
            this.items.Add(item);
        }

        public Person GetResident() 
        {
            return residents[HIVEMIND.RANDOM.Next(residents.Count)];
        }

        public Person GetResident(Person p)
        {
            Console.WriteLine(residents.Where(i => !Equals(p)));
            return null;
        }

        public void AddResident(Person p) 
        {
            residents.Add(p);
        }

        public void RemoveResident(Person p)
        {
            residents.Remove(p);
        }

    }
}
