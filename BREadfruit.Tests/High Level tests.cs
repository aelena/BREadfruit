using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Exceptions;
using NUnit.Framework;
using BREadfruit.Helpers;
using BREadfruit.Conditions;

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
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File001.txt" );
            // after parsing we should have something here...
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Form == "frmSearch" );
            Assert.That ( e.Defaults.Count () == 5 );
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

            Assert.IsTrue ( e.Defaults.First ().Token == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ).Token == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ).Token == Grammar.LabelDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 3 ).Token == Grammar.MinLengthDefaultClause );

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
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File002.txt" );
            // after parsing we should have something here...
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Form == "frmSearch" );
            Assert.That ( e.Defaults.Count () == 4 );
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

            Assert.IsTrue ( e.Defaults.First ().Token == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ).Token == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ).Token == Grammar.LabelDefaultClause );

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
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File003.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Form == "frmSearch" );
            Assert.That ( e.Defaults.Count () == 6 );
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

            Assert.IsTrue ( e.Defaults.First ().Token == Grammar.VisibleDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 1 ).Token == Grammar.ValueDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 2 ).Token == Grammar.MandatoryDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 3 ).Token == Grammar.LoadDataDefaultClause );
            Assert.IsTrue ( e.Defaults.ElementAt ( 4 ).Token == Grammar.LabelDefaultClause );

            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ) == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ) == Grammar.HideUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 2 ) == Grammar.LoadDataUnaryActionSymbol );
            Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 3 ) == Grammar.SetValueActionSymbol );

            Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operator == Grammar.InOperator );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operator == Grammar.IsOperator );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.First ().Operator == Grammar.NotInOperator );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );

            Assert.IsTrue ( e.Triggers.First () == Grammar.ChangedEventSymbol );



        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile004 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File004.txt" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Form == "frmSearch" );
            Assert.That ( e.Defaults.Count () == 3 );
            Assert.That ( e.Defaults.First ().Arguments.Count () == 4 );
            Assert.That ( e.Defaults.First ().Arguments.First ().Key == "\"Active\"" );
            Assert.That ( e.Defaults.First ().Arguments.First ().Value == "true" );
            Assert.That ( e.Defaults.First ().Arguments.Last ().Key == "\"Title\"" );
            Assert.That ( e.Defaults.First ().Arguments.Last ().Value == "\"Anything\"" );
            Assert.That ( e.Constraints.Count () == 1 );
            Assert.That ( e.Constraints.First ().Name == "only_numbers" );

            Assert.IsTrue ( e.Defaults.ElementAt ( 0 ).Token == Grammar.LoadDataDefaultClause );
            Assert.IsTrue ( e.Constraints.ElementAt ( 0 ) == Grammar.OnlyNumbersConstraintSymbol );



        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile005 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File005.txt" );
            Assert.That ( parser.Entities.Count () == 1 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile006 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File006.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );
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
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File007.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );
            Assert.That ( parser.Entities.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Count () == 9 );


            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.Count () == 5 );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 0 ).ToString () == "TBVendorName == \"\"" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 0 ).SuffixLogicalOperator == Grammar.ANDSymbol );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 0 ).Operand == "TBVendorName" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).ToString () == "TBVendorCity == \"\"" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).SuffixLogicalOperator == Grammar.ANDSymbol );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).Operand == "TBVendorCity" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).ToString () == "TBVendorIFA == \"\"" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).SuffixLogicalOperator == Grammar.ANDSymbol );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).Operand == "TBVendorIFA" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 3 ).ToString () == "DDLVDCountry == \"\"" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 3 ).SuffixLogicalOperator == Grammar.ANDSymbol );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 3 ).Operand == "DDLVDCountry" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 4 ).ToString () == "TBVendorVAT == \"\"" );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 4 ).SuffixLogicalOperator == null );
            Assert.That ( parser.Entities.First ().Rules.ElementAt ( 3 ).Conditions.ElementAt ( 4 ).Operand == "TBVendorVAT" );
            Assert.That ( parser.Entities.First ().Rules.Last ().Conditions.First ().ToString () == "TBVendorIFA.value != \"\"" );
            Assert.That ( parser.Entities.First ().Rules.Last ().Conditions.First ().Results.First ().ToString () == "mandatory DDLVDCountry" );
            Assert.That ( parser.Entities.First ().Rules.Last ().Conditions.First ().Results.First ().Token == "mandatory" );
            Assert.That ( parser.Entities.First ().Rules.Last ().Conditions.First ().Results.First ().Reference == "DDLVDCountry" );
            Assert.That ( parser.Entities.First ().Rules.Last ().Conditions.First ().ResultActions.Count () == 0 );

            Assert.That ( parser.Entities.First ().Rules.Penultimate ().Conditions.First ().ToString () == "TBVendorIFA.value != \"\"" );
            Assert.That ( parser.Entities.First ().Rules.Penultimate ().Conditions.First ().Results.First ().ToString () == "not_mandatory DDLVDCountry" );
            Assert.That ( parser.Entities.First ().Rules.Penultimate ().Conditions.First ().Results.First ().Token == "not_mandatory" );
            Assert.That ( parser.Entities.First ().Rules.Penultimate ().Conditions.First ().Results.First ().Reference == "DDLVDCountry" );
            Assert.That ( parser.Entities.First ().Rules.Penultimate ().Conditions.First ().ResultActions.Count () == 0 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile008 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File008.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );
            Assert.That ( parser.Entities.Count () == 1 );
            Assert.That ( parser.Entities.First ().Rules.Count () == 0 );
            Assert.That ( parser.Entities.First ().ConditionlessActions.Count () == 2 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        [ExpectedException ( ExpectedException = typeof ( InvalidEntityDeclarationException ),
            ExpectedMessage = "Invalid Entity declaration found in line 3 - 'Entity DDLCDCountry is Buttton in \"frmSearch\"'" )]
        public void ParseSampleFile009 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File009.txt" );
            Assert.That ( parser.Entities.Count () == 0 );

        }

        // ---------------------------------------------------------------------------------


        [Test]
        [ExpectedException ( ExpectedException = typeof ( InvalidWithClauseException ),
            ExpectedMessage = "Invalid With clause found in line 7 - 'with actiones'" )]
        public void ParseSampleFile010 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File010.txt" );

        }

        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile011 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File011.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );

            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 4 );

            Assert.That ( e.Defaults.ElementAt ( 0 ).ToString ().Equals ( "visible true" ) );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Token == Grammar.VisibleDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Value.ToString () == Grammar.TrueSymbol );

            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Equals ( "enable true" ) );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.EnabledDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == Grammar.TrueSymbol );

            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString ().Equals ( "value \"NG\"" ) );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Token == Grammar.ValueDefaultClause );
            // the value is represented internally as an already quoted string
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString () == "\"NG\"" );

            Assert.That ( e.Defaults.ElementAt ( 3 ).ToString ().Equals ( "load_data_from DATASOURCE.VENDOR_CLASSIFICATIONS" ) );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Token == Grammar.LoadDataDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Value.ToString () == "DATASOURCE.VENDOR_CLASSIFICATIONS" );

            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().ToString () == "role in {\"UPM\",\"CPM\"}" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().ResultActions.Count () == 0 );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.First ().ToString () == "enable false" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.First () == Grammar.EnableUnaryActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.First ().Token.ToString () == "enable" );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Count () == 2 );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ToString () == "Vendor.Country is \"ES\"" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ToString () == "GD_Ctr_TaxCode1.Text starts_with {\"P\",\"Q\",\"S\"}" );

            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ToString () == "Vendor.Country is \"ES\"" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ToString () == "GD_Ctr_TaxCode1.Text starts_with \"X\"" );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().ToString () == "set_value \"G\" GD_Ctr_VendorClassification" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().IsResultAction == true );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Action == "set_value" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "\"G\"" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Ctr_VendorClassification" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol );

            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().ToString () == "set_value \"NG\" GD_Ctr_VendorClassification" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().IsResultAction == true );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Action == "set_value" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "\"NG\"" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Ctr_VendorClassification" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile012 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File012.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmMain" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 3 );
            Assert.That ( e.Defaults.ElementAt ( 0 ).ToString ().Equals ( "visible true" ) );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Token == Grammar.VisibleDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Value.ToString () == Grammar.TrueSymbol );
            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Equals ( "enable true" ) );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.EnabledDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == Grammar.TrueSymbol );
            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString ().Equals ( "value " ) );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Token == Grammar.ValueDefaultClause );
            // the value is represented internally as an already quoted string
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString () == "" );

            Assert.That ( e.Rules.Count () == 3 );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Count () == 2 );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Count () == 2 );

            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Value.ToString () == "{\"YVT4\",\"YVT5\"}" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Operand == "Vendor.AccountGroup" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Operator == Grammar.InOperator );

            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.First ().ToString () == "show GD_Ctr_IsNaturalPerson" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.ElementAt ( 1 ).ToString () == "show DIV_ADDITIONAL_INFO_NATURAL_PERSON" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.ElementAt ( 2 ).ToString () == "hide GD_Ctr_IsNaturalPerson" );
            Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Results.ElementAt ( 3 ).ToString () == "hide ABCDEFG_HIJK" );

            Assert.That ( e.Rules.First ().Conditions.First ().Results.Count () == 4 );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().ToString () == "show GD_Ctr_IsNaturalPerson" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.ElementAt ( 1 ).ToString () == "show DIV_ADDITIONAL_INFO_NATURAL_PERSON" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.ElementAt ( 2 ).ToString () == "hide GD_Ctr_IsNaturalPerson" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.ElementAt ( 3 ).ToString () == "hide ABCDEFG_HIJK" );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Count () == 2 );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ToString () == "Vendor.Country is \"ES\"" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().SuffixLogicalOperator == Grammar.ANDSymbol );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ToString () == "GD_Ctr_TaxCode1.Text starts_with {\"P\",\"Q\",\"S\"}" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().SuffixLogicalOperator == null );

            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ToString () == "Vendor.Country is \"ES\"" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ToString () == "GD_Ctr_TaxCode1.Text starts_with \"X\"" );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().ToString () == "set_value false GD_Ctr_IsNaturalPerson.Checked" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().IsResultAction == true );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Action == "set_value" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "false" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Ctr_IsNaturalPerson.Checked" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol );

            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().ToString () == "set_value true GD_Ctr_IsNaturalPerson.Checked" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().IsResultAction == true );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Action == "set_value" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "true" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Ctr_IsNaturalPerson.Checked" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile013 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File013.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmMain" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 5 );
            Assert.That ( e.Defaults.ElementAt ( 0 ).ToString ().Equals ( "visible true" ) );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Token == Grammar.VisibleDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 0 ).Value.ToString () == Grammar.TrueSymbol );
            Assert.That ( e.Defaults.ElementAt ( 1 ).ToString ().Equals ( "enable true" ) );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.EnabledDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == Grammar.TrueSymbol );
            Assert.That ( e.Defaults.ElementAt ( 2 ).ToString ().Equals ( "value " ) );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Token == Grammar.ValueDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString () == "" );
            Assert.That ( e.Defaults.ElementAt ( 3 ).ToString ().Equals ( "load_data_from DATASOURCE.YES_NO" ) );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Token == Grammar.LoadDataDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Value.ToString () == "DATASOURCE.YES_NO" );
            Assert.That ( e.Defaults.ElementAt ( 4 ).ToString ().Equals ( "mandatory true" ) );
            Assert.That ( e.Defaults.ElementAt ( 4 ).Token == Grammar.MandatoryDefaultClause );
            Assert.That ( e.Defaults.ElementAt ( 4 ).Value.ToString () == "true" );

            Assert.That ( e.Rules.First ().Conditions.Count () == 2 );
            Assert.That ( e.Rules.First ().Conditions.First ().ToString () == "Request.FlowType not_in {\"VC\",\"VC_CC\"}" );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.Count () == 4 );
            Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.Count () == 0 );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.First ().Token == Grammar.MakeNonMandatoryUnaryActionSymbol );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.First ().Reference == "GD_Damex" );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.ElementAt ( 1 ) == Grammar.MakeMandatoryUnaryActionSymbol );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.ElementAt ( 1 ).Reference == "ABCD" );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.ElementAt ( 2 ) == Grammar.MakeMandatoryUnaryActionSymbol );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.ElementAt ( 2 ).Reference == "EFGH" );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.ElementAt ( 3 ) == Grammar.HideUnaryActionSymbol );
            Assert.That ( e.Rules.First ().Conditions.Last ().Results.ElementAt ( 3 ).Reference == "GD_Damex" );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile014 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File014.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.Defaults.Count () == 4 );
            Assert.That ( e.Triggers.Count () == 5, "Expected 5 triggers" );
            Assert.That ( e.Triggers.First ().ToString () == "TBVendorCity.value changed", "wrong trigger expression(1)." );
            Assert.That ( e.Triggers.ElementAt ( 1 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(2)." );
            Assert.That ( e.Triggers.ElementAt ( 2 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(3)." );
            Assert.That ( e.Triggers.ElementAt ( 3 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(4)." );
            Assert.That ( e.Triggers.Last ().ToString () == "TBVendorCity clicked", "wrong trigger expression(5)." );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile015 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File015.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmSearch" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();
            Assert.That ( e.ConditionlessActions.Count () == 3 );
            Assert.That ( e.ConditionlessActions.First ().ToString ().Equals ( "change_form_to \"frmMain\"" ) );
            Assert.That ( e.ConditionlessActions.ElementAt ( 1 ).ToString ().Equals ( "change_form_to frmMain" ) );
            Assert.That ( e.ConditionlessActions.ElementAt ( 2 ).ToString ().Equals ( "change_form_to \"frmMain\" this" ) );
            Assert.That ( e.ConditionlessActions.ElementAt ( 2 ).IsResultAction );
            Assert.That ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 2 ) ).Arguments.Count () == 1 );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile016 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File016.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmMain" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();

            Assert.That ( e.Rules.Count () == 1 );
            Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.Count () == 2 );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Reference == "GD_Damex" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Action == Grammar.MakeNonMandatoryUnaryActionSymbol.Token );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.Last ().Reference == "GD_Damex" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.Last ().Action == Grammar.NotVisibleUnaryActionSymbol.Token );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile017 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File017 - GD_Ctr_IsNaturalPerson.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmMain" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();

            Assert.That ( e.Rules.Count () == 3 );
            Assert.That ( e.Defaults.Count () == 3 );
            Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.Count () == 2 );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Reference == "GD_Ctr_IsNaturalPerson" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Action == Grammar.ShowElementUnaryActionSymbol.Token );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.Last ().Reference == "DIV_ADDITIONAL_INFO_NATURAL_PERSON" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.Last ().Action == Grammar.ShowElementUnaryActionSymbol.Token );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Count () == 2 );
            Assert.That ( e.Rules.ElementAt ( 1 ).IsMultipleConditionRule );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.Count () == 1 );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).Operand == "GD_Ctr_TaxCode1.Text" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).Operator == Grammar.StartsWithOperator );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Reference == "GD_Ctr_IsNaturalPerson.Checked" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Value.ToString () == Grammar.FalseSymbol.Token );


            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Reference == "GD_Ctr_IsNaturalPerson.Checked" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Value.ToString () == Grammar.TrueSymbol.Token );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile018 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File018 - GD_Adr_Street2.txt" );
            Assert.That ( parser.Entities.First ().Form == "frmMain" );
            Assert.That ( parser.Entities.Count () == 1 );
            var e = parser.Entities.First ();

            Assert.That ( e.Defaults.Count () == 3 );
            Assert.That ( e.Defaults.First ().Token == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Defaults.First ().Value.ToString () == "32" );

            Assert.That ( e.Rules.Count () == 1 );
            Assert.That ( e.Rules.First ().Conditions.First ().Operand == "REGIONAL_COUNTRY" );
            Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.IsOperator );

            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Action == Grammar.NotVisibleUnaryActionSymbol.Token );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Reference == "GD_Adr_Street2" );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile019 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File019 - GD_Adr_PostalCode.txt" );
            var e = parser.Entities.First ();

            Assert.That ( e.Defaults.Count () == 4 );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Token == "max_length" );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString () == "10" );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Token == "validation_regex" );
            Assert.That ( e.Defaults.ElementAt ( 3 ).Value.ToString () == "\"^[0-9]{4}-[0-9]{3}$\"" );

            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "4" );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.Last ().Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.Last ().Value.ToString () == "^[0-9]{4}$" );
            Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.Last ().Reference == "GD_Adr_PostalCode" );

            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First ().Value.ToString () == "5" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.Last ().Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.Last ().Value.ToString () == "^[0-9]{5}$" );
            Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.Last ().Reference == "GD_Adr_PostalCode" );

            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.First ().Value.ToString () == "8" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.Last ().Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.Last ().Value.ToString () == "^[0-9]{4}-[0-9]{3}$" );
            Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.Last ().Reference == "GD_Adr_PostalCode" );

            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Count () == 2 );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Last ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "4" );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Last ().ResultActions.Last ().Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Last ().ResultActions.Last ().Value.ToString () == "^(1[5-9][0-9]{2}){1}|((2|3)[0-9]{3}){1}|((8|9)[0-9]{3})$" );
            Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.Last ().ResultActions.Last ().Reference == "GD_Adr_PostalCode" );

            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Count () == 2 );
            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Last ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "4" );
            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Last ().ResultActions.Last ().Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Last ().ResultActions.Last ().Value.ToString () == "^(1[3-4][0-9]{2}){1}|([4-7][0-9]{3})$" );
            Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.Last ().ResultActions.Last ().Reference == "GD_Adr_PostalCode" );

            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Count () == 2 );
            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Last ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "4" );
            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Last ().ResultActions.Last ().Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Last ().ResultActions.Last ().Value.ToString () == "^(1[0-2][0-9]{2})$" );
            Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.Last ().ResultActions.Last ().Reference == "GD_Adr_PostalCode" );

            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.First ().Action == Grammar.MaxlengthDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "5" );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.First ().Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.ElementAt ( 1 ).Action == Grammar.ValidationRegexDefaultClause );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.ElementAt ( 1 ).Value.ToString () == "^[0-9]{5}$" );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.ElementAt ( 1 ).Reference == "GD_Adr_PostalCode" );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.ElementAt ( 2 ).Action == Grammar.SetValueActionSymbol );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.ElementAt ( 2 ).Value.ToString () == "substring(GD_Adr_PostalCode,2)" );
            Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.Last ().ResultActions.ElementAt ( 2 ).Reference == "GD_Adr_Region" );

        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile020 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File020 - GD_Adr_Search.txt" );
            var e = parser.Entities.First ();

            Assert.That ( e.Defaults.Count () == 4, "wrong default count" );
            Assert.That ( e.Defaults.Last ().Value.ToString () == "GD_Adr_Name1.Value" );
            Assert.That ( e.Rules.First ().Conditions.First ().Results.First ().Reference == "GD_Adr_Search" );


        }


        // ---------------------------------------------------------------------------------



        [Test]
        public void ParseSampleFile021 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File021 - Add Visible and Enabled by default if not indicated.txt" );
            var e = parser.Entities.First ();

            Assert.That ( e.Defaults.Count () == 3, "wrong default count" );
            Assert.That ( e.Defaults.First ().Value.ToString () == "32" );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.VisibleDefaultClause.Token );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value as Boolean? == true );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Token == Grammar.EnabledDefaultClause.Token );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value as Boolean? == true );


        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile022 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File022 - Don't add Visible and Enabled by default if already specified.txt" );
            var e = parser.Entities.First ();

            Assert.That ( e.Defaults.Count () == 3, "wrong default count" );
            Assert.That ( e.Defaults.First ().Value.ToString () == "32" );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.VisibleDefaultClause.Token );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == "true" );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Token == Grammar.EnabledDefaultClause.Token );
            Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString () == "true" );


        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile023 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File023 - GD_PVO_Questions.txt" );
            var e = parser.Entities.First ();
            Assert.That ( e.Name == "GD_PVO_Question1", "Wrong entity name" );
            Assert.That ( e.Defaults.Count () == 6, "should have 6 defaults but has " + e.Defaults.Count () );
            Assert.That ( e.Rules.Count () == 2, "should have 2 rules but has " + e.Rules.Count () );
            Assert.That ( e.Rules.All ( x => x.Conditions.First ().Operand == "GD_PVO_Question1.Value" ), "error in operands" );
            Assert.That ( e.Rules.All ( x => x.Conditions.First ().Operator == Grammar.IsOperator ), "shoud be IS operator" );


            var e2 = parser.Entities.ElementAt ( 1 );
            Assert.That ( e2.Name == "GD_PVO_Question2" );
            Assert.That ( e2.Defaults.Count () == 6, "should have 6 defaults but has " + e2.Defaults.Count () );
            Assert.That ( e2.Rules.Count () == 2, "should have 2 rules but has " + e2.Rules.Count () );
            Assert.That ( e2.Rules.All ( x => x.Conditions.First ().Operand == "GD_PVO_Question2.Value" ), "error in operands" );
            Assert.That ( e2.Rules.All ( x => x.Conditions.First ().Operator == Grammar.IsOperator ), "shoud be IS operator" );


        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile024 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File024 - Hyperlink.txt" );
            var e = parser.Entities.First ();
            Assert.That ( e.Name == "GD_PVO_Link2", "Wrong entity name" );
            Assert.That ( e.Defaults.Count () == 4, "should have 4 defaults but has " + e.Defaults.Count () );
            Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == "\"https://workspace.swe.siemens.com/content/70000071/07/22978293/Docs/2.-Corporate%20Responsibility%20Self%20Assessment.doc\"" );

        }

        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile025 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File025.txt" );
            var e = parser.Entities.First ();
            Assert.That ( e.Name == "GD_Adr_POBoxPostalCode", "Wrong entity name" );
            Assert.That ( e.Defaults.Count () == 4, "should have 4 defaults but has " + e.Defaults.Count () );

            Assert.That ( e.Rules.Count () == 2, "should have 2 rule(s) but has " + e.Rules.Count () );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile026 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File026 - Tooltip support.txt" );
            var e = parser.Entities.First ();
            Assert.That ( e.Name == "E_GD_VE_SmtpAddr", "Wrong entity name" );
            Assert.That ( e.Defaults.Count () == 5, "should have 5 defaults but has " + e.Defaults.Count () );
            Assert.That ( e.Defaults.First ().Token == Grammar.ToolTipDefaultClause );
            Assert.That ( e.Defaults.First ().Value.ToString () == "\"the email is used generally for all the automatic communication with the supplier - e.g. payment advice\"" );

            Assert.That ( e.Rules.Count () == 1, "should have 1 rule(s) but has " + e.Rules.Count () );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ParseSampleFile027 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File027 - Save Data To.txt" );
            var e = parser.Entities.First ();
            Assert.That ( e.Name == "btnInsert", "Wrong entity name" );
            Assert.That ( e.Defaults.Count () == 3, "should have 3 defaults but has " + e.Defaults.Count () );
            Assert.That ( e.Defaults.First ().Token == Grammar.ValueDefaultClause, "Expected other default clause, not " + e.Defaults.First ().Token );
            Assert.That ( e.Defaults.First ().Value.ToString () == "LABELS.GENERIC.labInsert", "Expected  e.Defaults.First ().Token, not" + e.Defaults.First ().Value.ToString () );

            Assert.That ( e.ConditionlessActions.Count () == 1, "should have 1 ConditionlessActions but has " + e.ConditionlessActions.Count () );
            Assert.That ( e.ConditionlessActions.ElementAt ( 0 ).ToString () == "save_data_to DATASOURCE.MDM_Requests_PhoneEntry this" );

            Assert.That ( e.ConditionlessActions.ElementAt ( 0 ).Action == Grammar.SaveDataUnaryActionSymbol );
            Assert.That ( e.ConditionlessActions.ElementAt ( 0 ).Reference == "this" );
            Assert.That ( e.ConditionlessActions.ElementAt ( 0 ).IsResultAction );

            Assert.That ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Value.ToString() == "DATASOURCE.MDM_Requests_PhoneEntry" );
            Assert.That ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.Count () == 1 );
            Assert.That ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Key == "Phone" );
            Assert.That ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Value == "123456" );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        [ExpectedException ( ExpectedException = typeof ( DuplicateEntityFoundException ),
            ExpectedMessage = "A duplicate declaration was found for Entity 'TBVendorCity' in line 9" )]
        public void ParseSampleFile028 ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File028 - No same entity name twice.txt" );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ShouldFindEntities ()
        {
            var parser = new Parser ();
            parser.ParseRuleSet ( @"..\..\sample files\vendor-rules.txt" );

            //Assert.That ( parser.Entities.Count () == 18 );

            var parser2 = new Parser ();
            var eList = parser2.ParseRuleSet ( @"..\..\sample files\vendor-rules.txt" );



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
            Assert.That ( parser.Entities.ElementAt ( 3 ).Rules.Count () == 2 );
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
            Assert.That ( parser.Entities.ElementAt ( 9 ).ConditionlessActions.Count () == 2 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Rules.Count () == 9 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Triggers.Count () == 1 );
            Assert.That ( parser.Entities.ElementAt ( 9 ).Constraints.Count () == 0 );

        }

        // ---------------------------------------------------------------------------------


    }
}
