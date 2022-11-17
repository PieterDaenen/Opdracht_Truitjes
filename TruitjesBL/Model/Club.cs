using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;

namespace VerkoopTruithesBL.Model
{
    public class Club
    {
        //om te testen
        public Club()
        {

        }
        public Club(string competitie, string ploegnaam)
        {
            Competitie = competitie;
            Ploegnaam = ploegnaam;
        }

        public string Competitie { get; private set; }
        public string Ploegnaam { get; private set; }

        public override string? ToString()
        {
            return $"{Competitie}, {Ploegnaam}";
        }

        public void ZetCompetitie(string competitie)
        {

            if (string.IsNullOrWhiteSpace(competitie)) throw new ClubException("competitie is null");
            if (competitie == Competitie) throw new ClubException("competitie is dezelfde");
            Competitie = competitie;
        }
        public void ZetPloegnaam(string ploegnaam)
        {
            if (string.IsNullOrWhiteSpace(ploegnaam)) throw new ClubException("naam is null");
            if (ploegnaam == Ploegnaam) throw new ClubException("naam is dezelfde");
            Ploegnaam = ploegnaam;
        }
    }
}
