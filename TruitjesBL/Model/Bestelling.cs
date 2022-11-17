using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;

namespace VerkoopTruithesBL.Model
{
    public class Bestelling
    {
        private Dictionary<Truitje, int> _truitjes = new Dictionary<Truitje, int>();

        public Bestelling(DateTime tijdstip)
        {
            Tijdstip = tijdstip;
        }

        public Bestelling(Dictionary<Truitje, int> truitjes, int bestellingNr, DateTime tijdstip, double prijs, Klant klant, bool betaald)
        {
            _truitjes = truitjes;
            Tijdstip = tijdstip;
            ZetBestellingNr(bestellingNr);
            ZetPrijs(prijs);
            ZetKlant(klant);
            ZetBetaald();
        }

        public int BestellingNr { get; private set; }
        public DateTime Tijdstip { get; set; }
        public double Prijs { get; private set; }
        public Klant Klant { get; private set; }
        public bool Betaald { get; private set; }
        public void ZetBestellingNr(int bestellingNr)
        {
            if (bestellingNr <= 0) throw new BestellingException("bestellingnr <=0");
            BestellingNr = bestellingNr;
        }
        public void ZetPrijs(double prijs)
        {
            if (prijs < 0) throw new BestellingException("prijs < 0");
            Prijs = prijs;
        }
        public void ZetKlant(Klant nieuweKlant)
        {
            if (nieuweKlant == null) throw new BestellingException("klant is null");
            if (nieuweKlant.Equals(Klant)) throw new BestellingException("klant is dezelfde");
            if (Klant != null) Klant.VerwijderBestelling(this);
            Klant = nieuweKlant;
            if (!nieuweKlant.GeefBestellingen().Contains(this))
                nieuweKlant.VoegBestellingToe(this);
        }
        public void VerwijderKlant()
        {
            if (Klant != null) 
                if (Klant.GeefBestellingen().Contains(this))  Klant.VerwijderBestelling(this);
            Klant = null;
        }
        public void ZetBetaald()
        {
            Betaald = true;
            //waarom zetprijs?
            ZetPrijs(BerekenPrijs());
        }
        public double BerekenPrijs()
        {
            double prijs=0.0;
            foreach(KeyValuePair<Truitje,int> keyValuePair in _truitjes)
            {
                prijs += keyValuePair.Value * keyValuePair.Key.Prijs;
            }
            if (Klant == null)
            {
                return prijs;
            }
            return prijs*(1-(Klant.Korting()/100.0));
            
        }
        public void ZetOnbetaald()
        {
            Betaald = false;
            //waarom prijs = 0?
            Prijs = 0.0;
        }
        public void VoegTruitjesToe(Truitje truitje,int aantal)
        {
            if (Betaald) throw new BestellingException("VoegTruitjesToe - reeds betaald");
            if (truitje == null) throw new BestellingException("VoegTruitjesToe");
            if (aantal<=0) throw new BestellingException("VoegTruitjesToe");
            if (_truitjes.ContainsKey(truitje))
            {
                _truitjes[truitje] += aantal;
            }
            else
            {
                _truitjes.Add(truitje, aantal);
            }
        }
        public void VerwijderTruitjes(Truitje truitje,int aantal)
        {
            if (Betaald) throw new BestellingException("VerwijderTruitjes - reeds betaald");
            if (truitje == null) throw new BestellingException("VerwijderTruitjes");
            if (aantal <= 0) throw new BestellingException("VerwijderTruitjes");
            if (!_truitjes.ContainsKey(truitje))
            {
                throw new BestellingException("VerwijderTruitjes");
            }
            if (_truitjes[truitje]<aantal) throw new BestellingException("VerwijderTruitjes");
            if (_truitjes[truitje]==aantal) _truitjes.Remove(truitje);
            else _truitjes[truitje] -= aantal;
        }
        public IReadOnlyDictionary<Truitje,int> GetTruitjes()
        {
            return _truitjes;
        }
        public bool IsDezelfde(Bestelling andereBestelling)
        {

            if (andereBestelling == null) throw new BestellingException("IsDezelfde - Bestelling is null");
            return (andereBestelling.Prijs == Prijs)
                && (andereBestelling.Betaald == Betaald)
                && (andereBestelling.Klant == Klant)
                && (andereBestelling.Tijdstip == Tijdstip);
            //    && (andereBestelling._truitjes.Equals( _truitjes));


            //if (andereBestelling == null) throw new BestellingException("IsDezelfde - bestelling is null");
            //if (BestellingNr != andereBestelling.BestellingNr) return false;
            //if (Prijs != andereBestelling.Prijs) return false;
            //if (Klant.Equals(andereBestelling.Klant)) return false;
            //if (!Betaald.Equals(andereBestelling.Betaald)) return false;
            //if (!Tijdstip.Equals(andereBestelling.Tijdstip)) return false;
            //if (!_truitjes.Equals(andereBestelling._truitjes)) return false;
            //return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is Bestelling bestelling &&
                   BestellingNr == bestelling.BestellingNr;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BestellingNr);
        }
    }
}
