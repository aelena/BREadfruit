using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class LineParserTests
    {
        [TestCase ( "max_length 10", 2, "10", Result = true )]
        [TestCase ( "max_length 11", 2, "10", Result = false )]
        [TestCase ( "max_length 11 0", 2, "10", Result = false )]
        [TestCase ( "max_length 10 ; this is a comment", 2, "10", Result = true )]
        [TestCase ( "max_length 12 ; this is a comment", 2, "10", Result = false )]
        [TestCase ( "mandatory true", 2, "true", Result = true )]
        [TestCase ( "mandatory", 1, "mandatory", Result = true )]
        [TestCase ( "mandatory ; this is a comment", 1, "mandatory", Result = true )]
        [TestCase ( "Entity XYZ is TextBox", 4, "TextBox", Result = true )]
        [TestCase ( "Entity XYZ is TextBox ; and this is a comment", 4, "TextBox", Result = true )]
        public bool ShouldExtractTokensCorrectly ( string line, int correctCount, string lastToken )
        {
            var _t = LineParser.ExtractTokens ( line );
            return ( _t.Count () == correctCount ) && ( _t.Last ().Token == lastToken );
        }
    }
}
