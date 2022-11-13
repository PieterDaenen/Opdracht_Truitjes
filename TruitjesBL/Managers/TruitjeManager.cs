using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruithesBL.Model;

namespace VerkoopTruithesBL.Managers
{
    public class TruitjeManager
    {
        private ITruitjeRepository repo;

        public TruitjeManager(ITruitjeRepository repo)
        {
            this.repo = repo;
        }
        public void VoegTruitjeToe(Truitje truitje)
        {
            try
            {
                if (repo.BestaatTruitje(truitje))
                {
                    throw new TruitjeManagerException("VoegTruitjeToe - bestaat al");
                }
                else
                {
                    repo.VoegTruitjeToe(truitje);
                }
            }
            catch (Exception ex)
            {
                throw new TruitjeManagerException("VoegTruitjeToe", ex);
            }
        }
        public IReadOnlyList<Truitje> ZoekTruitjes(int? voetbaltruitjeId, string competitie, string club, string seizoen, string kledingmaat, int? versie, bool? thuis, double? prijs)
        {
            List<Truitje> truitjes = new List<Truitje>();
            try
            {
                if (voetbaltruitjeId.HasValue)
                {
                    if (repo.BestaatTruitje((int)voetbaltruitjeId)) truitjes.Add(repo.GeefTruitje(voetbaltruitjeId.Value));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(competitie) || !string.IsNullOrWhiteSpace(club)
                        || !string.IsNullOrWhiteSpace(seizoen) || !string.IsNullOrWhiteSpace(kledingmaat)
                        || versie.HasValue || prijs.HasValue || thuis.HasValue
                        )
                    {
                        truitjes.AddRange(repo.GeefTruitjes(competitie, club, seizoen, kledingmaat, versie, thuis, prijs, false));
                    }
                    else throw new TruitjeManagerException("ZoekTruitjes - geen zoekcriteria");
                }
                return truitjes;
            }
            catch (Exception ex)
            {
                throw new TruitjeManagerException("ZoekTruitjes", ex);
            }
        }
        public void VerwijderVoetbaltruitje(Truitje voetbaltruitje)
        {
            try
            {
                if (!repo.BestaatTruitje(voetbaltruitje))
                {
                    throw new TruitjeManagerException("VerwijderVoetbaltruitje");
                }
                else
                {
                    repo.VerwijderTruitje(voetbaltruitje);
                }
            }
            catch (Exception ex)
            {
                throw new TruitjeManagerException("VerwijderVoetbaltruitje", ex);
            }
        }
        public void UpdateVoetbaltruitje(Truitje voetbaltruitje)
        {
            try
            {
                if (repo.BestaatTruitje(voetbaltruitje.Id))
                {
                    Truitje dbVoetbaltruitje = repo.GeefTruitje(voetbaltruitje.Id);
                    if (dbVoetbaltruitje == voetbaltruitje) //TODO fix
                    {
                        throw new TruitjeManagerException("UpdateVoetbaltruitje - geen verschillen");
                    }
                    else
                    {
                        repo.UpdateTruitje(voetbaltruitje);
                    }
                }
                else
                {
                    throw new TruitjeManagerException("UpdateVoetbaltruitje - bestaat niet");
                }
            }
            catch (Exception ex)
            {
                throw new TruitjeManagerException("UpdateVoetbaltruitjet", ex);
            }
        }
    }
}
