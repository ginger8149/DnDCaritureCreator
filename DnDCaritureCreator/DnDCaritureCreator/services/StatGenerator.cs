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
            List<int> exsistingStats = new List<int>() { storedStats.strength, storedStats.dexterity, storedStats.constitution, storedStats.intelligence, storedStats.wisdom, storedStats.charisma };

            //UI managment flags
            List<int?> usedStats = new List<int?>() { null, null, null, null, null, null };
            int slectedRoll = 0;
            int slectedStat = 6;// any number 6 or higher deactivates selection curser
            bool pickStat = false; //used for control not rendering
            bool removeStat = false;

            //UI 
            while (true)
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
                    for (int j = 0; j < 6; j++)
                    {
                        if (i == usedStats[j])
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
                    Console.WriteLine(statNames[i] + ": " + statValue + $"{(exsistingStats[i] != 0 ? $" + {exsistingStats[i]}" : "")}");
                }

                Console.WriteLine();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
                if (!pickStat)
                    Console.WriteLine($"Enter: select stat value  R: remove assigned stat {(usedStats.Contains(null) ? "" : "F: Finalise and confirm")}");
                else
                    Console.WriteLine("Enter: assign to stat C:Cancel");

                // ui controler
                ConsoleKeyInfo keyPress = Console.ReadKey();

                if (!pickStat)
                {
                    if (keyPress.Key == ConsoleKey.LeftArrow)
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

                    //prevent runing if not all stat assigned
                    if ((keyPress.Key == ConsoleKey.F) && !usedStats.Contains(null))
                    {
                        //display are you sure message
                        Console.Clear();
                        Console.WriteLine("are you happy with these stats? y/n");
                        Console.WriteLine();
                        for (int i = 0; i < 6; i++)
                        {
                            int statValue = 0;
                            for (int j = 0; j < 6; j++)
                            {
                                if (i == usedStats[j])
                                    statValue = rolledStats[j];
                            }
                            Console.WriteLine(statNames[i] + ": " + (statValue + exsistingStats[i]));

                        }

                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Y)
                        {
                            //save choices to cariture
                            foreach (int stat in usedStats)
                            {
                                //"Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"
                                if (stat == 0)
                                    storedStats.strength += stat;
                                else if (stat == 1)
                                    storedStats.dexterity += stat;
                                else if (stat == 2)
                                    storedStats.constitution += stat;
                                else if (stat == 3)
                                    storedStats.intelligence += stat;
                                else if (stat == 4)
                                    storedStats.wisdom += stat;
                                else if (stat == 5)
                                    storedStats.charisma += stat;
                            }
                            break;
                        }
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
                        for (int i = 0; i < 6; i++)
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

                        if (!statAssigned)
                        {
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

        public CharacterStats PointBuy(CharacterStats storedStats)
        {

            // storage variables
            List<string> statNames = new List<string>() { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };
            List<int> exsistingStats = new List<int>() { storedStats.strength, storedStats.dexterity, storedStats.constitution, storedStats.intelligence, storedStats.wisdom, storedStats.charisma };

            List<int> stats = new List<int>() { 8, 8, 8, 8, 8, 8 };

            //ui flags
            int cursorPos = 0;


            // pre render un updated eliments and placeholder values
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();
            Console.WriteLine("POINT BUY EDITOR");

            Console.Write("Points available:");
            Console.CursorLeft = 20;
            Console.Write("00/27");
            Console.WriteLine();
            Console.WriteLine();
            
            // pre render stat names and values
            for (int i = 0; i < 6; i++)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;

                Console.Write(statNames[i]);

                if (i == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                
                Console.CursorLeft = 20;
                Console.Write($"<{stats[i]}>");
                Console.WriteLine();
            }

            Console.WriteLine();
            // controls
            Console.WriteLine("  ↑: Up  ↓: Down  \u2190: Decrease stat value  →: Increase stat value");

            // UI control loop
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.DownArrow) 
                {
                    // reset colours on current selection
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(20, cursorPos + 3);
                    Console.Write($"<{stats[cursorPos]}>");

                    //update cursor pos
                    cursorPos += 1;
                    if(cursorPos > 5)
                        cursorPos = 0;

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(20, cursorPos + 3);
                    Console.Write($"<{stats[cursorPos]}>");

                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    // reset colours on current selection
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(20, cursorPos + 3);
                    Console.Write($"<{stats[cursorPos]}>");

                    //update cursor pos
                    cursorPos -= 1;
                    if (cursorPos < 0)
                        cursorPos = 5;

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(20, cursorPos + 3);
                    Console.Write($"<{stats[cursorPos]}>");

                }

                Console.SetCursorPosition(0, 20);
            }
            return storedStats;
        }


    }
}
