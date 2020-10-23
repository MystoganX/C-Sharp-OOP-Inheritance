using System;
using System.Collections.Generic;
using System.Text;

namespace ZombieMall
{
    class Item
    {
		public string name;
		public int cost;
		public int nutrients;
		public int fun;

		public Item(string name, int cost, int nutrients, int fun)
		{
			this.name = name;
			this.cost = cost;
			this.nutrients = nutrients;
			this.fun = fun;
		}
	}
}
