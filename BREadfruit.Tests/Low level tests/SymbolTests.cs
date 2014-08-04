using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class SymbolTests
    {

        [TestCase ( "test", 0, true )]
        [TestCase ( "test", 1, true )]
        public void SymbolShouldConstructCorrectly ( string token, int indentlevel, bool isTerminal )
        {
            var s = new Symbol ( token, indentlevel, isTerminal );
            Assert.That ( s.IndentLevel == indentlevel );
            Assert.That ( s.IsTerminal == isTerminal );
            Assert.That ( s.Token == token );
            Assert.That ( s.Children.Count () == 0 );
            Assert.That ( s.ChildTokens.Count () == 0 );
        }


        [TestCase ( "s1", 0, true, "s1", 0, true, Result = true )]
        [TestCase ( "s1", 0, true, "s1", 0, false, Result = false )]
        [TestCase ( "s1", 0, true, "s2", 0, true, Result = false )]
        [TestCase ( "s1", 1, true, "s1", 0, true, Result = false )]
        [TestCase ( "s1", 0, true, "s2", 1, true, Result = false )]
        public bool SymbolOverrideOfEqualsTest ( string symbol1, int indentlevel1, bool isTerminal1,
                                                 string symbol2, int indentlevel2, bool isTerminal2 )
        {
            var s1 = new Symbol ( symbol1, indentlevel1, isTerminal1 );
            var s2 = new Symbol ( symbol2, indentlevel2, isTerminal2 );
            return ( s1.Equals ( s2 ) );

            // not the same as overriding == 
            // which could be convenient as this is a widely used type
        }


        [Test]
        public void ShouldDetectSymbolsWithDifferentChildCount ()
        {
            var s1 = new Symbol ( "s1", 1, false );
            s1.AddValidChild ( Grammar.EntitySymbol );
            s1.AddValidChild ( Grammar.WithSymbol );

            var s2 = new Symbol ( "s1", 1, false);
            s2.AddValidChild ( Grammar.EntitySymbol );

            Assert.That ( !( s1.Equals ( s2 ) ) );
            
        }


        [Test]
        public void ShouldDetectSymbolsWithDifferentChildren ()
        {
            var s1 = new Symbol ( "s1", 1, false );
            s1.AddValidChild ( Grammar.EntitySymbol );
            s1.AddValidChild ( Grammar.WithSymbol );

            var s2 = new Symbol ( "s1", 1, false );
            s2.AddValidChild ( Grammar.EntitySymbol );
            s2.AddValidChild ( s1 );

            Assert.That ( !( s1.Equals ( s2 ) ) );

        }

        [Test]
        public void ShouldAcceptIdenticalSymbolInstances ()
        {
            var s1 = new Symbol ( "s1", 1, false );
            s1.AddValidChild ( Grammar.EntitySymbol );
            s1.AddValidChild ( Grammar.WithSymbol );

            var s2 = new Symbol ( "s1", 1, false );
            s2.AddValidChild ( Grammar.EntitySymbol );
            s2.AddValidChild ( Grammar.WithSymbol );

            Assert.That ( ( s1.Equals ( s2 ) ) );

        }

    }
}
