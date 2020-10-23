using ZombieMall;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZombieMall
{
    class Vendor: Person
    {
        protected int saleDuration = 8;

        public Vendor(int dp_en, int dp_joy, int dp_wst): base(dp_en, dp_joy, dp_wst)
        {            
        }

        public Vendor(string nickname, int dp_en, int dp_joy, int dp_wst) : base(nickname,dp_en, dp_joy, dp_wst)
        {            
        }

        public Item StartSale(Store st, Person customer) 
        {
            if (st.items.Count > 0) 
            {
                this.busyClock = saleDuration;
                double[] priorities = customer.GetPriorities();

                Item sale = null;
                double subjectiveValue = 0;
                foreach (Item item in st.items)
                {                    
                    double currentValue = item.nutrients * priorities[0] + item.fun * priorities[1];  // to help customer make the best decision
                    if (item.cost <= customer.cash && currentValue > subjectiveValue)
                    {
                        sale = item;
                    }
                }
                st.items.Remove(sale);
                st.AddCash(sale.cost);
                customer.cash -= sale.cost;                
                return sale;
            }
            return null;
            
        }
    }
}
