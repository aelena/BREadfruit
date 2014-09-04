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
            var def1 = new DefaultClause ( name, regex);
            try
            {
                return def1.SetValue ( o );
            }
            catch 
            {
                return false;
            }
        }


        public void AddArgumentsFromStringTests()
        {
        }
    }
}
