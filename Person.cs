using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieMall
{
    class Person
    {
        public char job = 'M';
        public string name = CODES.GENERIC_NAME;
        public bool alive = true;
        private int timeAlive = 0;
        public int energy = 100;
        public int joy = 100;
        public int waste = 0;

        public int DP_energy;  // Stands for "Depletion Period: Energy";
        public int DP_joy;     // Stands for "Depletion Period: Joy";
        public int DP_waste;   // Stands for "Depletion Period: Waste";

        public int cash;
        public List<Item> bag = new List<Item>();

        // Duration of different actions
        protected int busyClock = 0;
        protected int talkDuration = 30;        // At least 30s of talking
        protected int talkChance = 1;           // 1% per second

        protected int toiletDuration = 5*60;    // 5min at the toilet

        public Location location;               

        public Person(int DP_energy, int DP_joy, int DP_waste)
        {
            this.cash = HIVEMIND.RANDOM.Next(1, 100);  // Each person starts with a random amount of cash from 0 to 100 usd
            this.DP_energy = DP_energy;
            this.DP_joy = DP_joy;
            this.DP_waste = DP_waste;            
        }

        public Person(string name, int DP_energy, int DP_joy, int DP_waste)
        {
            this.cash = HIVEMIND.RANDOM.Next(1, 100);  // Each person starts with a random amount of cash from 0 to 100 usd
            this.DP_energy = DP_energy;
            this.DP_joy = DP_joy;
            this.DP_waste = DP_waste;
            this.name = name;
        }

        public Person(string name, int DP_energy, int DP_joy, int DP_waste, Location location)
        {
            this.cash = HIVEMIND.RANDOM.Next(1, 100);  // Each person starts with a random amount of cash from 0 to 100 usd
            this.DP_energy = DP_energy;
            this.DP_joy = DP_joy;
            this.DP_waste = DP_waste;
            this.name = name;
            this.location = location;
        }

        public string UpdateStatus()
        {
            if (alive)
            {
                // We age the person and check if it consumed resources this turn
                timeAlive++;
                if (timeAlive % DP_energy == 0) { energy--; }
                if (timeAlive % DP_joy == 0)    { joy--; }
                if (timeAlive % DP_waste == 0)  { waste++; }

                // We check if the person died this turn
                if (energy <= 0 || joy <= 0 || waste >= 100)
                {
                    alive = false;
                    if (energy <= 0)    { return name + CODES.DEATH_ENERGY; }
                    else if (joy <= 0)  { return name + CODES.DEATH_JOY; }
                    else                { return name + CODES.DEATH_WASTE; }
                }

                if (busyClock > 0)
                {
                    busyClock--;
                    if (busyClock == 0) { return name + CODES.IS_AVAILABLE; }
                }
                return "";
            }
            return "";
        }

        public string Act(Dictionary<string, Location> mall)
        {
            if (!IsAvailable())  // Nothing to do, otherwise continue
            { 
                return "";
            }           

            int roll = HIVEMIND.RANDOM.Next(1, 100);     // A dice roll to select the next action
            if (roll > energy || roll > joy)             // A necessity needs to be covered
            {
                if (bag.Count > 0)
                {
                    return $"{name}{CODES.USED}{this.UseItem()}";
                }
                else if (location is Store)
                {
                    string aux = Buy((Store)location);  // we need to buy an item for the next cycle
                    if (aux.Equals(CODES.NO_STOCK) || aux.Equals(CODES.NO_VENDOR))
                    {
                        string previousStore = location.name;
                        LeaveStore(mall);
                        return name + CODES.LEAVE + previousStore;
                    }
                    else
                    {
                        return aux;
                    }
                }
                else
                {
                    EnterStore(mall);
                    return name + CODES.ENTER + location.name;
                }
            }
            else if (roll<=talkChance) 
            {
                //Speak(randomGuy);
            }
            return "";
        }

        public void Speak(Person p)
        {
            Console.WriteLine(this.name + " started talking with " + p.name);
            p.StartTalk();
            this.StartTalk();
        }

        public void StartTalk() 
        {
            busyClock = talkDuration;
        }

        public string Buy(Store st)
        {
            Vendor minion = st.CallVendor();
            if (minion != null)
            {
                Item purchase = minion.StartSale(st, this);
                if (purchase != null) 
                {
                    bag.Add(purchase);
                    return (this.name + CODES.BOUGHT + purchase.name + CODES.FROM + minion.name);
                }
                return CODES.NO_STOCK;
            }
            return CODES.NO_VENDOR;
        }

        public string UseItem()
        {
            if (bag.Count > 0) 
            {
                double[] priorities = this.GetPriorities();
                double currentValue = 0;
                int idx = 0;
                for (int i = 0; i < bag.Count; i++)
                {
                    double value = bag[i].nutrients * priorities[0] + bag[i].fun * priorities[1];
                    if (value > currentValue)
                    {
                        currentValue = value;
                        idx = i;
                    }
                }
                string usedItem = bag[idx].name;
                this.Consume(bag[idx]);
                //bag.RemoveAt(idx);
                return usedItem;
            } 
            return "";
        }

        public void Consume(Item item)
        {
            this.energy += item.nutrients;
            if (this.energy > 100) { this.energy = 100; }
            this.joy += item.fun;
            if (this.joy > 100) { this.joy = 100; }
            this.bag.Remove(item);
        }

        public void PrintStats() 
        {
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Energy: " + this.energy + " || " + this.DP_energy);
            Console.WriteLine("Joy: " + this.joy + " || " + this.DP_joy);
            Console.WriteLine("Waste: " + this.waste + " || " + this.DP_waste);
            Console.WriteLine("Cash: $" + this.cash);
        }

        public bool IsAvailable() 
        {
            return !alive ? false : busyClock <= 0;
        }

        public double[] GetPriorities()
        {
            return new double[] { (100.0 - energy) / 100.0, (100.0 - joy) / 100.0 };
        }

        public void AddLocation(Location place) 
        {
            this.location = place;
        }

        public void LeaveStore(Dictionary<string, Location> mall) 
        {
            location = mall[CODES.HALL_NAME];
            mall[CODES.HALL_NAME].AddResident(this);
        }

        public void EnterStore(Dictionary<string, Location> mall)
        {
            int i = HIVEMIND.RANDOM.Next(1, mall.Count);
            location = mall.ElementAt(i).Value;
            mall[CODES.HALL_NAME].RemoveResident(this);
        }
    }
}
