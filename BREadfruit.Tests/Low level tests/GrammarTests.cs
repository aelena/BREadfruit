using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class GrammarTests
    {

        [Test]
        public void GrammarEnumerateAllRegexes()
        {
            var props = from x in typeof ( Grammar ).GetProperties ()
                         where x.Name.ToUpperInvariant ().Contains ( "REGEX" )
                         select new
                         {
                             Name = x.Name,
                             Ex = x.GetValue(null)
                         };
            var fields = from x in typeof ( Grammar ).GetFields ()
                             where x.Name.ToUpperInvariant ().Contains ( "REGEX" )
                             select new
                             {
                                 Name = x.Name,
                                 Ex = x.GetValue ( null )
                             };

            var _s = new StringBuilder ();
            foreach ( var _x in props )
                _s.AppendFormat ( "{0} - {1}{2}", _x.Name, _x.Ex, Environment.NewLine );
            foreach ( var _x in fields )
                _s.AppendFormat ( "{0} - {1}{2}", _x.Name, _x.Ex, Environment.NewLine );

            
            Assert.That ( props.Count() > 0 );
            Assert.That ( fields.Count() > 0 );


        }

        [TestCase ( " WItH ", false, 5, false )]
        [TestCase ( "with", false, 5, true )]
        [TestCase ( "enable", true, 0, true )]
        public void ShouldFindSymbol ( string token, bool isTerminal, int childCount, bool strictMatch )
        {
            var s = Grammar.GetSymbolByToken ( token, strictMatch );
            Assert.That ( s.IsTerminal == isTerminal );
            Assert.That ( s.Children.Count () == childCount );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "max_length" )]
        public void ShouldFindDefaultClause ( string token )
        {
            var dt = Grammar.GetDefaultClauseByToken ( token, true );
            Assert.That ( dt.RegexPattern == Grammar.PositiveIntegerValueRegex );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "Max_Length", Result = null )]
        [TestCase ( "non existing", Result = null )]
        public object ShouldNotFindWrongDefaultClause ( string token )
        {
            return Grammar.GetDefaultClauseByToken ( token, true );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This series of tests validates the Regular Expressions
        /// that are part of the Grammar.
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [TestCase ( Grammar.PositiveIntegerValueRegex, 0, Result = true )]
        [TestCase ( Grammar.PositiveIntegerValueRegex, 24, Result = true )]
        [TestCase ( Grammar.PositiveIntegerValueRegex, -24, Result = false )]
        [TestCase ( Grammar.PositiveIntegerValueRegex, -1.0, Result = false )]
        [TestCase ( Grammar.PositiveIntegerValueRegex, 'a', Result = false )]
        [TestCase ( Grammar.IntegerValueRegex, 0, Result = true )]
        [TestCase ( Grammar.IntegerValueRegex, 288, Result = true )]
        [TestCase ( Grammar.IntegerValueRegex, -288, Result = true )]
        [TestCase ( Grammar.IntegerValueRegex, 2.88, Result = false )]
        [TestCase ( Grammar.IntegerValueRegex, -2.88, Result = false )]
        [TestCase ( Grammar.FloatValueRegex, -2.88d, Result = true )]
        [TestCase ( Grammar.FloatValueRegex, 2.88d, Result = true )]
        [TestCase ( Grammar.FloatValueRegex, 288d, Result = true )]
        [TestCase ( Grammar.FloatValueRegex, -0.001d, Result = true )]
        [TestCase ( Grammar.FloatValueRegex, 0.0001d, Result = true )]
        [TestCase ( Grammar.BooleanValueRegex, true, Result = true )]
        [TestCase ( Grammar.BooleanValueRegex, false, Result = true )]
        [TestCase ( Grammar.BooleanValueRegex, 1, Result = false )]
        public bool GrammarRegexsShouldWork ( string regex, ValueType value )
        {
            return Regex.IsMatch ( value.ToString (), regex, RegexOptions.IgnoreCase );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "is", "is" )]
        [TestCase ( "is empty", "is_empty" )]
        [TestCase ( "not starts", "does_not_start_with" )]
        [TestCase ( "not starts with", "does_not_start_with" )]
        [TestCase ( "does not contain", "does_not_contain" )]
        [TestCase ( "not in", "not_in" )]
        [TestCase ( "in", "in" )]
        [TestCase ( "with arguments", "with_args" )]
        [TestCase ( "only ascii", "only_ascii" )]
        [TestCase ( "ascii only", "only_ascii" )]
        [TestCase ( "only numbers", "only_numbers" )]
        [TestCase ( "numbers only", "only_numbers" )]
        [TestCase ( "only letters", "only_letters" )]
        [TestCase ( "letters only", "only_letters" )]
        public void ShouldFindSymbolByAlias ( string aliasToken, string mainAlias )
        {
            var _op = Grammar.GetSymbolByToken ( aliasToken );
            Assert.That ( _op != null );
            if ( _op.Aliases.Count () > 0 )
                Assert.That ( _op.Aliases.Contains ( aliasToken ) );
            Assert.That ( _op.Token == mainAlias );
        }


        // ---------------------------------------------------------------------------------

        [TestCase ( "is", "is" )]
        [TestCase ( "is empty", "is_empty" )]
        [TestCase ( "not starts", "does_not_start_with" )]
        [TestCase ( "not starts with", "does_not_start_with" )]
        [TestCase ( "does not contain", "does_not_contain" )]
        [TestCase ( "not in", "not_in" )]
        [TestCase ( "in", "in" )]
        public void ShouldFindOperator ( string token, string mainAlias )
        {
            var _op = Grammar.GetOperator ( token );
            Assert.That ( _op != null );
            Assert.That ( _op.Aliases.Contains ( token ) );
            Assert.That ( _op.Token == mainAlias );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "i s", "is" )]
        [TestCase ( " is empty", "is_empty" )]
        [TestCase ( "not start", "does_not_start_with" )]
        [TestCase ( "not starts woth", "does_not_start_with" )]
        public void ShouldNotFindOperatorOnWrongToken ( string token, string mainAlias )
        {
            var _op = Grammar.GetOperator ( token );
            Assert.That ( _op == null );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "on changed", Result = true )]
        [TestCase ( "changed", Result = false )]
        [TestCase ( "make enabled", Result = true )]
        [TestCase ( "visible", Result = false )]
        [TestCase ( "with args", Result = true )]
        [TestCase ( "with arguments", Result = true )]
        [TestCase ( "numbers only", Result = true )]
        [TestCase ( "only ascii", Result = true )]
        [TestCase ( "only_ascii", Result = false )]
        [TestCase ( "with_args", Result = false )]
        [TestCase ( null, Result = false )]
        [TestCase ( "", Result = false )]
        [TestCase ( " ", Result = false )]
        [TestCase ( "   ", Result = false )]
        public bool ShouldKnowIfTokenIsAlias ( string alias )
        {
            return Grammar.TokenIsAlias ( alias );
        }

        // ---------------------------------------------------------------------------------


        [Test]
        public void ConstraintSymbolsTests ()
        {

            Assert.That ( Grammar.ConstraintSymbols.Count () == 3 );
            Assert.That ( Grammar.ConstraintLineRegex.Equals ( @"^\t{0,2}(only_ascii|only_numbers|only_letters)$" ) );

        }

        // ---------------------------------------------------------------------------------


        [Test]
        public void GrammarShouldHaveCorrectNumberOfTriggerSymbols ()
        {
            Assert.That ( Grammar.TriggerSymbols.Count () == 8 );
        }


        // ---------------------------------------------------------------------------------

    }
}
