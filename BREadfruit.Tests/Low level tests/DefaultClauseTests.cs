using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


        [TestCase ( "max_length  9", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length 9", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length 9   ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length  9   ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length     9       ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length     9   ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length  19", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length 19", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length 19   ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length  19   ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "max_length     19       ", Grammar.MaxLengthLineRegex, Result = true )]
        [TestCase ( "min_length     19   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length  9", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length 9", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length 9   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length  9   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length     9       ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length     9   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length  19", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length 19", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length 19   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length  19   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length     19       ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "min_length     19   ", Grammar.MinLengthLineRegex, Result = true )]
        [TestCase ( "mandatory true", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory false", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory  true", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory  false", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory true     ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory false        ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory      true    ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory      false   ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory yes", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory no", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory  yes", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory  no", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory yes     ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory no        ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory      yes    ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "mandatory      no   ", Grammar.MandatoryLineRegex, Result = true )]
        // this one below passes the test as absence of value indicates true
        [TestCase ( "mandatory  ", Grammar.MandatoryLineRegex, Result = true )]
        [TestCase ( "enabled true", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled false", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled  true", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled  false", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled true     ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled false        ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled      true    ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled      false   ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled yes", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled no", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled  yes", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled  no", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled yes     ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled no        ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled      yes    ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "enabled      no   ", Grammar.EnabledLineRegex, Result = true )]
        [TestCase ( "visible true", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible false", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible  true", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible  false", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible true     ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible false        ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible      true    ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible      false   ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible yes", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible no", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible  yes", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible  no", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible yes     ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible no        ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible      yes    ", Grammar.VisibleLineRegex, Result = true )]
        [TestCase ( "visible      no   ", Grammar.VisibleLineRegex, Result = true )]
        // this one below passes the test as absence of value indicates true
        [TestCase ( "visible  ", Grammar.VisibleLineRegex, Result = true )]

        [TestCase ( "value ''", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value \"\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value      ''", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value      ''  ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value ''   ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value  \"\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value  \"\"    ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value   \"\"     ", Grammar.FreeValueLineRegex, Result = true )]

        [TestCase ( "value 'ANY VALUE INSIDE'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value \"just anything\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value      'xuyz'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value      'aaaa'  ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value 'ANYTHING 89'   ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value  \"auua88 0a-1\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value  \"\t;\t\"    ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value   \")(()(/&&%A/&S\"     ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "value   ')(()(/&&%A/&S\\'    ", Grammar.FreeValueLineRegex, Result = true )]

        [TestCase ( "VALUE USER.TAKE.us.HOME", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE USER.COUNTRY", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE USERCOUNTRY", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE USER_COUNTRY_IS_SHIT", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE   USER.TAKE.us.HOME", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE   USER.COUNTRY", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE   USERCOUNTRY", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE   USER_COUNTRY_IS_SHIT", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USER.TAKE.us.HOME", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USER.COUNTRY", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USERCOUNTRY", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USER_COUNTRY_IS_SHIT", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USER.TAKE.us.HOME  ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USER.COUNTRY ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USERCOUNTRY     ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE     USER_COUNTRY_IS._SHIT     ", Grammar.FreeValueLineRegex, Result = true )]

        [TestCase ( "VALUE 'USER.TAKE.us.HOME'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE 'USER.COUNTRY'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE 'USERCOUNTRY'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE 'USER_COUNTRY_IS_SHIT'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '  USER.TAKE.us.HOME'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '  USER.COUNTRY'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '  USERCOUNTRY'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '  USER_COUNTRY_IS_SHIT'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USER.TAKE.us.HOME'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USER.COUNTRY'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USERCOUNTRY'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USER_COUNTRY_IS_SHIT'", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USER.TAKE.us.HOME  '", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USER.COUNTRY' ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USERCOUNTRY  '   ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE '    USER_COUNTRY_IS._SHIT   '  ", Grammar.FreeValueLineRegex, Result = true )]

        [TestCase ( "VALUE \"USER.TAKE.us.HOME\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"USER.COUNTRY\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"USERCOUNTRY\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"USER_COUNTRY_IS_SHIT\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"  USER.TAKE.us.HOME\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"  USER.COUNTRY\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"  USERCOUNTRY\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"  USER_COUNTRY_IS_SHIT\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USER.TAKE.us.HOME\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USER.COUNTRY\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USERCOUNTRY\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USER_COUNTRY_IS_SHIT\"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USER.TAKE.us.HOME  \"", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USER.COUNTRY\" ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USERCOUNTRY  \"   ", Grammar.FreeValueLineRegex, Result = true )]
        [TestCase ( "VALUE \"    USER_COUNTRY_IS._SHIT   \"  ", Grammar.FreeValueLineRegex, Result = true )]



        [TestCase ( "label ''", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label \"\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label      ''", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label      ''  ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label ''   ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label  \"\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label  \"\"    ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label   \"\"     ", Grammar.LabelDefaultLineRegex, Result = true )]

        [TestCase ( "label 'ANY VALUE INSIDE'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label \"just anything\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label      'xuyz'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label      'aaaa'  ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label 'ANYTHING 89'   ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label  \"auua88 0a-1\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label  \"\t;\t\"    ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label   \")(()(/&&%A/&S\"     ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "label   ')(()(/&&%A/&S\\'    ", Grammar.LabelDefaultLineRegex, Result = true )]

        [TestCase ( "LABEL USER.TAKE.us.HOME", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL USER.COUNTRY", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL USERCOUNTRY", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL USER_COUNTRY_IS_SHIT", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL   USER.TAKE.us.HOME", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL   USER.COUNTRY", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL   USERCOUNTRY", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL   USER_COUNTRY_IS_SHIT", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USER.TAKE.us.HOME", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USER.COUNTRY", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USERCOUNTRY", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USER_COUNTRY_IS_SHIT", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USER.TAKE.us.HOME  ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USER.COUNTRY ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USERCOUNTRY     ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL     USER_COUNTRY_IS._SHIT     ", Grammar.LabelDefaultLineRegex, Result = true )]

        [TestCase ( "LABEL 'USER.TAKE.us.HOME'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL 'USER.COUNTRY'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL 'USERCOUNTRY'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL 'USER_COUNTRY_IS_SHIT'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '  USER.TAKE.us.HOME'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '  USER.COUNTRY'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '  USERCOUNTRY'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '  USER_COUNTRY_IS_SHIT'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USER.TAKE.us.HOME'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USER.COUNTRY'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USERCOUNTRY'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USER_COUNTRY_IS_SHIT'", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USER.TAKE.us.HOME  '", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USER.COUNTRY' ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USERCOUNTRY  '   ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL '    USER_COUNTRY_IS._SHIT   '  ", Grammar.LabelDefaultLineRegex, Result = true )]

        [TestCase ( "LABEL \"USER.TAKE.us.HOME\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"USER.COUNTRY\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"USERCOUNTRY\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"USER_COUNTRY_IS_SHIT\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"  USER.TAKE.us.HOME\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"  USER.COUNTRY\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"  USERCOUNTRY\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"  USER_COUNTRY_IS_SHIT\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USER.TAKE.us.HOME\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USER.COUNTRY\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USERCOUNTRY\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USER_COUNTRY_IS_SHIT\"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USER.TAKE.us.HOME  \"", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USER.COUNTRY\" ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USERCOUNTRY  \"   ", Grammar.LabelDefaultLineRegex, Result = true )]
        [TestCase ( "LABEL \"    USER_COUNTRY_IS._SHIT   \"  ", Grammar.LabelDefaultLineRegex, Result = true )]

        [TestCase ( "load_data_from DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = true )]
        [TestCase ( "load_data_from WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = true )]
        [TestCase ( "load_data_from DATASOURCE.S", Grammar.LoadDataDefaultLineRegex, Result = true )]
        [TestCase ( "load_data_from WEBSERVICE.S", Grammar.LoadDataDefaultLineRegex, Result = true )]
        [TestCase ( "load_data_from DATAsouRCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = true )]
        [TestCase ( "load_data_from webservice.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = true )]
        public bool DefaultLinesShouldPassValidation ( string line, string grammarRegex )
        {
            return Regex.IsMatch ( line.ToUpperInvariant (), grammarRegex );
        }

        [TestCase ( "max_length  ", Grammar.MaxLengthLineRegex, Result = false )]
        [TestCase ( "max_length a", Grammar.MaxLengthLineRegex, Result = false )]
        [TestCase ( "max_length    ", Grammar.MaxLengthLineRegex, Result = false )]
        [TestCase ( "max_length      ", Grammar.MaxLengthLineRegex, Result = false )]
        [TestCase ( "max_length     ?       ", Grammar.MaxLengthLineRegex, Result = false )]
        [TestCase ( "max_length     *   ", Grammar.MaxLengthLineRegex, Result = false )]
        [TestCase ( "min_length  ", Grammar.MinLengthLineRegex, Result = false )]
        [TestCase ( "min_length a", Grammar.MinLengthLineRegex, Result = false )]
        [TestCase ( "min_length    ", Grammar.MinLengthLineRegex, Result = false )]
        [TestCase ( "min_length      ", Grammar.MinLengthLineRegex, Result = false )]
        [TestCase ( "min_length     ?       ", Grammar.MinLengthLineRegex, Result = false )]
        [TestCase ( "min_length     *   ", Grammar.MinLengthLineRegex, Result = false )]
        [TestCase ( "mandatory 1", Grammar.MandatoryLineRegex, Result = false )]
        [TestCase ( "mandatory 0", Grammar.MandatoryLineRegex, Result = false )]
        [TestCase ( "mandatory  not", Grammar.MandatoryLineRegex, Result = false )]
        [TestCase ( "mandatory zza     ", Grammar.MandatoryLineRegex, Result = false )]
        [TestCase ( "mandatrory yes     ", Grammar.MandatoryLineRegex, Result = false )]
        [TestCase ( "mandatrory true     ", Grammar.MandatoryLineRegex, Result = false )]
        [TestCase ( "enabled 1", Grammar.EnabledLineRegex, Result = false )]
        [TestCase ( "enabled 0", Grammar.EnabledLineRegex, Result = false )]
        [TestCase ( "enabled  not", Grammar.EnabledLineRegex, Result = false )]
        [TestCase ( "enabled zza     ", Grammar.EnabledLineRegex, Result = false )]
        [TestCase ( "enabed  yes     ", Grammar.EnabledLineRegex, Result = false )]
        [TestCase ( "enabed  true     ", Grammar.EnabledLineRegex, Result = false )]
        [TestCase ( "visible 1", Grammar.VisibleLineRegex, Result = false )]
        [TestCase ( "visible 0", Grammar.VisibleLineRegex, Result = false )]
        [TestCase ( "visible  not", Grammar.VisibleLineRegex, Result = false )]
        [TestCase ( "visible zza     ", Grammar.VisibleLineRegex, Result = false )]
        [TestCase ( "visble yes     ", Grammar.VisibleLineRegex, Result = false )]
        [TestCase ( "visibled true     ", Grammar.VisibleLineRegex, Result = false )]
        [TestCase ( " VALUE USER.TAKE.us.HOME", Grammar.FreeValueLineRegex, Result = false )]
        [TestCase ( " VALUE USER.TAKE.us.   HOME", Grammar.FreeValueLineRegex, Result = false )]
        [TestCase ( " VALUE USER.TAKE.us.HOME", Grammar.FreeValueLineRegex, Result = false )]
        [TestCase ( "V ALUE USER.COUNTRY", Grammar.FreeValueLineRegex, Result = false )]
        [TestCase ( " VALUE USERCOUNTRY", Grammar.FreeValueLineRegex, Result = false )]
        [TestCase ( "   VALUE USER_COUN TRY_IS_SHIT", Grammar.FreeValueLineRegex, Result = false )]
        [TestCase ( " LABEL USER.TAKE.us.HOME", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( " LABEL USER.TAKE.us.   HOME", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( " LABEL USER.TAKE.us.HOME", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( "L ABEL USER.COUNTRY", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( " LABEL USERCOUNTRY", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( "   LABEL USER_COUN TRY_IS_SHIT", Grammar.LabelDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'DATAsourRCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'webservice.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'DATASOURCE.S", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'WEBSERVICE.S", Grammar.LoadDataDefaultLineRegex, Result = false )]

        [TestCase ( "load_data_from DATASOUCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from WEBSERICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS'", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS'", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'DATAsourRCE.SXXSX.XASSS.XXSSSXX.XSXS_XSX'S", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'webservice.SXXSX.XASSS.XXSSSXX.XSXS_XSXS'", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'DATASOURCE.S'", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from 'WEBSERVICE.S'", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from \"DATASOURCE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS\"", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from \"WEBSERVICE.SXXSX.XASSS.XXSSSXX.XSXS_XSXS\"", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from \"DATAsourRCE.SXXSX.XASSS.XXSSSXX.XSXS_XSX\"S", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from \"webservice.SXXSX.XASSS.XXSSSXX.XSXS_XSXS\"", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "load_data_from \"DATASOURCE.S\"", Grammar.LoadDataDefaultLineRegex, Result = false )]
        [TestCase ( "LOAD_DATA_FROM \"WEBSERVICE.S\"'", Grammar.LoadDataDefaultLineRegex, Result = false )]
        public bool DefaultLinesShouldNotPassValidation ( string line, string grammarRegex )
        {
            return Regex.IsMatch ( line.ToUpperInvariant (), grammarRegex );
        }
    }
}
