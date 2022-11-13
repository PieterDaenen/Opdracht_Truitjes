using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Model
{
    public class ClubSet
    {
        public ClubSet()
        {

        }
        public ClubSet(bool uit, int versie)
        {
            Uit = uit;
            Versie = versie;
        }

        public bool Uit { get; set; }
        public int Versie { get; set; }
    }
}
