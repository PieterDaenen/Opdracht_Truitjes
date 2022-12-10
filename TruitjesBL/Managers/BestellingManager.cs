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
    public class BestellingManager
    {
        private IBestellingRepository _bestellingRepo;

        public BestellingManager(IBestellingRepository bestellingRepo)
        {
            _bestellingRepo = bestellingRepo;
        }
        public void VoegBestellingToe(Bestelling bestelling)
        {
            if (bestelling == null) throw new BestellingManagerException("VoegBestellingToe");
            try
            {
                if (_bestellingRepo.BestaatBestelling(bestelling)) 
                    throw new BestellingManagerException("VoegBestellingToe");
                if (bestelling.Klant==null)
                    throw new BestellingManagerException("VoegBestellingToe");
                if (bestelling.GetTruitjes().Count==0)
                    throw new BestellingManagerException("VoegBestellingToe");
                _bestellingRepo.VoegBestellingToe(bestelling);
            }
            catch(Exception ex)
            {
                throw new BestellingManagerException("VoegBestellingToe",ex);
            }
        }
        public void VerwijderBestelling(Bestelling bestelling)
        {
            if (bestelling == null) throw new BestellingManagerException("VerwijderBestelling");
            try
            {
                if (!_bestellingRepo.BestaatBestelling(bestelling.BestellingNr))
                    throw new BestellingManagerException("VoegBestellingToe");
                _bestellingRepo.VerwijderBestelling(bestelling);
            }
            catch (Exception ex)
            {
                throw new BestellingManagerException("VerwijderBestelling", ex);
            }
        }
        public bool ZoekTruitjeInBestellingen(Truitje truitje)
        {
            try
            {
                return _bestellingRepo.ZoekTruitjeInBestellingen(truitje);
            } catch (Exception ex)
            {
                throw new BestellingManagerException("ZoekTruitjeInBestellingen", ex);
            }
        }
        public void UpdateBestelling(Bestelling bestelling)
        {
            if (bestelling == null) throw new BestellingManagerException("UpdateBestelling");
            try
            {
                if (!_bestellingRepo.BestaatBestelling(bestelling))
                    VoegBestellingToe(bestelling);
                if (bestelling.Klant == null)
                    throw new BestellingManagerException("UpdateBestelling");
                if (bestelling.GetTruitjes().Count == 0)
                    throw new BestellingManagerException("UpdateBestelling");
                if (_bestellingRepo.GeefBestelling(bestelling.BestellingNr).IsDezelfde(bestelling))
                    throw new BestellingManagerException("UpdateBestelling");
                _bestellingRepo.UpdateBestelling(bestelling);
            }
            catch (Exception ex)
            {
                throw new BestellingManagerException("UpdateBestellingen", ex);
            }
        }
        public IReadOnlyList<Bestelling> ZoekBestellingen(int? bestellingId,int? klantId,DateTime? startDatum,DateTime? eindDatum)
        {
            try
            {
                List<Bestelling> bestellingen = new List<Bestelling>();
                if (bestellingId.HasValue)
                {
                    if (_bestellingRepo.BestaatBestelling(bestellingId.Value))
                        bestellingen.Add(_bestellingRepo.GeefBestelling(bestellingId.Value));
                }
                else
                {
                    if ((klantId.HasValue) || (startDatum.HasValue) || (eindDatum.HasValue))
                        bestellingen.AddRange(_bestellingRepo.GeefBestellingen(klantId, startDatum, eindDatum));
                    else
                        throw new BestellingManagerException("ZoekBestellingen");
                }
                return bestellingen;
            }
            catch (Exception ex)
            {
                throw new BestellingManagerException("ZoekBestellingen", ex);
            }
        }
    }
}
