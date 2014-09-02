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
        [TestCase ( "DREAMS", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "DREAMS.OF.FIRE", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "DREAMS_OF_FIRE", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "'Any text goes here'", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "\"Any text goes here\"", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "invalid.naming", Grammar.LabelDefaultValueRegex, Result = false )]
        public bool RegexTest_LabelDefaultClause(string line, string regex )
        {
            return Regex.IsMatch ( line, regex );
        }

        [TestCase ( "load data from DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load data from WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load data from DATAsourRCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = false )]
        [TestCase ( "load data from webservice.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = false )]
        [TestCase ( "load data from DATASOURCE.S", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load data from WEBSERVICE.S", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromValueRegex, Result = true )]
        [TestCase ( "WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromValueRegex, Result = true )]
        [TestCase ( "DATASOURCE.SXXSX.XASSS.XXSSSXX.", Grammar.LoadDataFromValueRegex, Result = false )]
        [TestCase ( "WEBSERVICE.SXXSX.XASSS.XXSSSXX.", Grammar.LoadDataFromValueRegex, Result = false )]
        [TestCase ( "DATASOURCE.S", Grammar.LoadDataFromValueRegex, Result = true )]
        [TestCase ( "WEBSERVICE.S", Grammar.LoadDataFromValueRegex, Result = true )]
        [TestCase ( "DATASOURCE.", Grammar.LoadDataFromValueRegex, Result = false )]
        [TestCase ( "WEBSERVICE.", Grammar.LoadDataFromValueRegex, Result = false )]
        [TestCase ( "DATASOURCE", Grammar.LoadDataFromValueRegex, Result = false )]
        [TestCase ( "WEBSERVICE", Grammar.LoadDataFromValueRegex, Result = false )]
        public bool RegexTest_LoadDataFromClause(string line, string regex)
        {
            return Regex.IsMatch ( line, regex );
        }
    }
}
