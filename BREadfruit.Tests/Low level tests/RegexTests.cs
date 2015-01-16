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
        public bool RegexTest_LabelDefaultClause ( string line, string regex )
        {
            return Regex.IsMatch ( line.ToUpperInvariant (), regex );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "load_data_from DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load_data_from WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load_data_from     WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load_data_from     WEBSERVICE.SXXSX    ", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load_data_from       WEBSERVICE.SXXSX  ", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load_data_from DATAsourRCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = false )]
        [TestCase ( "load_data_from webservice.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataFromLineRegex, Result = false )]
        [TestCase ( "load_data_from     DATASOURCE.S", Grammar.LoadDataFromLineRegex, Result = true )]
        [TestCase ( "load_data_from WEBSERVICE.S", Grammar.LoadDataFromLineRegex, Result = true )]
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
        public bool RegexTest_LoadDataFromClause ( string line, string regex )
        {
            return Regex.IsMatch ( line, regex );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "show this", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( "show NAME_OF_DIV", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( "show this      ", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( "show NAME_OF_DIV   ", Grammar.ShowElementLineRegex, Result = true )]
        [TestCase ( " show NAME_OF_DIV   ", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( " show", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( " show", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( "show", Grammar.ShowElementLineRegex, Result = false )]
        [TestCase ( "show 'dudud'", Grammar.ShowElementLineRegex, Result = false )]
        public bool ShouldDiscriminateShowElementsLineCorrectly ( string line, string regex )
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


        [TestCase ( "CHANGE_TO fmrSearch", Result = true )]
        [TestCase ( "CHANGE_TO 'fmrSearch'", Result = true )]
        [TestCase ( "CHANGE_TO \"fmrSearch\"", Result = true )]
        [TestCase ( "CHANGE_TO     \"fmrSearch\"", Result = true )]
        [TestCase ( "CHANGE_TO     \"fmrSearch\"   ", Result = true )]
        // allow for identifiers with spaces in them when quoted
        [TestCase ( "CHANGE_TO \"Vendor Search Screen\"", Result = true )]
        [TestCase ( "CHANGE_TO 'Vendor Search Screen'", Result = true )]
        [TestCase ( "CHANGE_TO Vendor Search Screen", Result = false )]
        [TestCase ( "CHANGE_TO Vendor Search Screen    ", Result = false )]
        public bool ChangeFormNoArgumentsLineRegexTests ( string line )
        {
            return Regex.IsMatch ( line, Grammar.ChangeFormNoArgumentsLineRegex, RegexOptions.IgnoreCase );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "WITH_ARGS {\"A\":\"los cojones 33\"}", Result = true )]
        [TestCase ( "WITH_ARGS     {\"A\":\"los cojones 33\"}", Result = true )]
        [TestCase ( "WITH_ARGS     {\"A\"  : \"los cojones 33\"}  ", Result = true )]
        [TestCase ( "WITH_ARGS {'A':\"los cojones 33\"}", Result = true )]
        [TestCase ( "WITH_ARGS {'A':'los cojones 33'}", Result = true )]
        [TestCase ( "WITH_ARGS {'xsxsA':'sxsxs  )(//(& nes 33'}", Result = true )]
        [TestCase ( "WITH_ARGS {xsxsA:'sxsxs  )(//(& nes 33'}", Result = false )]
        [TestCase ( "WITH_ARGS {xsxsA:sxsxs  )(//(& nes 33'}", Result = false )]
        [TestCase ( "WITH_ARGS {\"A\":\"los cojones 33\", \"B2\" : \"another val\"}", Result = true )]
        [TestCase ( "WITH_ARGS {\"A\":\"los cojones 33\", \"B2\" : \"another val\", \"CCC\": \"soixuosi\"}", Result = true )]
        public bool WithArgumentsClauseRegexTests ( string line )
        {
            return Regex.IsMatch ( line, Grammar.WithArgumentsClauseLineRegex, RegexOptions.IgnoreCase );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "substring ( ANYTHING.GOES, 4 )", Result = true )]
        [TestCase ( "substring ( ANY566THING.GOES, 4 )", Result = true )]
        [TestCase ( "substring ( ANYTHING.GOES, 4 ) ", Result = true )]
        [TestCase ( "substring   (  ANYTHING.GOES, 4 ) ", Result = true )]
        [TestCase ( "substring ( ANYTHING.GOES_HERE, 4 )", Result = true )]
        [TestCase ( "substring ( ANYTHING.GOES.HERE_IS_ME, 14 ) ", Result = true )]
        [TestCase ( "substring   (  \"ANYTHING.GOES\", 4 ) ", Result = true )]
        [TestCase ( "substring ( '79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\"', 4 ) ", Result = true )]
        [TestCase ( "substring ( 79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\", 4 ) ", Result = false )]

		[TestCase ( "substring(ANYTHING.GOES,4)", Result = true )]
		[TestCase ( "substring(ANY566THING.GOES,4)", Result = true )]
		[TestCase ( "substring(ANYTHING.GOES,4) ", Result = true )]
		[TestCase ( "substring(ANYTHING.GOES_HERE,4)", Result = true )]
		[TestCase ( "substring(ANYTHING.GOES.HERE_IS_ME,14) ", Result = true )]
		[TestCase ( "substring(\"ANYTHING.GOES\",4) ", Result = true )]
		[TestCase ( "substring('79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\"',4) ", Result = true )]
		[TestCase ( "substring(79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\",4) ", Result = false )]

		[TestCase ( "substring(ANYTHING.GOES,4", Result = false )]
		[TestCase ( "substring(ANYTHING.GOES,)", Result = false )]
		[TestCase ( "substring(,4)", Result = false )]
		[TestCase ( "substring(,)", Result = false )]
		[TestCase ( "substring()", Result = false )]


        public bool SubStringExpressionRegexTests ( string line )
        {
            return Regex.IsMatch ( line, Grammar.SubStringExpressionRegex, RegexOptions.IgnoreCase );

        }


        // ---------------------------------------------------------------------------------


		[TestCase ( "ToUpper ( ANYTHING.GOES)", Result = true )]
		[TestCase ( "ToUpper ( ANY566THING.GOES )", Result = true )]
		[TestCase ( "ToUpper ( ANYTHING.GOES ) ", Result = true )]
		[TestCase ( "ToUpper   (  ANYTHING.GOES) ", Result = true )]
		[TestCase ( "ToUpper ( ANYTHING.GOES_HERE )", Result = true )]
		[TestCase ( "ToUpper ( ANYTHING.GOES.HERE_IS_ME ) ", Result = true )]
		[TestCase ( "ToUpper   (  \"ANYTHING.GOES\" ) ", Result = true )]
		[TestCase ( "ToUpper ( '79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\"') ", Result = true )]
		[TestCase ( "ToUpper ( 79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\" ) ", Result = false )]

		[TestCase ( "toupper(ANYTHING.GOES)", Result = true )]
		[TestCase ( "toupper(ANY566THING.GOES)", Result = true )]
		[TestCase ( "toupper(ANYTHING.GOES) ", Result = true )]
		[TestCase ( "toupper(ANYTHING.GOES_HERE)", Result = true )]
		[TestCase ( "toupper(ANYTHING.GOES.HERE_IS_ME) ", Result = true )]
		[TestCase ( "toupper(\"ANYTHING.GOES\") ", Result = true )]
		[TestCase ( "toupper('79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\"') ", Result = true )]
		[TestCase ( "toupper(79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\") ", Result = false )]

		[TestCase ( "ToUpper ( ANYTHING.GOES, 3)", Result = false )]
		[TestCase ( "ToUpper ( ANYTHING.GOES", Result = false )]
		[TestCase ( "ToUpperANYTHING.GOES", Result = false )]
		[TestCase ( "ToUpper ANYTHING.GOES", Result = false )]
		[TestCase ( "ToUpper()", Result = false )]

		public bool ToUpperExpressionRegexTests ( string line )
		{
			return Regex.IsMatch ( line, Grammar.ToUpperExpressionRegex, RegexOptions.IgnoreCase );

		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "ToLower ( ANYTHING.GOES)", Result = true )]
		[TestCase ( "ToLower ( ANY566THING.GOES )", Result = true )]
		[TestCase ( "ToLower ( ANYTHING.GOES ) ", Result = true )]
		[TestCase ( "ToLower   (  ANYTHING.GOES) ", Result = true )]
		[TestCase ( "ToLower ( ANYTHING.GOES_HERE )", Result = true )]
		[TestCase ( "ToLower ( ANYTHING.GOES.HERE_IS_ME ) ", Result = true )]
		[TestCase ( "ToLower   (  \"ANYTHING.GOES\" ) ", Result = true )]
		[TestCase ( "ToLower ( '79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\"') ", Result = true )]
		[TestCase ( "ToLower ( 79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\" ) ", Result = false )]

		[TestCase ( "tolower(ANYTHING.GOES)", Result = true )]
		[TestCase ( "tolower(ANY566THING.GOES)", Result = true )]
		[TestCase ( "tolower(ANYTHING.GOES) ", Result = true )]
		[TestCase ( "tolower(ANYTHING.GOES_HERE)", Result = true )]
		[TestCase ( "tolower(ANYTHING.GOES.HERE_IS_ME) ", Result = true )]
		[TestCase ( "tolower(\"ANYTHING.GOES\") ", Result = true )]
		[TestCase ( "tolower('79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\"') ", Result = true )]
		[TestCase ( "tolower(79842398/)(/)(//&(&(/&(/&/(\"!\"·!·\") ", Result = false )]
		[TestCase ( "ToLower ( ANYTHING.GOES, 3)", Result = false )]
		[TestCase ( "ToLower ( ANYTHING.GOES", Result = false )]
		[TestCase ( "ToLowerANYTHING.GOES", Result = false )]
		[TestCase ( "ToLower ANYTHING.GOES", Result = false )]
		[TestCase ( "ToLower()", Result = false )]

		public bool ToLowerExpressionRegexTests ( string line )
		{
			return Regex.IsMatch ( line, Grammar.ToLowerExpressionRegex, RegexOptions.IgnoreCase );
		}


		// ---------------------------------------------------------------------------------

        // TODO: PENDING TESTS FOR 
        // ^save data to[\t\s]+(DATASOURCE|WEBSERVICE)(\.){1}[A-Za-z0-9_-]+[\t\s]+with arguments[\t\s]*{{1}[\t\s]*((\"|'){1}[A-Za-z0-9_-]+(\"|'){1}){1}[\t\s]*:[\t\s]*((\"|'){1}.+(\"|'){1}){1}[\t\s]*}{1}[\t\s]*$
        // for example
        // save data to DATASOURCE.MDM_Requests_PhoneEntry with arguments {"Phone":"123$·$·$·456"}
        // and equivalent rule to detect those same instances which are neither "with argumetns" or between { and }


        [TestCase ( "save data to DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "SAVE DATA TO DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "save data to WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "save data to WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {   \"Phone\":  \"123$·$·$·456\"}", Result = true )]
        [TestCase ( "SAVE DATA TO DATASOURCE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "save data to   WEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "SAVE DATA TOWEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "SAVE DATA TO WEBSERVICEMDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "SAVE DATA TO WEBSERVICEMDM_Requests_PhoneEntry with arguments 'Phone':'123$·$·$·456'", Result = false )]
        [TestCase ( "save data to DATASOURCE.MDM_Requests_PhoneEntry with arguments \"Phone\":\"123$·$·$·456\"", Result = false )]

        [TestCase ( "save_data_to DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "SAVE_DATA_TO DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "save_data_to WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "save_data_to WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {   \"Phone\":  \"123$·$·$·456\"}", Result = true )]
        [TestCase ( "SAVE_DATA_TO DATASOURCE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "save_data_to   WEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "SAVE_DATA_TOWEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "SAVE_DATA_TO WEBSERVICEMDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "SAVE_DATA_TO WEBSERVICEMDM_Requests_PhoneEntry with arguments 'Phone':'123$·$·$·456'", Result = false )]
        [TestCase ( "save_data_to DATASOURCE.MDM_Requests_PhoneEntry with arguments \"Phone\":\"123$·$·$·456\"", Result = false )]
        public bool SpecialRegexTests_IsArgumentArray (string line)
        {
            return Grammar.SaveDataToFullLineRegex.IsMatch ( line );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "load data from DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "LOAD DATA FROM DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "load data from WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "load data from WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {   \"Phone\":  \"123$·$·$·456\"}", Result = true )]
        [TestCase ( "LOAD DATA FROM DATASOURCE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "load data from   WEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "load DATA fromWEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "LOAD DATA FROM WEBSERVICEMDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "load DATA from WEBSERVICEMDM_Requests_PhoneEntry with arguments 'Phone':'123$·$·$·456'", Result = false )]
        [TestCase ( "load data from DATASOURCE.MDM_Requests_PhoneEntry with arguments \"Phone\":\"123$·$·$·456\"", Result = false )]

        [TestCase ( "load_data_from DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "LOAD_DATA_from DATASOURCE.MDM_Requests_PhoneEntry with arguments {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "load_data_from WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {\"Phone\":\"123$·$·$·456\"}", Result = true )]
        [TestCase ( "load_data_from WEBSERVICE.MDM_Requests_PhoneEntry with arguments     {   \"Phone\":  \"123$·$·$·456\"}", Result = true )]
        [TestCase ( "LOAD_DATA_FROM DATASOURCE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "load_data_from   WEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = true )]
        [TestCase ( "load_DATA_FROMWEBSERVICE.MDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "LOAD_DATA_FROM WEBSERVICEMDM_Requests_PhoneEntry with arguments {'Phone':'123$·$·$·456'}", Result = false )]
        [TestCase ( "load_DATA_from WEBSERVICEMDM_Requests_PhoneEntry with arguments 'Phone':'123$·$·$·456'", Result = false )]
        [TestCase ( "load_data_from DATASOURCE.MDM_Requests_PhoneEntry with arguments \"Phone\":\"123$·$·$·456\"", Result = false )]
        public bool SpecialRegexTests_IsArgumentArray_2 ( string line )
        {
            return Grammar.LoadDataFromFullLineRegex.IsMatch ( line );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "this should be ok, since 'there are no outside brackets', right?", Result = true )]
        [TestCase ( "this should be ok, since \"there are no outside brackets\", right?", Result = true )]
        [TestCase ( "\"look ma, no brackets\"", Result = true )]
        [TestCase ( "'look ma, no brackets'", Result = true )]
        [TestCase ( "this should be ok, since 'the only { and } are inside a delimited quoted string', right?", Result = true )]
        [TestCase ( "this should be ok, since \"the only { and } are inside a delimited quoted string\", right?", Result = true )]
        [TestCase ( "In turn, the fact that { appears outside \"the quoted string\" makes this invalid", Result = false )]
        [TestCase ( "In turn, the fact that { or } appear outside 'the quoted string' makes this invalid", Result = false )]
        public bool LineWithQuotedStringAndNoOutsideBracketsTests ( string line )
        {
            return Grammar.LineWithQuotedStringAndNoOutsideBrackets.IsMatch ( line );
        }


        // ---------------------------------------------------------------------------------

        [TestCase ( "postpunk, darkwave, goth {\"neofolk\", \"darkfolk\", \"martial\"}, and other genres ...", Result = true )]
        [TestCase ( "postpunk, darkwave, goth {'neofolk', 'darkfolk', 'martial'}, and other genres ...", Result = true )]
        [TestCase ( "postpunk, darkwave, goth {\"neofolk, darkfolk, martial\"}, and other genres ...", Result = true )]
        [TestCase ( "postpunk, darkwave, goth {'neofolk, darkfolk, martial}, and other genres ...", Result = true )]
        [TestCase ( "postpunk, \"darkwave\", goth {'neofolk, darkfolk, martial}, and other genres ...", Result = false )]
        [TestCase ( "postpunk, \"darkwave\", goth {\"neofolk\", \"darkfolk\", \"martial\"}, and other genres ...", Result = false )]
        public bool LineWithOutsideBracketsAndNoOutsideQuotesTests ( string line )
        {
            return Grammar.LineWithOutsideBracketsAndNoOutsideQuotes.IsMatch ( line );
        }

        // ---------------------------------------------------------------------------------



        #region " --- problematic samples --- "

        [TestCase ( "load_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\":F_GD_VF_Country.Value} in F_GD_VF_CountryCode", Result = true )]
        [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\": F_GD_VF_Country.Value} in F_GD_VF_CountryCode", Result = true )]
        [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\":     F_GD_VF_Country.Value}", Result = true )]
		//[TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\":", Result = false )]
        // [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\" :    F_GD_VF_Country.Value", Result = false)]

        [TestCase ( "load_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\":\"F_GD_VF_Country.Value\"} in F_GD_VF_CountryCode", Result = true )]
        [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\":\"F_GD/&(%Country.Value\"} in F_GD_VF_CountryCode", Result = true )]
        [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\" : \"F_GD_VF_Country.Value\"}", Result = true )]
        [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes", Result = true )]
        [TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes in ANOTHER.CONTROL", Result = true )]
		//[TestCase ( "\t\tload_data_from DATASOURCE.MDM_CountryTelephonePrefixes with_args {\"COUNTRY\": ", Result = false )]

        public bool FullLoadDataFromRegexTest (string line)
        {
			// the problem with this monster is that some tests pass when the sentences are incomplete,....
            string regex = "^load_data_from[\t\\s]+((DATASOURCE|WEBSERVICE)\\.[A-Za-z0-9_\\.-]+){1}([\t\\s]+with_args[\t\\s]*{[\t\\s]*(\"|'){1}[A-Za-z0-9-_\\.]*(\"|'){1}[\t\\s]*:[\t\\s]*(((\"|'){1}.*(\"|'){1})|[A-Za-z09_\\.-]+)}[\t\\s]*)?([\t\\s]*in[\t\\s]*[A-Za-z0-9_\\.]+[\t\\s]*)?";
            var r= Regex.IsMatch ( line.Trim (), regex, RegexOptions.IgnoreCase );
            return r;
        }

        #endregion

        
        // ---------------------------------------------------------------------------------


        [TestCase ( "myregex", Result = true )]
        [TestCase ( "MYREGEX", Result = true )]
        [TestCase ( "myRegEx", Result = true )]
        public bool TestInsensitiveFlagInRegex (string testline)
        {
            return Regex.IsMatch ( testline, "^(?i)myregex$" );
        }


        // ---------------------------------------------------------------------------------


        // does not seem to work.....
        //[TestCase ( "...", Result = true )]
        //public bool TestBlockEscape ( string testline )
        //{
        //    return Regex.IsMatch ( testline, "^\Q...\E$" );
        //}
    }
}
