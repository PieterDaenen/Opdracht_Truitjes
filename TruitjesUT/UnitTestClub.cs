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
    public class UnitTestClub
    {
        [Fact]
        public void Test_ZetCompetitie_Valid()
        {
            //arrange
            Club club = new Club();
            //act
            club.ZetCompetitie("testcompetitie");
            //assert
            Assert.Equal("testcompetitie", club.Competitie);
        }
        [Theory]
        [InlineData("testcompetitie")]
        [InlineData("")]
        public void Test_ZetCompetitie_Invalid(string competitie)
        {
            Club club = new Club("testcompetitie", "testnaam");
            Assert.Throws<ClubException>(() => club.ZetCompetitie(competitie));
        }
        [Fact]
        public void Test_ZetPloegnaam_Valid()
        {
            //arrange
            Club club = new Club();
            //act
            club.ZetPloegnaam("testnaam");
            //assert
            Assert.Equal("testnaam", club.Ploegnaam);
        }
        [Theory]
        [InlineData("testnaam")]
        [InlineData("")]
        public void Test_ZetPloegnaam_Invalid(string naam)
        {
            Club club = new Club("testcompetitie", "testnaam");
            Assert.Throws<ClubException>(() => club.ZetPloegnaam(naam));
        }
    }
}
