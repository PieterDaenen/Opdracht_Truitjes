using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruithesBL.Model;

namespace VerkoopTruitjesDL.Repositories
{
    public class BestellingRepository : IBestellingRepository
    {
        public bool BestaatBestelling(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }

        public bool BestaatBestelling(int bestellingNr)
        {
            throw new NotImplementedException();
        }

        public Bestelling GeefBestelling(int bestellingNr)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bestelling> GeefBestellingen(int? klantId, DateTime? startDatum, DateTime? eindDatum)
        {
            throw new NotImplementedException();
        }

        public void UpdateBestelling(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }

        public void VerwijderBestelling(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }

        public void VoegBestellingToe(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }
    }
}
