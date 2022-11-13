using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;
using VerkoopTruithesBL.Model;
using Xunit;

namespace TestDomein
{
    public class UnitTestTruitje
    { 
        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void Test_ZetId_Valid(int id)
        {
            Truitje t = new Truitje();
            t.ZetId(id);
            Assert.Equal(id, t.Id);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(0.5)]
        public void Test_ZetId_Invalid(int id)
        {
            Truitje t = new Truitje();
            Assert.Throws<TruitjeException>(() => t.ZetId(id));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5.5)]
        [InlineData(1000)]
        public void Test_ZetPrijs_Valid(double prijs)
        {
            Truitje t = new Truitje();
            t.ZetPrijs(prijs);
            Assert.Equal(prijs, t.Prijs);
        }
        [Theory]
        [InlineData(-0.5)]
        [InlineData(-1)]
        public void Test_ZetPrijs_Invalid(double prijs)
        {
            Truitje t = new Truitje();
            Assert.Throws<TruitjeException>(() => t.ZetPrijs(prijs));
        }
        [Theory]
        [InlineData("1696-1697")]
        [InlineData("2021-2022")]
        public void Test_ZetSeizoen_Valid(string seizoen)
        {
            Truitje t = new Truitje();
            t.ZetSeizoen(seizoen);
            Assert.Equal(seizoen, t.Seizoen);
        }
        [Theory]
        [InlineData("")]
        public void Test_ZetSeizoen_Invalid(string seizoen)
        {
            Truitje t = new Truitje();
            Assert.Throws<TruitjeException>(() => t.ZetSeizoen(seizoen));
        }
        [Fact]
        public void Test_ZetClub_Valid()
        {
            Club clubnaam = new Club();
            Truitje t = new Truitje();
            t.ZetClub(clubnaam);
            Assert.Equal(clubnaam, t.Club);
        }
        [Fact]
        public void Test_ZetClub_Invalid()
        {
            Club clubnaam = new Club();
            clubnaam = null;
            Truitje t = new Truitje();
            Assert.Throws<TruitjeException>(() => t.ZetClub(clubnaam));
        }
        [Fact]
        public void Test_ZetClubSet_Valid()
        {
            ClubSet clubset = new ClubSet();
            Truitje t = new Truitje();
            t.ZetClubSet(clubset);
            Assert.Equal(clubset, t.ClubSet);
        }
        [Fact]
        public void Test_ZetClubSet_Invalid()
        {
            ClubSet clubset = new ClubSet();
            clubset = null;
            Truitje t = new Truitje();
            Assert.Throws<TruitjeException>(() => t.ZetClubSet(clubset));
        }
    }
}
