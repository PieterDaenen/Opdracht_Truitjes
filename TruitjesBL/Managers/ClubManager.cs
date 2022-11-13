using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;
using VerkoopTruithesBL.Interfaces;

namespace VerkoopTruithesBL.Managers
{
    public class ClubManager
    {
        private IClubRepository repo;

        public ClubManager(IClubRepository repo)
        {
            this.repo = repo;
        }
        public IReadOnlyList<string> GeefCompetities()
        {
            try
            {
                return repo.GeefCompetities();
            }
            catch (Exception ex)
            {
                throw new ClubManagerException("GeefCompetities", ex);
            }
        }
        public IReadOnlyList<string> GeefClubs(string competitie)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(competitie))
                {
                    if (repo.BestaatCompetitie(competitie))
                    {
                        return repo.GeefClubs(competitie);
                    }
                    else throw new ClubManagerException("GeefClubs - competie bestaat niet");
                }
                else throw new ClubManagerException("GeefClubs - competienaam is leeg");
            }
            catch (Exception ex)
            {
                throw new ClubManagerException("GeefClubs", ex);
            }
        }
    }
}
