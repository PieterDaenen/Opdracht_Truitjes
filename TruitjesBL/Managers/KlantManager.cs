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
    public class KlantManager
    {
        private IKlantRepository _klantRepo;

        public KlantManager(IKlantRepository klantRepo)
        {
            this._klantRepo = klantRepo;
        }

        public void KlantToevoegen(Klant klant)
        {
            if (klant == null) throw new KlantManagerException("KlantToevoegen");
            try
            {
                if (_klantRepo.BestaatKlant(klant))
                {
                    throw new KlantManagerException("KlantToevoegen - klant bestaat al");
                }
                _klantRepo.VoegKlantToe(klant);
            }
            catch(Exception ex)
            {
                throw new KlantManagerException("KlantToevoegen", ex);
            }
        }
        public void KlantVerwijderen(Klant klant)
        {
            if (klant == null) throw new KlantManagerException("KlantVerwijderen");
            try
            {
                if (!_klantRepo.BestaatKlant(klant.KlantNr))
                {
                    throw new KlantManagerException("KlantVerwijderen - klant bestaat niet");
                }

                IReadOnlyList<Bestelling> bestellingen = klant.GeefBestellingen();
                bool moetNogBetalen = false;
                foreach (Bestelling b in bestellingen)
                {
                    if (b.Betaald == false) moetNogBetalen = true;
                }
                if (moetNogBetalen) throw new KlantManagerException("KlantVerwijderen - klant heeft nog bestellingen te betalen");
                _klantRepo.VerwijderKlant(klant);
            }
            catch (Exception ex)
            {
                throw new KlantManagerException("KlantVerwijderen", ex);
            }
        }
        public void KlantUpdaten(Klant klant)
        {
            if (klant == null) throw new KlantManagerException("KlantUpdaten");
            try
            {
                if (_klantRepo.BestaatKlant(klant.KlantNr))
                {
                    Klant dbKlant = _klantRepo.GeefKlant(klant.KlantNr);
                    if (dbKlant.IsDezelfde(klant)) throw new KlantManagerException("KlantUpdaten");
                    _klantRepo.UpdateKlant(klant);
                }
               else
                {
                    throw new KlantManagerException("KlantUpdaten");
                }

            }
            catch (Exception ex)
            {
                throw new KlantManagerException("KlantUpdaten", ex);
            }
        }
        public IReadOnlyList<Klant> ZoekKlant(int? klantId,string naam,string adres)
        {
            List<Klant> klanten=new List<Klant> ();
            try
            {
                if (klantId.HasValue)
                {
                    if (_klantRepo.BestaatKlant(klantId.Value))
                        klanten.Add(_klantRepo.GeefKlant(klantId.Value));
                    //else
                      //  throw new KlantManagerException("ZoekKlant");
                }
                else
                {
                    if ((!string.IsNullOrWhiteSpace(naam)) || !string.IsNullOrWhiteSpace(adres))
                        //tryparse bijzetten?
                    {
                        klanten.AddRange(_klantRepo.GeefKlanten(naam,adres));
                    }
                    else
                    {
                        throw new KlantManagerException("ZoekKlant - naam,adres zijn leeg");
                    }
                }
                return klanten;
            }
            catch(Exception ex)
            {
                throw new KlantManagerException("ZoekKlant", ex);
            }
        }
    }
}
