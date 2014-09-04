using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using NUnit.Framework;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class DefaultClauseTests
    {

        /// <summary> 
        /// This test ensures that when setting a value to an instance
        /// of DefaultClause the regular expression that validates
        /// the value assigned works.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="regex"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        [TestCase ( "anyname", Grammar.PositiveIntegerValueRegex, 26, Result = true )]
        [TestCase ( "anyname", Grammar.PositiveIntegerValueRegex, "u", Result = false )]
        [TestCase ( "anyname", Grammar.IntegerValueRegex, "u", Result = false )]
        [TestCase ( "anyname", Grammar.IntegerValueRegex, 0, Result = true )]
        [TestCase ( "anyname", Grammar.IntegerValueRegex, -26, Result = true )]
        [TestCase ( "anyname", Grammar.BooleanValueRegex, "true", Result = true )]
        [TestCase ( "anyname", Grammar.BooleanValueRegex, "false", Result = true )]
        [TestCase ( "anyname", Grammar.IntegerValueRegex, "false", Result = false )]
        public bool DefaultClauseSetValueWorks ( string name, string regex, object o )
        {
            var def1 = new DefaultClause ( name, regex );
            try
            {
                return def1.SetValue ( o );
            }
            catch
            {
                return false;
            }
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "{\"Country\":\"ES\"}", 0, "\"Country\"", "\"ES\"", -1, null, null, Result = 1 )]
        [TestCase ( "{\"Country\":\"ES\",\"Active\":true, \"Age\":30, \"Title\":\"No reason to fear this\" }", 1, "\"Active\"", "true", 2, "\"Age\"", "30", Result = 4 )]
        public int AddArgumentsFromStringTests ( string argLine, int index1, string key1, string value1, int index2, string key2, string value2 )
        {

            var defC = new DefaultClause ( "", "", null ); // don't like this.... redesign this class
            defC.AddArgumentsFromString ( argLine );

            if ( index1 >= 0 )
            {
                Assert.That ( defC.Arguments.First ().Key == key1 );
                Assert.That ( defC.Arguments.First ().Value == value1 );
            }
            if ( index2 >= 0 )
            {
                Assert.That ( defC.Arguments.ElementAt ( 1 ).Key == key2 );
                Assert.That ( defC.Arguments.ElementAt ( 1 ).Value == value2 );
            }

            return defC.Arguments.Count ();

        }


        // ---------------------------------------------------------------------------------

    }
}
