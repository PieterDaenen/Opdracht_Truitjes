using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Interfaces
{
    public interface IClubRepository
    {
        IReadOnlyList<string> GeefCompetities();
        bool BestaatCompetitie(string competitie);
        IReadOnlyList<string> GeefClubs(string competitie);
    }
}
