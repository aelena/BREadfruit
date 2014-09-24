using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Exceptions;
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
            Assert.That ( e.Form == "frmSearch" );
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

            // test equality stuff  

            Assert.IsTrue ( e.Defaults.First ().Token == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ).Token == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ).Token == Grammar.LabelDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 3 ).Token == Grammar.MinLengthDefaultClause );

            Assert.IsTrue ( e.Defaults.First () == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ) == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ) == Grammar.LabelDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 3 ) == Grammar.MinLengthDefaultClause );

            Assert.IsTrue ( e.ConditionlessActions.First ().Action == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.First () == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ).Action == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ) == Grammar.HideUnaryActionSymbol );

            Assert.IsTrue ( e.Triggers.First ().Event == Grammar.ChangedEventSymbol );
            Assert.IsTrue ( e.Triggers.First () == Grammar.ChangedEventSymbol );

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
            Assert.That ( e.Form == "frmSearch" );
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

            Assert.IsTrue ( e.Defaults.First () == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ) == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ) == Grammar.LabelDefaultClause );

            Assert.IsTrue ( e.ConditionlessActions.First ().Action == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.First () == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ).Action == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ) == Grammar.HideUnaryActionSymbol );

            Assert.IsTrue ( e.Triggers.First ().Event == Grammar.ChangedEventSymbol );
            Assert.IsTrue ( e.Triggers.First () == Grammar.ChangedEventSymbol );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile003 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File003.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Form == "frmSearch" );
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

            Assert.IsTrue ( e.Defaults.First () == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ) == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ) == Grammar.MandatoryDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 3 ) == Grammar.LoadDataDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 4 ) == Grammar.LabelDefaultClause );

            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ) == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ) == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 2 ) == Grammar.LoadDataUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 3 ) == Grammar.SetValueActionSymbol );

            Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First().Operator == Grammar.InOperator );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First().ResultActions.First () == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First().Operator == Grammar.IsOperator );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First().ResultActions.First () == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.First().Operator == Grammar.NotInOperator );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.First().ResultActions.First () == Grammar.SetValueActionSymbol );

            Assert.IsTrue ( e.Triggers.First () == Grammar.ChangedEventSymbol );



        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile004 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File004.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Form == "frmSearch" );
            Assert.That ( e.Defaults.Count () == 1 );
            Assert.That ( e.Defaults.First ().Arguments.Count () == 4 );
            Assert.That ( e.Defaults.First ().Arguments.First ().Key == "\"Active\"" );
            Assert.That ( e.Defaults.First ().Arguments.First ().Value == "true" );
            Assert.That ( e.Defaults.First ().Arguments.Last ().Key == "\"Title\"" );
            Assert.That ( e.Defaults.First ().Arguments.Last ().Value == "\"Anything\"" );
            Assert.That ( e.Constraints.Count () == 1 );
            Assert.That ( e.Constraints.First ().Name == "only_numbers" );

            Assert.IsTrue ( e.Defaults.ElementAt ( 0 ) == Grammar.LoadDataDefaultClause );
            Assert.IsTrue ( e.Constraints.ElementAt ( 0 ) == Grammar.OnlyNumbersConstraintSymbol );



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
            Assert.That ( parser.Entities.First().Form == "frmSearch" );
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
            Assert.That ( parser.Entities.First().Form == "frmSearch" );
            Assert.That ( parser.Entities.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Count () == 8 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile008 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File008.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );
            Assert.That ( parser.Entities.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Count () == 0 );
            Assert.That ( parser.Entities.First ().ConditionlessActions.Count () == 2 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        [ExpectedException ( ExpectedException = typeof(InvalidEntityDeclarationException), 
            ExpectedMessage="Invalid Entity declaration found in line 1 - 'Entity DDLCDCountry is Buttton in \"frmSearch\"'"  )]
        public void ParseSampleFile009 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File009.txt" );
            Assert.That ( parser.Entities.Count () == 0 );

        }

        // ---------------------------------------------------------------------------------


        [Test]
        [ExpectedException ( ExpectedException = typeof ( InvalidWithClauseException ),
            ExpectedMessage = "Invalid With clause found in line 4 - 'with actiones'" )]
        public void ParseSampleFile010 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File010.txt" );

        }

        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile011 ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\single entity tests\File011.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );

            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 4 );

            Assert.That ( e.Defaults.ElementAt ( 0 ).ToString ().Equals ( "visible true" ) );
            Assert.That ( e.Defaults.ElementAt ( 0 ) == Grammar.VisibleDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Value.ToString () == Grammar.TrueSymbol );

            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Equals ( "enable true" ) );
            Assert.That ( e.Defaults.ElementAt ( 1 ) == Grammar.EnabledDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == Grammar.TrueSymbol );

            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString ().Equals ( "value \"NG\"" ) );
            Assert.That ( e.Defaults.ElementAt ( 2 ) == Grammar.ValueDefaultClause );
            // the value is represented internally as an already quoted string
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString() == "\"NG\"" );

            Assert.That ( e.Defaults.ElementAt ( 3 ).ToString ().Equals ( "load_data_from DATASOURCE.VENDOR_CLASSIFICATIONS" ) );
            Assert.That ( e.Defaults.ElementAt ( 3 ) == Grammar.LoadDataDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Value.ToString () == "DATASOURCE.VENDOR_CLASSIFICATIONS" );


        } 

        // ---------------------------------------------------------------------------------


        [Test]
        public void ShouldFindEntities ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\vendor-rules.txt" );

            Assert.That ( parser.Entities.Count () == 18 );

            var parser2 = new Parser ();
            var eList = parser2.ParseRuleFile ( @"..\..\sample files\vendor-rules.txt" );

            Assert.That ( eList.Count () == 18 );


            // TBVendorName
            Assert.That ( parser.Entities.ElementAt ( 0 ).Name == "TBVendorName" );
            Assert.That ( parser.Entities.ElementAt ( 0 ).Defaults.Count () == 4 );
            Assert.That ( parser.Entities.ElementAt ( 0 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 0 ).ConditionlessActions.First ().ToString () == "hide btnCreateVendor" );
            Assert.That ( parser.Entities.ElementAt ( 0 ).ConditionlessActions.Last ().ToString () == "hide vendorSearchResultGrid" );
            Assert.That ( parser.Entities.ElementAt ( 0 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 0 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 0 ).Triggers.First ().Event == "changed" );
            Assert.That ( parser.Entities.ElementAt ( 0 ).Triggers.First ().Target == "TBVendorName.value" );
            Assert.That ( parser.Entities.ElementAt ( 0 ).Constraints.Count () == 0 );

            // TBVendorCity
            Assert.That ( parser.Entities.ElementAt ( 1 ).Name == "TBVendorCity" );
            Assert.That ( parser.Entities.ElementAt ( 1 ).Defaults.Count () == 3 );
            Assert.That ( parser.Entities.ElementAt ( 1 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 1 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 1 ).ConditionlessActions.First ().ToString () == "hide btnCreateVendor" );
            Assert.That ( parser.Entities.ElementAt ( 1 ).ConditionlessActions.Last ().ToString () == "hide vendorSearchResultGrid" );
            Assert.That ( parser.Entities.ElementAt ( 1 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 1 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 2 ).Name == "TBVendorNumber" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Defaults.Count () == 5 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.First ().Conditions.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.First ().Conditions.First ().ResultActions.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.First ().Conditions.First ().ResultActions.First ().Action == "set_value" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.First ().Conditions.First ().ResultActions.First ().Reference == "ErrorMessage" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "MESSAGES.VALIDATIONS.SEARCH.NOTMANAGED" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.Last ().Conditions.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.Last ().Conditions.First ().ResultActions.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.Last ().Conditions.First ().ResultActions.First ().Action == "set_value" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.Last ().Conditions.First ().ResultActions.First ().Reference == "ErrorMessage" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Rules.Last ().Conditions.First ().ResultActions.First ().Value.ToString () == "''" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Triggers.First ().Event == "changed" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Triggers.First ().Target == "TBVendorNumber.value" );
            Assert.That ( parser.Entities.ElementAt ( 2 ).Constraints.Count () == 1 );


            Assert.That ( parser.Entities.ElementAt ( 3 ).Name == "TBVendorIFA" );
            Assert.That ( parser.Entities.ElementAt ( 3 ).Defaults.Count () == 3 );
            Assert.That ( parser.Entities.ElementAt ( 3 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 3 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 3 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 3 ).Triggers.First ().Event == "changed" );
            Assert.That ( parser.Entities.ElementAt ( 3 ).Triggers.First ().Target == "TBVendorIFA.value" );
            Assert.That ( parser.Entities.ElementAt ( 3 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 4 ).Name == "DDLVDCountry" );
            Assert.That ( parser.Entities.ElementAt ( 4 ).Defaults.Count () == 5 );
            Assert.That ( parser.Entities.ElementAt ( 4 ).ConditionlessActions.Count () == 4 );
            Assert.That ( parser.Entities.ElementAt ( 4 ).Rules.Count () == 4 );
            Assert.That ( parser.Entities.ElementAt ( 4 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 4 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 5 ).Name == "TBVendorVAT" );
            Assert.That ( parser.Entities.ElementAt ( 5 ).Defaults.Count () == 3 );
            Assert.That ( parser.Entities.ElementAt ( 5 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 5 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 5 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 5 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 6 ).Name == "DDLCDCountry" );
            Assert.That ( parser.Entities.ElementAt ( 6 ).Defaults.Count () == 4 );
            Assert.That ( parser.Entities.ElementAt ( 6 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 6 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 6 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 6 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 7 ).Name == "DDLCDCompany" );
            Assert.That ( parser.Entities.ElementAt ( 7 ).Defaults.Count () == 3 );
            Assert.That ( parser.Entities.ElementAt ( 7 ).ConditionlessActions.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 7 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 7 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 7 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 8 ).Name == "DDLCDPurchOrg" );
            Assert.That ( parser.Entities.ElementAt ( 8 ).Defaults.Count () == 3 );
            Assert.That ( parser.Entities.ElementAt ( 8 ).ConditionlessActions.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 8 ).Rules.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 8 ).Triggers.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 8 ).Constraints.Count () == 0 );


            Assert.That ( parser.Entities.ElementAt ( 9 ).Name == "btnSearch" );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Defaults.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).ConditionlessActions.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Rules.Count () == 8 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Triggers.Count () == 0 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Constraints.Count () == 0 );

        }

        // ---------------------------------------------------------------------------------


    }
}
