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

        [TestCase ( "anyname", Grammar.PositiveIntegerRegex, 26, Result = true )]
        [TestCase ( "anyname", Grammar.PositiveIntegerRegex, "u", Result = false )]
        [TestCase ( "anyname", Grammar.IntegerRegex, "u", Result = false )]
        [TestCase ( "anyname", Grammar.IntegerRegex, 0, Result = true )]
        [TestCase ( "anyname", Grammar.IntegerRegex, -26, Result = true )]
        [TestCase ( "anyname", Grammar.BooleanRegex, "true", Result = true )]
        [TestCase ( "anyname", Grammar.BooleanRegex, "false", Result = true )]
        [TestCase ( "anyname", Grammar.IntegerRegex, "false", Result = false )]
        public bool DefaultClauseSetValueWorks (string name, string regex, object o)
        {
            var def1 = new DefaultClause ( name, regex);
            return def1.SetValue ( o );
        }

    }
}
