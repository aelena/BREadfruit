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
        [TestCase ( "label DREAMS_OF_FIRE", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label 'Any text goes here'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label \"Any text goes here\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label invalid.naming", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "DREAMS", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "DREAMS.OF.FIRE", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "DREAMS_OF_FIRE", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "'Any text goes here'", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "\"Any text goes here\"", Grammar.LabelDefaultValueRegex, Result = true )]
        [TestCase ( "invalid.naming", Grammar.LabelDefaultValueRegex, Result = true )]
        public bool RegexTest_LabelDefaultClause(string line, string regex )
        {
            return Regex.IsMatch ( line.ToUpperInvariant(), regex );
        }


        // ---------------------------------------------------------------------------------


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


        // ---------------------------------------------------------------------------------


        [TestCase ( "show this", Grammar.ShowElementLineRegex, Result =true )]
        [TestCase ( "show NAME_OF_DIV", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( "show this      ", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( "show NAME_OF_DIV   ", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( " show NAME_OF_DIV   ", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( " show", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( " show", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( "show", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( "show 'dudud'", Grammar.ShowElementLineRegex, Result = false )]
        public bool ShouldDiscriminateShowElementsLineCorrectly( string line, string regex)
        {
             return Regex.IsMatch ( line, regex, RegexOptions.IgnoreCase );
        }


        // ---------------------------------------------------------------------------------



        [TestCase ( "hide this", Grammar.HideElementLineRegex, Result = true )]
        [TestCase ( "hide NAME_OF_DIV", Grammar.HideElementLineRegex, Result = true )]
        [TestCase ( "hide this      ", Grammar.HideElementLineRegex, Result = true )]
        [TestCase ( "hide NAME_OF_DIV   ", Grammar.HideElementLineRegex, Result = true )]
        [TestCase ( " hide NAME_OF_DIV   ", Grammar.HideElementLineRegex, Result = false )]
        [TestCase ( " hide", Grammar.HideElementLineRegex, Result = false )]
        [TestCase ( " hide", Grammar.HideElementLineRegex, Result = false )]
        [TestCase ( "hide", Grammar.HideElementLineRegex, Result = false )]
        [TestCase ( "hide 'dudud'", Grammar.HideElementLineRegex, Result = false )]
        public bool ShouldDiscriminateHideElementsLineCorrectly ( string line, string regex )
        {
            return Regex.IsMatch ( line, regex, RegexOptions.IgnoreCase );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "CHANGE_FORM_TO fmrSearch", Result = true )]
        [TestCase ( "CHANGE_FORM_TO 'fmrSearch'", Result = true )]
        [TestCase ( "CHANGE_FORM_TO \"fmrSearch\"", Result = true )]
        [TestCase ( "CHANGE_FORM_TO     \"fmrSearch\"", Result = true )]
        [TestCase ( "CHANGE_FORM_TO     \"fmrSearch\"   ", Result = true )]
        // allow for identifiers with spaces in them when quoted
        [TestCase ( "CHANGE_FORM_TO \"Vendor Search Screen\"", Result = true )]
        [TestCase ( "CHANGE_FORM_TO 'Vendor Search Screen'", Result = true )]
        [TestCase ( "CHANGE_FORM_TO Vendor Search Screen", Result = false )]
        [TestCase ( "CHANGE_FORM_TO Vendor Search Screen    ", Result = false )]
        public bool ChangeFormNoArgumentsLineRegexTests ( string line )
        {
            return Regex.IsMatch ( line, Grammar.ChangeFormNoArgumentsLineRegex, RegexOptions.IgnoreCase );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "WITH_ARGUMENTS {\"A\":\"los cojones 33\"}", Result = true )]
        [TestCase ( "WITH_ARGUMENTS     {\"A\":\"los cojones 33\"}", Result = true )]
        [TestCase ( "WITH_ARGUMENTS     {\"A\"  : \"los cojones 33\"}  ", Result = true )]
        [TestCase ( "WITH_ARGUMENTS {'A':\"los cojones 33\"}", Result = true )]
        [TestCase ( "WITH_ARGUMENTS {'A':'los cojones 33'}", Result = true )]
        [TestCase ( "WITH_ARGUMENTS {'xsxsA':'sxsxs  )(//(& nes 33'}", Result = true )]
        [TestCase ( "WITH_ARGUMENTS {xsxsA:'sxsxs  )(//(& nes 33'}", Result = false )]
        [TestCase ( "WITH_ARGUMENTS {xsxsA:sxsxs  )(//(& nes 33'}", Result = false )]
        [TestCase ( "WITH_ARGUMENTS {\"A\":\"los cojones 33\", \"B2\" : \"another val\"}", Result = true )]
        [TestCase ( "WITH_ARGUMENTS {\"A\":\"los cojones 33\", \"B2\" : \"another val\", \"CCC\": \"soixuosi\"}", Result = true )]
        public bool WithArgumentsClauseRegexTests ( string line )
        {
            return Regex.IsMatch ( line, Grammar.WithArgumentsClauseLineRegex, RegexOptions.IgnoreCase );
        }


    }
}
