using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Model;

namespace VerkoopTruithesBL.Interfaces
{
    public interface IKlantRepository
    {
        bool BestaatKlant(Klant klant);
        void VoegKlantToe(Klant klant);
        void VerwijderKlant(Klant klant);
        void UpdateKlant(Klant klant);
        Klant GeefKlant(int klantNr);
        bool BestaatKlant(int klantNr);
        IEnumerable<Klant> GeefKlanten(string naam, string adres);
    }
}
