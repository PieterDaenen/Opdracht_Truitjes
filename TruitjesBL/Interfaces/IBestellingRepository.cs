using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Model;

namespace VerkoopTruithesBL.Interfaces
{
    public interface IBestellingRepository
    {
        void VoegBestellingToe(Bestelling bestelling);
        bool BestaatBestelling(Bestelling bestelling);
        void VerwijderBestelling(Bestelling bestelling);
        bool BestaatBestelling(int bestellingNr);
        void UpdateBestelling(Bestelling bestelling);
        Bestelling GeefBestelling(int bestellingNr);
        IEnumerable<Bestelling> GeefBestellingen(int? klantId, DateTime? startDatum, DateTime? eindDatum);
        bool ZoekTruitjeInBestellingen(Truitje truitje);


    }
}
