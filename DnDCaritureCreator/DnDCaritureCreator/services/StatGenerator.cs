using DnDCaritureCreator.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDCaritureCreator.services
{
    internal class StatGenerator
    {

        public CharacterStats RolledStats(CharacterStats storedStats)
        {
            Random random = new Random();

            //gen dice numbers
            List<int> rolledStats = new List<int>();

            for (int i = 0; i < 6; i++)
                rolledStats.Add(random.Next(1,21));


            List<string> statNames = new List<string>() { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };



            //UI managment flags
            List<int?> usedStats = new List<int?>() { null, null, null, null, null, null };
            int slectedRoll = 0;
            int slectedStat = 6;// any number 6 or higher deactivates selection curser
            bool pickStat = false; //used for control not rendering



            //UI
            while( true)
            {
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine("ROLLED STATS EDITOR");

                // show rolled numbers
                Console.Write("rolled stats:");
                for (int i = 0; i < 6; i++)
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;

                    if ((usedStats[i] != null) && (i == slectedRoll))
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else if (usedStats[i] != null)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (i == slectedRoll)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    

                    Console.Write(rolledStats[i]);
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();

                for (int i = 0; i < 6; i++)
                {
                    int statValue = 0;

                    //check if a stat has been assigned
                    for(int j = 0; j < 6; j++)
                    {
                        if(i == usedStats[j])
                            statValue = rolledStats[j];
                    }
                    

                    
                    // colour managment
                    Console.ResetColor();
                    if (i == slectedStat)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    if (statValue != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.WriteLine(statNames[i] + ": " + statValue);



                }



                Console.ReadLine();



            }










            return storedStats;

        }


    }
}
