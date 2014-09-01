using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class RegexTests
    {

        [TestCase ( "label DREAMS", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label DREAMS.OF.FIRE", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label DREAMS_OF_FIRE", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( "label 'Any text goes here'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label \"Any text goes here\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label invalid.naming", Grammar.LabelDefaultLineRegex, Result = false )]
        public bool RegexTest_LabelDefaultClause(string line, string regex )
        {
            return Regex.IsMatch ( line, regex );
        }

    }
}
