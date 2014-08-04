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

        [TestCase ( " WItH ", false, 4, false )]
        [TestCase ( "with", false, 4, true )]
        [TestCase ("enable", true, 2, true)]
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
            Assert.That ( dt.RegexPattern == Grammar.PositiveIntegerRegex );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "Max_Length", Result = null )]
        [TestCase ( "non existing", Result = null )]
        public object ShouldNotFindWrongDefaultClause ( string token )
        {
            return Grammar.GetDefaultClauseByToken ( token, true );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ShouldHaveEntityRegex ()
        {
            Assert.That ( Grammar.EntityLineRegex.Contains ( "DYNAMIC" ) );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This series of tests validates the Regular Expressions
        /// that are part of the Grammar.
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [TestCase ( Grammar.PositiveIntegerRegex, 0, Result = true )]
        [TestCase ( Grammar.PositiveIntegerRegex, 24, Result = true )]
        [TestCase ( Grammar.PositiveIntegerRegex, -24, Result = false )]
        [TestCase ( Grammar.PositiveIntegerRegex, -1.0, Result = false )]
        [TestCase ( Grammar.PositiveIntegerRegex, 'a', Result = false )]
        [TestCase ( Grammar.IntegerRegex, 0, Result = true )]
        [TestCase ( Grammar.IntegerRegex, 288, Result = true )]
        [TestCase ( Grammar.IntegerRegex, -288, Result = true )]
        [TestCase ( Grammar.IntegerRegex, 2.88, Result = false )]
        [TestCase ( Grammar.IntegerRegex, -2.88, Result = false )]
        [TestCase ( Grammar.FloatRegex, -2.88d, Result = true )]
        [TestCase ( Grammar.FloatRegex, 2.88d, Result = true )]
        [TestCase ( Grammar.FloatRegex, 288d, Result = true )]
        [TestCase ( Grammar.FloatRegex, -0.001d, Result = true )]
        [TestCase ( Grammar.FloatRegex, 0.0001d, Result = true )]
        [TestCase ( Grammar.BooleanRegex, true, Result = true )]
        [TestCase ( Grammar.BooleanRegex, false, Result = true )]
        [TestCase ( Grammar.BooleanRegex, 1, Result = false )]
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
        public void ShouldFindOperator ( string token, string mainAlias )
        {
            var _op = Grammar.GetOperator ( token );
            Assert.That ( _op != null );
            Assert.That ( _op.Aliases.Contains ( token ) );
            Assert.That ( _op.Token == mainAlias );
        }

        [TestCase ( "i s", "is" )]
        [TestCase ( " is empty", "is_empty" )]
        [TestCase ( "not start", "does_not_start_with" )]
        [TestCase ( "not starts woth", "does_not_start_with" )]
        public void ShouldNotFindOperatorOnWrongToken ( string token, string mainAlias )
        {
            var _op = Grammar.GetOperator ( token );
            Assert.That ( _op == null );
        }

    }
}
