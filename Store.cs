using System;
using System.Collections.Generic;
using System.Text;
using ZombieMall;

namespace ZombieMall
{
    class Store: Location
    {
        private int money = 0;

        public List<Person> employees = new List<Person>();       

        public Store(string nickname): base(nickname)
        {          
        }

        public Vendor CallVendor() 
        {
            for (int i = 0; i < this.employees.Count; i++) 
            {
                if (this.employees[i] is Vendor && this.employees[i].IsAvailable()) 
                {
                    return (Vendor) employees[i];
                }
            }
            return null;
        }

        public void AddEmployee(Person employee) 
        {
            this.employees.Add(employee);
        }
        
        public void AddCash(int cash)
        {
            money += cash;
        }
    }


}
