using DnDCaritureCreator.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDCaritureCreator.services
{
    internal class StatGenerator
    {

        public CharacterStats RolledStats(CharacterStats stats)
        {
            Random random = new Random();

            //gen dice numbers
            List<int> rolledStats = new List<int>();
            for (int i = 0; i < 6; i++)
                rolledStats.Add(random.Next(1,21));
            

            //UI
            while( true)
            {







            }










            return stats;

        }


    }
}
