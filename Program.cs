using System;
using System.Collections.Generic;

namespace ZombieMall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the grand Dead or Alive Mall Simulation!");
            Random chaos = new Random();

            int population = chaos.Next(1, 100);
            List<Person> people = new List<Person>();
            Dictionary<string, Location> mall = new Dictionary<string, Location>();

            /* // We generate the people inside the mall
            for (int i = 0; i < population; i++) 
            {                
                people.Add(new Person(chaos.Next(1, 10), chaos.Next(1, 10), chaos.Next(1, 10)));
            }      
            int peopleAlive = 0;
            int peopleDead = 0;
            for (int n = 0; n < people.Count; n++)
            {
                
                //Console.WriteLine("Person <" + n + "> is alive? " + people[n].alive);                               
                //people[n].PrintStats();                
                //Console.WriteLine("Person <" + n + ">'s cash: " + people[n].cash);
                
                if (people[n].alive) { peopleAlive++; }
                else { peopleDead++; }
            }
            Console.WriteLine("Population: " + people.Count);
            Console.WriteLine("Number of people alive: " + peopleAlive);
            Console.WriteLine("Number of people dead: " + peopleDead);
            */

            Location hallway = new Location(CODES.HALL_NAME);
            mall.Add(CODES.HALL_NAME, hallway);

            people.Add(new Person("Iscariote", 2, 5, 1, mall[CODES.HALL_NAME]));
            people.Add(new Person("Iskandar", 3, 1, 200, mall[CODES.HALL_NAME]));
            people.Add(new Person("Gilgamesh", 5, 5, 5, mall[CODES.HALL_NAME]));

            Person alex = new Person("Alexander", 1, 1, 1);
            people.Add(alex);
            mall[CODES.HALL_NAME].AddResident(alex);

            Store st = new Store("La tienda de la esquina");
            Store st2 = new Store("Donde el chavo");
            Store st3 = new Store("24/7");
            Store st4 = new Store("Seventh Heaven");
            Vendor clerk = new Vendor("LeClerk", 100, 100, 100);
            Vendor lepope = new Vendor("LePope", 100, 100, 100);
            people.Add(clerk);
            people.Add(lepope);
            st.AddEmployee(clerk);
            st.AddEmployee(lepope);
            st.AddItem(new Item("Chocolate", 10, 15, 15));
            st.AddItem(new Item("Cup Noodles", 15, 40, 5));
            st.AddItem(new Item("Confetti", 5, 0, 50));
            st.AddItem(new Item("Full Course", 80, 100, 20));
            mall.Add(st.name, st);
            mall.Add(st2.name, st2);
            mall.Add(st3.name, st3);
            mall.Add(st4.name, st4);

            // Alex initial stats
            alex.cash = 1000;
            alex.energy = 5;
            alex.joy = 50;
            alex.AddLocation(st);

            // Let's check the initial stats
            Console.WriteLine("Initial stats of simulation: ");
            alex.PrintStats();
            Console.WriteLine("__________________________________________________");

            // And we start the simulation
            for (int i = 0; i < 150; i++)
            {
                for (int n = 0; n < people.Count; n++)
                {
                    string message1 = people[n].Act(mall);
                    string message2 = people[n].UpdateStatus();
                    if (message1 != "") 
                    {
                        Console.WriteLine("Cycle " + i + ": " + message1);                        
                    }
                    if (message2 != "")
                    {
                        Console.WriteLine("Cycle " + i + ": " + message2);
                    }
                }

            }
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("End results of simulation: ");
            alex.PrintStats();
            clerk.PrintStats();
            Console.WriteLine("");            
        }
    }
}
