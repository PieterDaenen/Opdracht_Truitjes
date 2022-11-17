using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;

namespace VerkoopTruithesBL.Model
{
    public class Truitje
    {
        public Truitje()
        {

        }
        public Truitje(double prijs, string seizoen, Club club, ClubSet clubSet, KledingMaat kledingMaat)
        {
            Prijs = prijs;
            Seizoen = seizoen;
            Club = club;
            ClubSet = clubSet;
            KledingMaat = kledingMaat;
        }

        public Truitje(double prijs, int id, string seizoen, Club club, ClubSet clubSet, KledingMaat kledingMaat)
        {
            Prijs = prijs;
            Id = id;
            Seizoen = seizoen;
            Club = club;
            ClubSet = clubSet;
            KledingMaat = kledingMaat;
        }

        public double Prijs { get; private set; }
        public int Id { get; private set; }
        public string Seizoen { get; private set; }
        public Club Club { get; private set; }
        public ClubSet ClubSet {get;private set;}
        public KledingMaat KledingMaat { get; set; }
        public void ZetPrijs(double prijs)
        {
            if (prijs < 0) throw new TruitjeException("ZetPrijs");
            Prijs = prijs;
        }
        public void ZetId(int id)
        {
            if (id <= 0) throw new TruitjeException("ZetId");
            Id = id;
        }
        public void ZetSeizoen(string seizoen)
        {
            if (string.IsNullOrWhiteSpace(seizoen)) throw new TruitjeException("ZetSeizoen");
            Seizoen=seizoen;
        }
        public void ZetClub(Club club)
        {
            if (club == null) throw new TruitjeException("ZetClub");
            Club = club;
        }
        public void ZetClubSet(ClubSet clubSet)
        {
            if (clubSet == null) throw new TruitjeException("ZetClubSet");
            ClubSet = clubSet;
        }
        public override bool Equals(object? obj)
        {
            return obj is Truitje truitje &&
                   Id == truitje.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string? ToString()
        {
            return $"{Id}, {Seizoen}, {Club.ToString()}, {ClubSet.ToString()}, {KledingMaat}, {Prijs}";
        }
    }
}
