using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace BREadfruit.Tests
{
    [TestFixture]
    public class HighLevelTests
    {
        /// <summary>
        /// The purpose of the tests in this section is to consume a series of small
        /// files containing just one or only a few entity definitions and
        /// performing a set of checks against the produced object map to ensure
        /// the files have been read correctly and the object graph is correct.
        /// 
        /// The numbering in the test, such as 
        /// 
        /// ParseSampleFile001
        /// 
        /// corresponde to the naming of the files,
        /// where the file is File001 in folder 'single entity tests'.
        /// </summary>
        #region " --- single entity files for testing --- "

        [Test]
        public void ParseSampleFile001 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File001.txt" );
            // after parsing we should have something here...
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 4 );
            Assert.That ( e.Defaults.First ().ToString () == "visible true" );
            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Trim () == "value" );
            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString () == "label LABELS.VENDOR.NAME" );
            Assert.That ( e.ConditionlessActions.Count () == 2 );
            Assert.That ( e.Rules.Count () == 0 );
            Assert.That ( e.ConditionlessActions.Count () == 2 );
            Assert.That ( e.ConditionlessActions.First ().ToString () == "hide btnCreateVendor" );
            Assert.That ( e.ConditionlessActions.ElementAt ( 1 ).ToString () == "hide vendorSearchResultGrid" );
            Assert.That ( e.Constraints.Count () == 0 );
            Assert.That ( e.Triggers.Count () == 1 );
            Assert.That ( e.Triggers.First ().ToString () == "TBVendorName.value changed" );


        }

        #endregion

        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile002 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File002.txt" );
            // after parsing we should have something here...
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 3 );
            Assert.That ( e.Defaults.First ().ToString () == "visible true" );
            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Trim () == "value" );
            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString () == "label LABELS.VENDOR.CITY" );
            Assert.That ( e.ConditionlessActions.Count () == 2 );
            Assert.That ( e.Rules.Count () == 0 );
            Assert.That ( e.ConditionlessActions.Count () == 2 );
            Assert.That ( e.ConditionlessActions.First ().ToString () == "hide btnCreateVendor" );
            Assert.That ( e.ConditionlessActions.ElementAt ( 1 ).ToString () == "hide vendorSearchResultGrid" );
            Assert.That ( e.Constraints.Count () == 0 );
            Assert.That ( e.Triggers.Count () == 1 );
            Assert.That ( e.Triggers.First ().ToString () == "TBVendorCity.value changed" );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile003 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File003.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 5 );
            Assert.That ( e.Defaults.First ().ToString () == "visible true" );
            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Trim () == "value USER.COUNTRY" );
            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString ().Trim () == "mandatory true" );
            Assert.That ( e.Defaults.ElementAt ( 3 ).ToString ().Trim () == "load_data_from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES" );
            Assert.That ( e.Defaults.ElementAt ( 4 ).ToString () == "label LABELS.GENERIC.COUNTRY" );
            Assert.That ( e.ConditionlessActions.Count () == 4 );
            Assert.That ( e.ConditionlessActions.First ().ToString () == "hide vendorSearchResultGrid" );
            Assert.That ( e.ConditionlessActions.ElementAt ( 1 ).ToString () == "hide btnCreateVendor" );
            Assert.That ( e.ConditionlessActions.ElementAt ( 2 ).ToString () == "load_data_from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES" );
            Assert.That ( e.Rules.Count () == 4 );
            Assert.That ( e.Constraints.Count () == 0 );
            Assert.That ( e.Triggers.Count () == 1 );
            Assert.That ( e.Triggers.First ().ToString () == "DDLVDCountry.value changed" );



        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile004 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File004.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 1 );
            Assert.That ( e.Defaults.First ().Arguments.Count () == 4 );
            Assert.That ( e.Defaults.First ().Arguments.First ().Key == "\"Active\"" );
            Assert.That ( e.Defaults.First ().Arguments.First ().Value == "true" );
            Assert.That ( e.Defaults.First ().Arguments.Last ().Key == "\"Title\"" );
            Assert.That ( e.Defaults.First ().Arguments.Last ().Value == "\"Anything\"" );
            Assert.That ( e.Constraints.Count () == 1 );
            Assert.That ( e.Constraints.First ().Name == "only_numbers" );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile005 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File005.txt" );
            Assert.That ( parser.Entities.Count () == 1 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile006 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File006.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Count () == 2 );
            Assert.That ( parser.Entities.First ().Rules.First ().Conditions.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Last ().Conditions.Count () == 1 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile007 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File007.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Count () == 8 );

        }




        // ---------------------------------------------------------------------------------



        [Test]
        public void ShouldFindEntities ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\vendor-rules.txt" );

            Assert.That ( parser.Entities.Count () == 8 );

        }

        // ---------------------------------------------------------------------------------


    }
}
