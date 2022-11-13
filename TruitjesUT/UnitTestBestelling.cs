using System;
using System.Collections.Generic;
using VerkoopTruithesBL.Exceptions;
using VerkoopTruithesBL.Model;
using Xunit;

namespace TestDomein
{
    public class UnitTestBestelling
    {
        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(1)]
        public void Test_ZetPrijs_Valid(double prijs)
        {
            //arrange
            Bestelling b = new Bestelling(System.DateTime.Now);
            //act
            b.ZetPrijs(prijs);
            //assert
            Assert.Equal(prijs, b.Prijs);
        }
        [Theory]
        [InlineData(-0.5)]
        [InlineData(-1)]
        public void Test_ZetPrijs_Invalid(double prijs)
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Assert.Throws<BestellingException>(() => b.ZetPrijs(prijs));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void Test_ZetBestellingNr_Valid(int bestellingNr)
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.ZetBestellingNr(bestellingNr);
            Assert.Equal(bestellingNr, b.BestellingNr);
        }
        [Theory]
        [InlineData(0.5)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Test_ZetBestellingNr_Invalid(int bestellingNr)
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Assert.Throws<BestellingException>(() => b.ZetBestellingNr(bestellingNr));
        }
        [Fact]
        public void Test_ZetKlant_Valid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Klant klant = new Klant();
            b.ZetKlant(klant);
            Assert.Equal(klant, b.Klant);
            Assert.Contains(b, klant.GeefBestellingen());
        }
        [Fact]
        public void Test_ZetKlant_Nieuw_Valid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Klant klant1 = new Klant();
            Klant klant2 = new Klant();
            klant2.ZetKlantNr(2);
            klant1.VoegBestellingToe(b);
            b.ZetKlant(klant2);
            Assert.Equal(klant2, b.Klant);
            Assert.Contains(b, klant2.GeefBestellingen());
            Assert.DoesNotContain(b, klant1.GeefBestellingen());
        }
        [Fact]
        public void Test_ZetKlant_Invalid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Klant klant = null;
            Assert.Throws<BestellingException>(() => b.ZetKlant(klant));
        }
        [Fact]
        public void Test_ZetKlant_Nieuw_Invalid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Klant klant1 = new Klant();
            Klant klant2 = new Klant();
            b.ZetKlant(klant1);
            Assert.Throws<BestellingException>(() => b.ZetKlant(klant2));
        }
        [Fact]
        public void Test_ZetBetaald_Valid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.ZetPrijs(10);
            b.ZetBetaald();
            Assert.Equal(0, b.Prijs);
            Assert.True(b.Betaald);
        }
        [Fact]
        public void Test_ZetBetaald_Invalid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);

            Truitje t = new Truitje();
            t.ZetPrijs(10);
            b.VoegTruitjesToe(t, 2);
            b.ZetOnbetaald();
            Assert.Equal(20, b.BerekenPrijs());
            Assert.False(b.Betaald);
        }
        [Fact]
        public void Test_BerekenPrijs_Valid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Truitje t = new Truitje();
            t.ZetPrijs(10);
            Truitje t2 = new Truitje();
            t2.ZetPrijs(10);
            b.VoegTruitjesToe(t, 2);
            b.VoegTruitjesToe(t2, 2);
            Assert.Equal(40, b.BerekenPrijs());
        }
        [Fact]
        public void Test_VerwijderKlant_Valid()
        {
            Bestelling b = new Bestelling(System.DateTime.Now);
            Klant klant = new Klant();
            b.ZetKlant(klant);
            b.VerwijderKlant();
            Assert.Equal(null, b.Klant);
        }
        [Fact]
        public void Test_VoegTruitjesToe_Valid()
        {
            Truitje t = new Truitje();
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.VoegTruitjesToe(t, 5);

            Dictionary<Truitje, int> dict = new Dictionary<Truitje, int>();
            dict.Add(t, 5);

            Assert.Equal(dict, b.GetTruitjes());
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Test_VoegTruitjesToe_Invalid_FoutiefAantal(int aantal)
        {
            Truitje t = new Truitje();
            Bestelling b = new Bestelling(System.DateTime.Now);
            Assert.Throws<BestellingException>(() => b.VoegTruitjesToe(t, aantal));
            
        }
        [Fact]
        public void Test_VoegTruitjesToe_Invalid_ReedsBetaald()
        {
            Truitje t = new Truitje();

            Bestelling b = new Bestelling(System.DateTime.Now);
            b.ZetBetaald();
            Assert.Throws<BestellingException>(() => b.VoegTruitjesToe(t, 5));
        }
        [Fact]
        public void Test_VoegTruitjesToe_Invalid_TruitjeNull()
        {
            Truitje t = null;
            Bestelling b = new Bestelling(System.DateTime.Now);
            Assert.Throws<BestellingException>(() => b.VoegTruitjesToe(t, 5));
        }
        [Fact]
        public void Test_VerwijderTruitjes_Valid()
        {
            Truitje t = new Truitje();
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.VoegTruitjesToe(t, 5);
            b.VerwijderTruitjes(t, 5);
            Bestelling c = new Bestelling(System.DateTime.Now);
            Assert.Equal(b.GetTruitjes(), c.GetTruitjes());
        }
        [Fact]
        public void Test_VerwijderTruitjes_Invalid_ReedsBetaald()
        {
            Truitje t = new Truitje();
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.VoegTruitjesToe(t, 5);
            b.ZetBetaald();
            Assert.Throws<BestellingException>(() => b.VerwijderTruitjes(t, 5));
        }
        [Fact]
        public void Test_VerwijderTruitjes_Invalid_TruitjeFout()
        {
            Truitje t = new Truitje();
            Truitje u = null;
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.VoegTruitjesToe(t, 5);
            b.ZetBetaald();
            Assert.Throws<BestellingException>(() => b.VerwijderTruitjes(u, 5));
        }
        [Fact]
        public void Test_VerwijderTruitjes_Invalid_FoutiefAantal()
        {
            Truitje t = new Truitje();
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.VoegTruitjesToe(t, 5);
            b.ZetBetaald();
            Assert.Throws<BestellingException>(() => b.VerwijderTruitjes(t, -1));
        }
        [Fact]
        public void Test_IsDezelfde_Valid()
        {
            DateTime dt = System.DateTime.Now;
            Klant klant = new Klant();
            Truitje t = new Truitje();
            t.ZetId(1);
            t.ZetPrijs(50);
            Truitje t2 = new Truitje();
            t2.ZetId(2);
            t2.ZetPrijs(30);

            Bestelling b = new Bestelling(dt);
            b.ZetBestellingNr(2);
            b.ZetKlant(klant);
            b.VoegTruitjesToe(t, 1);
            b.VoegTruitjesToe(t2, 2);
            b.ZetPrijs(b.BerekenPrijs());

            Bestelling c = new Bestelling(dt);
            c.ZetBestellingNr(2);
            c.ZetKlant(klant);
            c.VoegTruitjesToe(t, 1);
            c.VoegTruitjesToe(t2, 2);
            c.ZetPrijs(c.BerekenPrijs());

            Assert.True(b.IsDezelfde(c));
        }
        //Isdezelfde invalid tests? eentje voor elke parameter?
    }
}