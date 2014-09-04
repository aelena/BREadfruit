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
    public class WithLineTests
    {

        LineParser lineParser = new LineParser ();


        [TestCase ( "\twith defaults", 1, Result = true )]
        [TestCase ( "\twith triggers", 1, Result = true )]
        [TestCase ( "\twith rules", 1, Result = true )]
        [TestCase ( "\twith constraints", 1, Result = true )]
        /*
         * even when passing 0 as the indent level for this symbol, which 
         * is wrong according to the grammar, this test passes as the
         * indentLevel is recalculated on constructing the LineInfo instance.
         */
        [TestCase ( "\twith constraints", 0, Result = true )]
        [TestCase ( "with constraints", 1, Result = false )]
        // TODO: Check for exceptions here and put something to indicate where symbols that are invalid according to the grammar are found
        //[TestCase ( "\twith booz", 1, Result = false )]
        public bool ValidateWithLineInfosCorrectly ( string representation, int indentLevel )
        {
            var li = new LineInfo ( representation );
            return lineParser.IsAValidSentence ( li );
        }
    }
}
