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
    public class UnitTestKlant
    {
        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void Test_ZetKlantNr_Valid(int klantNr)
        {
            //arrange
           Klant klant = new Klant();
            //act
            klant.ZetKlantNr(klantNr);
            //assert
            Assert.Equal(klantNr, klant.KlantNr);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(0.5)]
        public void Test_ZetKlantNr_Invalid(int klantNr)
        {
            Klant klant = new Klant();
            Assert.Throws<KlantException>(() => klant.ZetKlantNr(klantNr));
        }
        [Theory]
        [InlineData("testnaam")]
        [InlineData("test naam2")]
        public void Test_ZetNaam_Valid(string naam)
        {
            Klant klant = new Klant();
            klant.ZetNaam(naam);
            Assert.Equal(naam, klant.Naam);
        }
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        public void Test_ZetNaam_Invalid(string naam)
        {
            Klant klant = new Klant();
            Assert.Throws<KlantException>(() => klant.ZetNaam(naam));
        }
        [Theory]
        [InlineData("testadres")]
        [InlineData("test adres2")]
        public void Test_Adres_Valid(string adres)
        {
            Klant klant = new Klant();
            klant.ZetAdres(adres);
            Assert.Equal(adres, klant.Adres);
        }
        [Theory]
        [InlineData("")]
        [InlineData("test")]
        public void Test_Adres_Invalid(string adres)
        {
            Klant klant = new Klant();
            Assert.Throws<KlantException>(() => klant.ZetAdres(adres));
        }

        [Fact]
        public void Test_Korting_Valid_0()
        {
            Klant klant = new Klant();
            Assert.Equal(0, klant.Korting());
        }
        [Fact]
        public void Test_Korting_Valid_10()
        {
            Klant klant = new Klant();
            Bestelling a = new Bestelling(System.DateTime.Now);
            a.ZetBestellingNr(1);
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.ZetBestellingNr(2);
            Bestelling c = new Bestelling(System.DateTime.Now);
            c.ZetBestellingNr(3);
            Bestelling d = new Bestelling(System.DateTime.Now);
            d.ZetBestellingNr(4);
            Bestelling e = new Bestelling(System.DateTime.Now);
            e.ZetBestellingNr(5);
            klant.VoegBestellingToe(a);
            klant.VoegBestellingToe(b);
            klant.VoegBestellingToe(c);
            klant.VoegBestellingToe(d);
            klant.VoegBestellingToe(e);
            Assert.Equal(10, klant.Korting());
        }
        [Fact]
        public void Test_Korting_Valid_20()
        {
            Klant klant = new Klant();
            Bestelling a = new Bestelling(System.DateTime.Now);
            a.ZetBestellingNr(1);
            Bestelling b = new Bestelling(System.DateTime.Now);
            b.ZetBestellingNr(2);
            Bestelling c = new Bestelling(System.DateTime.Now);
            c.ZetBestellingNr(3);
            Bestelling d = new Bestelling(System.DateTime.Now);
            d.ZetBestellingNr(4);
            Bestelling e = new Bestelling(System.DateTime.Now);
            e.ZetBestellingNr(5);
            Bestelling f = new Bestelling(System.DateTime.Now);
            f.ZetBestellingNr(6);
            Bestelling g = new Bestelling(System.DateTime.Now);
            g.ZetBestellingNr(7);
            Bestelling h = new Bestelling(System.DateTime.Now);
            h.ZetBestellingNr(8);
            Bestelling i = new Bestelling(System.DateTime.Now);
            i.ZetBestellingNr(9);
            Bestelling j = new Bestelling(System.DateTime.Now);
            j.ZetBestellingNr(10);
            klant.VoegBestellingToe(a);
            klant.VoegBestellingToe(b);
            klant.VoegBestellingToe(c);
            klant.VoegBestellingToe(d);
            klant.VoegBestellingToe(e);
            klant.VoegBestellingToe(f);
            klant.VoegBestellingToe(g);
            klant.VoegBestellingToe(h);
            klant.VoegBestellingToe(i);
            klant.VoegBestellingToe(j);
            Assert.Equal(20, klant.Korting());
        }
        [Fact]
        public void Test_VoegBestellingToe_Valid()
        {
            Klant klant = new Klant();
            Bestelling b = new Bestelling(System.DateTime.Now);
            klant.VoegBestellingToe(b);
        }
        [Fact]
        public void Test_VoegBestellingToe_Valid_AndereKlant()
        {
            Klant klant = new Klant();
            klant.ZetKlantNr(1);
            Bestelling b = new Bestelling(System.DateTime.Now);
            klant.VoegBestellingToe(b);

            Klant klant2 = new Klant();
            klant2.ZetKlantNr(2);
            klant2.VoegBestellingToe(b);
            Assert.Contains(b, klant2.GeefBestellingen());
            Assert.DoesNotContain(b, klant.GeefBestellingen());

        }
        [Fact]
        public void Test_VoegBestellingToe_Invalid_BestellingNull()
        {
            Klant klant = new Klant();
            Bestelling b = null;
            Assert.Throws<KlantException>(() => klant.VoegBestellingToe(b));
        }
        [Fact]
        public void Test_VoegBestellingToe_Invalid_BestellingZelfde()
        {
            Klant klant = new Klant();
            Bestelling b = new Bestelling(System.DateTime.Now);
            klant.VoegBestellingToe(b);
            Assert.Throws<KlantException>(() => klant.VoegBestellingToe(b));
        }
        [Fact]
        public void Test_VerwijderBestelling_Valid()
        {
            Klant klant = new Klant();
            Bestelling b = new Bestelling(System.DateTime.Now);
            klant.VoegBestellingToe(b);
            klant.VerwijderBestelling(b);
            Assert.DoesNotContain(b, klant.GeefBestellingen());
        }
        [Fact]
        public void Test_VerwijderBestelling_Invalid_BestellingNull()
        {
            Klant klant = new Klant();
            Bestelling b = new Bestelling(System.DateTime.Now);
            klant.VoegBestellingToe(b);
            b = null;
            Assert.Throws<KlantException>( () => klant.VerwijderBestelling(b));
        }
        [Fact]
        public void Test_VerwijderBestelling_Invalid_BestaatNiet()
        {
            Klant klant = new Klant();
            Bestelling b = new Bestelling(System.DateTime.Now);
            Assert.Throws<KlantException>(() => klant.VerwijderBestelling(b));
        }
        [Fact]
        public void Test_IsDezelfde_Valid()
        {
            Klant klant1 = new Klant(1, "testklant", "testadres");
            Klant klant2 = new Klant(1, "testklant", "testadres");
            Assert.True(klant1.IsDezelfde(klant2));
        }
        [Fact]
        public void Test_IsDezelfde_Invalid_KlantNr()
        {
            Klant klant1 = new Klant(1, "testklant", "testadres");
            Klant klant2 = new Klant(2, "testklant", "testadres");
            Assert.False(klant1.IsDezelfde(klant2));
        }
        [Fact]
        public void Test_IsDezelfde_Invalid_Naam()
        {
            Klant klant1 = new Klant(1, "testklant", "testadres");
            Klant klant2 = new Klant(1, "andereNaam", "testadres");
            Assert.False(klant1.IsDezelfde(klant2));
        }
        [Fact]
        public void Test_IsDezelfde_Invalid_Adres()
        {
            Klant klant1 = new Klant(1, "testklant", "testadres");
            Klant klant2 = new Klant(1, "testklant", "anderAdres");
            Assert.False(klant1.IsDezelfde(klant2));
        }
    }
}
