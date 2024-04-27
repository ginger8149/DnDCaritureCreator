using DnDCaritureCreator.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            {
                List<int> dice = new List<int>();
                for (int j = 0; j < 4; j++)
                    dice.Add(random.Next(1, 6));
                dice.Sort();
                rolledStats.Add(dice[1] + dice[2] + dice[3]);
            }
                





            List<string> statNames = new List<string>() { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };

            //UI managment flags
            List<int?> usedStats = new List<int?>() { null, null, null, null, null, null };
            int slectedRoll = 0;
            int slectedStat = 6;// any number 6 or higher deactivates selection curser
            bool pickStat = false; //used for control not rendering
            bool removeStat = false;
            


            //UI 
            while( true)
            {

                // UI renderer
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
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
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.WriteLine();


                // stat renderer
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
                    Console.ForegroundColor = ConsoleColor.Gray;
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

                Console.WriteLine();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
                if (!pickStat)
                    Console.WriteLine("Enter: select stat value  r: remove assigned stat");
                else
                    Console.WriteLine("Enter: assign to stat c:Cancel");

                // ui controler
                ConsoleKeyInfo keyPress =  Console.ReadKey();

                if (!pickStat)
                {
                    if(keyPress.Key == ConsoleKey.LeftArrow)
                    {
                        slectedRoll--;
                        if (slectedRoll < 0)
                            slectedRoll = 5;
                    }
                    if (keyPress.Key == ConsoleKey.RightArrow)
                    {
                        slectedRoll++;
                        if (slectedRoll > 5)
                            slectedRoll = 0;
                    }
                    if (keyPress.Key == ConsoleKey.R)
                    {
                        slectedRoll = 6;
                        slectedStat = 0;

                        pickStat = true;
                        removeStat = true;
                    }
                    if (keyPress.Key == ConsoleKey.Enter)
                    {
                        
                        slectedStat = 0;
                        pickStat = true;
                        
                    }

                }
                else if (removeStat)
                {
                    if (keyPress.Key == ConsoleKey.UpArrow)
                    {
                        slectedStat--;
                        if (slectedStat < 0)
                            slectedStat = 5;
                    }
                    if (keyPress.Key == ConsoleKey.DownArrow)
                    {
                        slectedStat++;
                        if (slectedStat > 5)
                            slectedStat = 0;
                    }
                    if (keyPress.Key == ConsoleKey.C)
                    {
                        slectedRoll = 0;
                        slectedStat = 6;
                        pickStat = false;
                        removeStat = false;

                    }
                    if (keyPress.Key == ConsoleKey.Enter) 
                    {
                        for(int i = 0; i < 6; i++)
                        {
                            if (usedStats[i] == slectedStat)
                            {
                                usedStats[i] = null;
                                slectedRoll = 0;
                                slectedStat = 6;
                                pickStat = false;
                                removeStat = false;

                            }
                        }

                    }
                }
                else
                {
                    if (keyPress.Key == ConsoleKey.UpArrow)
                    {
                        slectedStat--;
                        if (slectedStat < 0)
                            slectedStat = 5;
                    }
                    if (keyPress.Key == ConsoleKey.DownArrow)
                    {
                        slectedStat++;
                        if (slectedStat > 5)
                            slectedStat = 0;
                    }
                    if (keyPress.Key == ConsoleKey.C)
                    {
                        slectedRoll = 0;
                        slectedStat = 6;
                        pickStat = false;
                        removeStat = false;

                    }
                    if (keyPress.Key == ConsoleKey.Enter)
                    {
                        bool statAssigned = false;
                        for (int i = 0; i < 6; i++)
                        {
                            if (usedStats[i] == slectedStat)
                            {
                                statAssigned = true;
                            }
                        }

                        if (!statAssigned) { 
                            usedStats[slectedRoll] = slectedStat;
                            slectedRoll = 0;
                            slectedStat = 6;
                            pickStat = false;
                            removeStat = false;
                        }

                    }
                }


            }










            return storedStats;

        }


    }
}
