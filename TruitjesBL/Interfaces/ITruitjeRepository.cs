using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Model;

namespace VerkoopTruithesBL.Interfaces
{
    public interface ITruitjeRepository
    {
        bool BestaatClubSet(ClubSet clubset);
        bool BestaatTruitje(Truitje truitje);
        bool BestaatTruitje(int truitjeId);
        void VoegClubSetToe(ClubSet clubset);
        void VoegTruitjeToe(Truitje truitje);
        Truitje GeefTruitje(int value);
        IEnumerable<Truitje> GeefTruitjes(string? competitie, string? club, string? seizoen, string? kledingmaat, int? versie, bool? thuis, double? prijs);
        void VerwijderTruitje(Truitje voetbaltruitje);
        void UpdateTruitje(Truitje voetbaltruitje);
    }
}
