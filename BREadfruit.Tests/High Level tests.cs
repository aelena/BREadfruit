#define DISABLECONSTRAINTS

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
#if !DISABLECONSTRAINTS
			Assert.That ( e.Constraints.Count () == 0 );
#endif
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
#if !DISABLECONSTRAINTS
			Assert.That ( e.Constraints.Count () == 0 );
#endif
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
			Assert.That ( e.Defaults.ElementAt ( 3 ).ToString ().Trim () == "load_data_from DATASOURCE.WORLD_COUNTRIES" );
			Assert.That ( e.Defaults.ElementAt ( 4 ).ToString () == "label LABELS.GENERIC.COUNTRY" );
			Assert.That ( e.ConditionlessActions.Count () == 4 );
			Assert.That ( e.ConditionlessActions.First ().ToString () == "hide vendorSearchResultGrid" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 1 ).ToString () == "hide btnCreateVendor" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 2 ).ToString () == "load_data_from DATASOURCE.WORLD_COUNTRIES" );
			Assert.That ( e.Rules.Count () == 4 );
#if !DISABLECONSTRAINTS
			Assert.That ( e.Constraints.Count () == 0 );
#endif
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
			Assert.IsTrue ( e.Defaults.ElementAt ( 0 ).Token == Grammar.LoadDataDefaultClause );
			Assert.That ( e.Defaults.First ().Arguments.Last ().Value == "\"Anything Goes\"" );
#if !DISABLECONSTRAINTS
			Assert.That ( e.Constraints.Count () == 1 );
			Assert.That ( e.Constraints.First ().Name == "only_numbers" );
			Assert.IsTrue ( e.Constraints.ElementAt ( 0 ) == Grammar.OnlyNumbersConstraintSymbol );
#endif


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
			Assert.That ( e.Triggers.Count () == 6, "Expected 6 triggers" );
			Assert.That ( e.Triggers.First ().ToString () == "TBVendorCity.value changed", "wrong trigger expression(1)." );
			Assert.That ( e.Triggers.ElementAt ( 1 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(2)." );
			Assert.That ( e.Triggers.ElementAt ( 2 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(3)." );
			Assert.That ( e.Triggers.ElementAt ( 3 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(4)." );
			Assert.That ( e.Triggers.ElementAt ( 4 ).ToString () == "TBVendorCity clicked", "wrong trigger expression(5)." );
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
			Assert.That ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 2 ) ).Arguments.Count () == 1 );

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

			Assert.That ( e.Rules.Count () == 3, "should have 3 rule(s) but has " + e.Rules.Count () );
			Assert.That ( e.Rules.All ( x => x.Conditions.First ().ResultActions.First ().Action == Grammar.ToolTipDefaultClause.Token ) );
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

			Assert.That ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Value.ToString () == "DATASOURCE.MDM_Requests_PhoneEntry" );
			Assert.That ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.Count () == 1 );
			Assert.That ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Key == "Phone" );
			Assert.That ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Value == "123456" );
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
		public void ParseSampleFile029 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File029 - Add Value.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Defaults.Count () == 3, "should have 3 defaults but has " + e.Defaults.Count () );

			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ) == Grammar.AddValueActionSymbol );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ) == Grammar.AddValueActionSymbol );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).IsResultAction );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ).IsResultAction );

			Assert.IsTrue ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Reference == "PaymentTermsOptions" );
			Assert.IsTrue ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Reference == "PaymentTermsOptions_2" );

			Assert.IsTrue ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Value.ToString () == "\"BIT$ - Payment via BitCoin\"" );
			Assert.IsTrue ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Value.ToString () == "'BIT$ - Payment via BitCoin'" );

			Assert.IsTrue ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Token == Grammar.AddValueActionSymbol );
			Assert.IsTrue ( ( ( ResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Token == Grammar.AddValueActionSymbol );

			Assert.That ( e.ConditionlessActions.All ( x => x.Property == PropertyType.NOT_SET ) );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile030 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File030 - Load Data in.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Defaults.Count () == 2, "should have 2 defaults but has " + e.Defaults.Count () );

			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).Action == Grammar.LoadDataUnaryActionSymbol.Token );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).IsResultAction );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Key == "COUNTRY" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Value.ToString () == "F_GD_VF_Country.Value" );

			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Reference == "F_GD_VF_CountryCode" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Value.ToString () == "DATASOURCE.MDM_CountryTelephonePrefixes" );

			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ).Action == Grammar.LoadDataUnaryActionSymbol.Token );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 1 ).IsResultAction );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Arguments.First ().Key == "COUNTRY" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Arguments.First ().Value.ToString () == "F_GD_VF_Country.Value" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Reference == "F_GD_VF_CountryCode" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 1 ) ).Value.ToString () == "JAVASCRIPT.FUNCTION_NAME" );


			Assert.That ( e.Triggers.Count () == 1, "should have 1 trigger(s)but has " + e.Triggers.Count () );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile031 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File031 - GD_Ctr_CustomerNumber.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Rules.Count () == 5, "should have 5 defaults but has " + e.Rules.Count () );

			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().ToString () == "REQUEST.STATUS is_not REQUEST.CLOSED" );
			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.ElementAt ( 0 ).ResultActions.Count () == 2 );


			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 0 ).ToString () == "REQUEST.STATUS is_not REQUEST.CLOSED" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ToString () == "REQUEST.REQUEST_TYPE is \"VM\"" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 2 ).ToString () == "PO_ReturnsVendor.Checked is true" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 3 ).ToString () == "GD_Ctr_CustomerNumber.OldDisplayValue == \"\"" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 3 ).Results.Count () == 1 );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 3 ).Results.First ().IsUnary );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 3 ).Results.First ().Token == Grammar.MakeMandatoryUnaryActionSymbol.Token );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 0 ).ToString () == "REQUEST.STATUS is_not REQUEST.CLOSED" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ToString () == "REQUEST.REQUEST_TYPE is \"VM\"" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 2 ).ToString () == "PO_ReturnsVendor.Checked is true" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 3 ).ToString () == "GD_Ctr_CustomerNumber.OldDisplayValue != \"\"" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 3 ).Results.Count () == 1 );

			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 0 ).ToString () == "REQUEST.STATUS is_not REQUEST.CLOSED" );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).ToString () == "REQUEST.REQUEST_TYPE is \"VM\"" );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).ToString () == "PO_ReturnsVendor.Checked is false" );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).Results.Count () == 2 );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).Results.First ().Action == Grammar.MakeNonMandatoryUnaryActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).Results.First ().Reference == "GD_Ctr_CustomerNumber" );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 1 ).Action == Grammar.MakeMandatoryUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 1 ).Reference == "GD_Abc_DEMO" );

			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 0 ).ToString () == "REQUEST.STATUS is_not REQUEST.CLOSED" );
			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 1 ).ToString () == "REQUEST.REQUEST_TYPE is_not \"VM\"" );
			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 1 ).Results.Count () == 1 );

		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void ParseSampleFile032 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File032 - Rules with integers.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "GD_Ctr_Fictitious", "Entity name should be 'GD_Ctr_Fictitious' but is " + e.Name );

			Assert.That ( Int32.Parse ( e.Defaults.First ().Value.ToString () ) == 1234 );

			Assert.That ( Int32.Parse ( e.Rules.First ().Conditions.First ().Value.ToString () ) == 5140 );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Value.ToString () == "{5924,592F}" );


			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().ResultActions.First ().Value.ToString () == "3M60" );

		}

		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile033 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File033.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "PO_ReturnsVendor", "Entity name should be 'PO_ReturnsVendor' but is " + e.Name );

			Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == "false" );

			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 0 ).Action == Grammar.MakeMandatoryUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 0 ).Reference == "GD_Ctr_CustomerNumber" );
			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 1 ).Action == Grammar.EnableUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 1 ).Reference == "GD_Ctr_CustomerNumber" );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 0 ).Action == Grammar.MakeNonMandatoryUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 0 ).Reference == "GD_Ctr_CustomerNumber" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 1 ).Action == Grammar.DisableUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 2 ).Results.ElementAt ( 1 ).Reference == "GD_Ctr_CustomerNumber" );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).Results.ElementAt ( 0 ).Action == Grammar.DisableUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).Results.ElementAt ( 0 ).Reference == "GD_Ctr_CustomerNumber" );

			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).Results.ElementAt ( 0 ).Action == Grammar.MakeMandatoryUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).Results.ElementAt ( 0 ).Reference == "GD_Ctr_CustomerNumber" );

			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 0 ).Results.ElementAt ( 0 ).Action == Grammar.NotVisibleUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 0 ).Results.ElementAt ( 0 ).Reference == "GD_Ctr_CustomerNumber" );

		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void ParseSampleFile034 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File034 - GD_NatP_PlaceOfBirth.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "GD_NatP_PlaceOfBirth", "Entity name should be 'GD_NatP_PlaceOfBirth' but is " + e.Name );

			Assert.That ( e.Defaults.First ().Token == Grammar.MaxlengthDefaultClause.Token );
			Assert.That ( e.Defaults.First ().Value.ToString () == "25" );
			Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.LabelDefaultClause.Token );
			Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == "LABELS.labPlaceOfBirth" );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "REQUEST.COMPANY_CODE" );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.IsOperator );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "\"5240\"" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).Operand == "GD_Ctr_NIF.Value" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).Operator == Grammar.StartsWithOperator );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).Value.ToString () == "{1,2}" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Operand == "GD_Ctr_TaxCode1.Value" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Operator == Grammar.StartsWithOperator );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Value.ToString () == "{1,2}" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.First ().Action == Grammar.LabelDefaultClause.Token );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.First ().Value.ToString () == "Nº Employees" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Results.First ().Action == Grammar.MakeMandatoryUnaryActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Results.First ().Reference == "GD_NatP_PlaceOfBirth" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Results.Last ().Action == Grammar.MakeMandatoryUnaryActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Results.Last ().Reference == "GD_NatP_PlaceOfBirth" );


		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile035 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File035 - GD_NatP_Profession.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "GD_NatP_Profession", "Entity name should be 'GD_NatP_Profession' but is " + e.Name );

			Assert.That ( e.Defaults.First ().Token == Grammar.MaxlengthDefaultClause.Token );
			Assert.That ( e.Defaults.First ().Value.ToString () == "30" );
			Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.LabelDefaultClause.Token );
			Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == "LABELS.labProfession" );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "VENDOR.ACCOUNT_GROUP" );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "{YVT6,YVT9}" );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First () == Grammar.VisibleUnaryActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "GD_NatP_Profession" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "false" );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First ().Action == Grammar.VisibleUnaryActionSymbol.Token );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First ().Value.ToString () == "false" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().ResultActions.First ().Reference == "GD_NatP_Profession" );

			// this rule was commented in the test file....
			//Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Results.First ().Action == Grammar.VisibleUnaryActionSymbol.Token );
			//Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Results.First ().Reference == "GD_NatP_Profession" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile036 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File036.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "GD_NatP_Exemption", "Entity name should be 'GD_NatP_Exemption' but is " + e.Name );

			Assert.That ( e.Defaults.ElementAt ( 0 ).Token == Grammar.LabelDefaultClause.Token );
			Assert.That ( e.Defaults.ElementAt ( 0 ).Value.ToString () == "LABELS.labExemption" );
			Assert.That ( e.Defaults.ElementAt ( 1 ).Token == Grammar.VisibleDefaultClause.Token );
			Assert.That ( e.Defaults.ElementAt ( 1 ).Value.ToString () == "false" );
			Assert.That ( e.Defaults.ElementAt ( 2 ).Token == Grammar.LoadDataDefaultClause.Token );
			Assert.That ( e.Defaults.ElementAt ( 2 ).Value.ToString () == "DATASOURCE.MDM_NaturalPersonExemptions" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Token == Grammar.ValueDefaultClause.Token );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Value.ToString () == "" );


			// REQUEST.VENDOR_COUNTRY is PT and GD_Ctr_NIF starts with 1 or GD_Ctr_NIF starts with 2
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "REQUEST.VENDOR_COUNTRY", "Operand should not be " + e.Rules.First ().Conditions.First ().Operand );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.IsOperator, "Expected operator was " + Grammar.IsOperator.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "PT", "Unexpected Value found" );
			Assert.That ( e.Rules.First ().Conditions.First ().SuffixLogicalOperator == Grammar.ANDSymbol, "Unexpected logical suffix" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).Operand == "GD_Ctr_NIF" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).Operator == Grammar.StartsWithOperator );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).Value.ToString () == "1" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).SuffixLogicalOperator == Grammar.ORSymbol );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Operand == "GD_Ctr_NIF" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Operator == Grammar.StartsWithOperator );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Value.ToString () == "2" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.First ().Action == Grammar.VisibleUnaryActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.First ().Value.ToString () == "True" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.First ().Reference == "CC_AM_ExemptionNumber" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.ElementAt ( 1 ).Action == Grammar.LabelDefaultClause.Token );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.ElementAt ( 1 ).Value.ToString () == "LABELS.SOCIAL_SEC_SITUATION" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).ResultActions.ElementAt ( 1 ).Reference == "GD_NatP_Exemption" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Results.First ().Reference == "CC_AM_ExemptionNumber" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 2 ).Results.First ().Token == Grammar.MakeMandatoryUnaryActionSymbol.Token );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile037 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File037 - CC_AM_CashManagementGroup.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "CC_AM_CashManagementGroup", "Entity name should be 'CC_AM_CashManagementGroup' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "REQUEST.VENDOR_COUNTRY", "Operand should not be " + e.Rules.First ().Conditions.First ().Operand );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.IsOperator, "Expected operator was " + Grammar.IsOperator.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "REQUEST.COUNTRY", "Unexpected Value found" );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 0 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 0 ).ResultActions.First ().Value.ToString () == "A1" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 0 ).ResultActions.First ().Reference == "CC_AM_CashManagementGroup" );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 0 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 0 ).ResultActions.First ().Value.ToString () == "A2" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 0 ).ResultActions.First ().Reference == "CC_AM_CashManagementGroup" );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Value.ToString () == "A1" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Reference == "CC_AM_CashManagementGroup" );


		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile038 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File038 - CC_PT_PaymentMethods.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "CC_PT_PaymentMethods", "Entity name should be 'CC_PT_PaymentMethods' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 5 );

			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).ResultActions.First ().Value.ToString () == "G" );
			Assert.That ( e.Rules.First ().Conditions.ElementAt ( 1 ).ResultActions.First ().Reference == "CC_PT_PaymentMethods" );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Value.ToString () == "Y" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Reference == "CC_PT_PaymentMethods" );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 0 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 0 ).ResultActions.First ().Value.ToString () == "GY" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 0 ).ResultActions.First ().Reference == "CC_PT_PaymentMethods" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 0 ).Results.ElementAt ( 0 ).Action == Grammar.DisableUnaryActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.ElementAt ( 0 ).Results.ElementAt ( 0 ).Reference == "CC_PT_PaymentMethods" );

			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Action == Grammar.SetValueActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Value.ToString () == "2GHY" );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).ResultActions.First ().Reference == "CC_PT_PaymentMethods" );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).Results.ElementAt ( 0 ).Action == Grammar.DisableUnaryActionSymbol );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.ElementAt ( 1 ).Results.ElementAt ( 0 ).Reference == "CC_PT_PaymentMethods" );

			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 1 ).Results.First ().Action == Grammar.MandatoryDefaultClause );
			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.ElementAt ( 1 ).Results.First ().Reference == "CC_PT_HouseBank" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile039 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File039 - Hide.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "CC_DD_Level", "Entity name should be 'CC_DD_Level' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 3 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile040 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File040 - Count.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_SalesOffice", "Entity name should be 'SO_SalesOffice' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "SO_SalesOffice.Items.Count" );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.GreaterThanOperator );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "0" );

		}


		// ---------------------------------------------------------------------------------



		[Test]
		public void ParseSampleFile041 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File041 - ListBox.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_lbxSODSalesOrganization1", "Entity name should be 'SO_lbxSODSalesOrganization1' but is " + e.Name );
			Assert.That ( e.TypeDescription == Grammar.ListBoxSymbol.Token, "Entity name should be " + Grammar.ListBoxSymbol.Token + " but is " + e.TypeDescription );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile042 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File042 - Grid.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "P_GD_VT_Grid", "Entity name should be 'P_GD_VT_Grid' but is " + e.Name );
			Assert.That ( e.TypeDescription == Grammar.GridSymbol.Token, "Entity name should be " + Grammar.GridSymbol.Token + " but is " + e.TypeDescription );

			Assert.That ( e.Defaults.Count () == 9, "there should be 9 default clauses, but there are " + e.Defaults.Count () );

			Assert.That ( e.Defaults.First ().Token == Grammar.DefineColumnDefaultClause.Token );
			Assert.That ( e.Defaults.First ().Value.ToString () == "GRID_P_GD_VT_Country" );
			Assert.That ( e.Defaults.First ().Arguments.Count () == 4 );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 0 ).Key == "ControlType" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 0 ).Value == "DropDownList" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 1 ).Key == "DataField" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 1 ).Value == "GD_VT_Country" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 2 ).Key == "DataSource" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 2 ).Value == "DATASOURCE.MDM_Countries" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 3 ).Key == "Header" );
			Assert.That ( e.Defaults.First ().Arguments.ElementAt ( 3 ).Value == "LABELS.labCountry" );

			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 0 ).Key == "Constraints" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 0 ).Value == "Only_Numbers" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 1 ).Key == "ControlType" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 1 ).Value == "TextBox" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 2 ).Key == "DataField" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 2 ).Value == "GD_VT_Extension" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 3 ).Key == "Header" );
			Assert.That ( e.Defaults.ElementAt ( 3 ).Arguments.ElementAt ( 3 ).Value == "LABELS.labExtension" );

			Assert.That ( e.Triggers.Count () == 3 );
			Assert.That ( e.Triggers.First ().Event == Grammar.RowInsertedEventSymbol );
			Assert.That ( e.Triggers.First ().Target == e.Name );
			Assert.That ( e.Triggers.ElementAt ( 1 ).Event == Grammar.RowDeletedEventSymbol );
			Assert.That ( e.Triggers.ElementAt ( 1 ).Target == e.Name );
			Assert.That ( e.Triggers.ElementAt ( 2 ).Event == Grammar.RowUpdatedEventSymbol );
			Assert.That ( e.Triggers.ElementAt ( 2 ).Target == e.Name );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		[ExpectedException (
			ExpectedMessage = "Line 4 \t\tdefine_column GRID_P_GD_VT_Country with_args {ControlType:DropDownList,DataField:XYZ,Header:LABELS.labXYZ} caused exception : An unexpected clause was found\r\nCannot define a default column in an Entity that is not a grid." )]
		public void ParseSampleFile043 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File043 - InvalidGrid.txt" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		[ExpectedException ( ExpectedException = typeof ( UnexpectedClauseException ),
			ExpectedMessage = "An unexpected clause was found" + "\r\n" + "Cannot define a row trigger in an Entity that is not a grid." )]
		public void ParseSampleFile044 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File044 - InvalidGrid2.txt" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile045 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File045 - ArithOps.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "xARKANEx", "Entity name should be 'xARKANEx' but is " + e.Name );
			Assert.That ( e.TypeDescription == Grammar.TextBoxSymbol.Token, "Entity name should be " + Grammar.TextBoxSymbol.Token + " but is " + e.TypeDescription );

			Assert.That ( e.Rules.Count () == 10, "there should be 8 rules" );

			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Operator == Grammar.GreaterThanOperator );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operator == Grammar.GreaterThanOperator );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operator == Grammar.GreaterEqualThanOperator );
			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.First ().Operator == Grammar.GreaterEqualThanOperator );
			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.First ().Operator == Grammar.LowerThanOperator );
			Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.First ().Operator == Grammar.LowerThanOperator );
			Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.First ().Operator == Grammar.LowerThanOperator );
			Assert.That ( e.Rules.ElementAt ( 7 ).Conditions.First ().Operator == Grammar.LowerEqualThanOperator );
			Assert.That ( e.Rules.ElementAt ( 8 ).Conditions.First ().Operator == Grammar.LowerEqualThanOperator );
			Assert.That ( e.Rules.ElementAt ( 9 ).Conditions.First ().Operator == Grammar.LowerEqualThanOperator );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile046 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File046 - Is_Mandatory.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "xARKANEx", "Entity name should be 'xARKANEx' but is " + e.Name );
			Assert.That ( e.TypeDescription == Grammar.TextBoxSymbol.Token, "Entity name should be " + Grammar.TextBoxSymbol.Token + " but is " + e.TypeDescription );


		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile047 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File047 - [].txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "xARKANEx", "Entity name should be 'xARKANEx' but is " + e.Name );
			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "LU + GD_Ctr_PIVA.Value" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile048 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File048 - IsNumber etc.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "Sample", "Entity name should be 'Sample' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );

			Assert.That ( e.Rules.Count () == 12, "Should have 12 rules, but has" + e.Rules.Count () );


			Assert.That ( e.Rules.ElementAt ( 0 ).Conditions.First ().Operator.Token == Grammar.IsNumberOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operator.Token == Grammar.IsNumberOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operator.Token == Grammar.IsNumberOperator.Token );

			Assert.That ( e.Rules.ElementAt ( 3 ).Conditions.First ().Operator.Token == Grammar.IsNotNumberOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 4 ).Conditions.First ().Operator.Token == Grammar.IsNotNumberOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 5 ).Conditions.First ().Operator.Token == Grammar.IsNotNumberOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 6 ).Conditions.First ().Operator.Token == Grammar.IsNotNumberOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 7 ).Conditions.First ().Operator.Token == Grammar.IsNotNumberOperator.Token );

			Assert.That ( e.Rules.ElementAt ( 8 ).Conditions.First ().Operator.Token == Grammar.IsDecimalOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 9 ).Conditions.First ().Operator.Token == Grammar.IsNotDecimalOperator.Token );

			Assert.That ( e.Rules.ElementAt ( 10 ).Conditions.First ().Operator.Token == Grammar.IsAplhaOperator.Token );
			Assert.That ( e.Rules.ElementAt ( 11 ).Conditions.First ().Operator.Token == Grammar.IsAplhaOperator.Token );


		}


		// ---------------------------------------------------------------------------------



		[Test]
		public void ParseSampleFile049 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File049 - InRange.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "Sample", "Entity name should be 'Sample' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );
			Assert.That ( e.Rules.Count () == 2, "Should have 2 rules, but has" + e.Rules.Count () );

			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InRangeOperator, "Operator Error" );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "{723,1332}" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );

			Assert.That ( e.Rules.Last ().Conditions.First ().Operator == Grammar.NotInRangeOperator );
			Assert.That ( e.Rules.Last ().Conditions.First ().Value.ToString () == "{100,200}" );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First () == Grammar.SetValueActionSymbol );

		}


		// ---------------------------------------------------------------------------------



		[Test]
		public void ParseSampleFile050 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File050 - Clear.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "CP_btnClearAll", "Entity name should be 'CP_btnClearAll' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 3, "Should have 3 default clauses, but has" + e.Defaults.Count () );
			Assert.That ( e.Rules.Count () == 1, "Should have 1 rules, but has" + e.Rules.Count () );

			Assert.That ( e.Triggers.Count () == 1, "Should have 1 Triggers, but has" + e.Triggers.Count () );
			Assert.That ( e.ConditionlessActions.Count () == 1, "Should have 1 Conditionless Actions, but has" + e.ConditionlessActions.Count () );

			Assert.That ( e.ConditionlessActions.First ().Action == Grammar.ClearValueUnaryActionSymbol.Token );
			Assert.That ( e.Triggers.First ().Event == Grammar.ClickedEventSymbol.Token );
			Assert.That ( e.Triggers.First ().Target == "CP_btnClearAll" );


		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile051 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File051 - SetValue.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );
			Assert.That ( e.Rules.Count () == 1, "Should have 1 rules, but has" + e.Rules.Count () );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile052 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File052 - In Aliases.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );
			Assert.That ( e.Rules.Count () == 3, "Should have 3 rules, but has" + e.Rules.Count () );


			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operator == Grammar.InOperator );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operator == Grammar.InOperator );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );

		}


		[Test]
		public void ParseSampleFile053 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File053 - Not In Aliases.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );
			Assert.That ( e.Rules.Count () == 5, "Should have 5 rules, but has" + e.Rules.Count () );

			Assert.That ( e.Rules.All ( x => x.Conditions.First ().Operator == Grammar.NotInOperator ) );
			Assert.That ( e.Rules.All ( x => x.Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" ) );

			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.NotInOperator );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operator == Grammar.NotInOperator );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operator == Grammar.NotInOperator );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Reference == "SO_TaxClassification" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Value.ToString () == "01" );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Last ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile054 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File054 - ToUpper.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Defaults.Count () == 2, "Should have 2 default clauses, but has" + e.Defaults.Count () );
			Assert.That ( e.Rules.Count () == 1, "Should have 1 rules, but has" + e.Rules.Count () );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Action == Grammar.SetValueActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "GD_Adr_Name2" );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "toupper(GD_Adr_Name)" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.ElementAt ( 1 ).Value.ToString () == "tolower(GD_Adr_Name)" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile055 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File055 - Conds.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.Take ( 5 ).All ( x => x.SuffixLogicalOperator == "and" ) );
			Assert.That ( e.Rules.First ().Conditions.Last ().SuffixLogicalOperator == null );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile056 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File056 - Else01.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorName" );

			Assert.IsTrue ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorName" );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.Count () == 1 );

			Assert.IsTrue ( e.Rules.First ().Conditions.Last ().Else.First ().IsResultAction );
			Assert.That ( ( ( ResultAction ) e.Rules.First ().Conditions.Last ().Else.First () ).Value.ToString () == "'BBB'" );
			Assert.That ( ( ( ResultAction ) e.Rules.First ().Conditions.Last ().Else.First () ).Reference == "txtExample" );

			Assert.IsFalse ( e.Rules.Last ().HasElseClause );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile057 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File057 - Else02.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "SO_TaxClassification", "Entity name should be 'SO_TaxClassification' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorName" );

			Assert.IsTrue ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorName" );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.Count () == 4 );


			Assert.IsTrue ( e.Rules.First ().Conditions.Last ().Else.ElementAt ( 0 ).IsResultAction );
			Assert.That ( ( ( ResultAction ) e.Rules.First ().Conditions.Last ().Else.ElementAt ( 0 ) ).Value.ToString () == "'BBB'" );
			Assert.That ( ( ( ResultAction ) e.Rules.First ().Conditions.Last ().Else.ElementAt ( 0 ) ).Reference == "txtExample" );
			Assert.IsTrue ( e.Rules.First ().Conditions.Last ().Else.ElementAt ( 1 ).IsResultAction );
			Assert.That ( ( ( ResultAction ) e.Rules.First ().Conditions.Last ().Else.ElementAt ( 1 ) ).Value.ToString () == "Hello World" );
			Assert.That ( ( ( ResultAction ) e.Rules.First ().Conditions.Last ().Else.ElementAt ( 1 ) ).Reference == "SO_TaxClassification" );
			Assert.IsFalse ( e.Rules.First ().Conditions.Last ().Else.ElementAt ( 2 ).IsResultAction );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.ElementAt ( 2 ).Reference == "SO_TaxClassification" );
			Assert.IsFalse ( e.Rules.First ().Conditions.Last ().Else.ElementAt ( 3 ).IsResultAction );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.ElementAt ( 3 ).Reference == "SO_TaxClassification" );

			Assert.IsFalse ( e.Rules.Last ().HasElseClause );
		}


		// ---------------------------------------------------------------------------------


		/*
		 * TESTS FOR FILES 058, 059 and 060 are in testgen
		 */


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile061 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File061 - RealElse.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorIFA.value" );

			Assert.IsTrue ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorIFA.value" );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.Count () == 1 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile062 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File062 - NoElse.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorIFA.value" );

			Assert.IsFalse ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorIFA.value" );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.Count () == 0 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile063 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File063 - SingleLineElse.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorIFA.value" );

			Assert.IsTrue ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "TBVendorIFA.value" );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.Count () == 1 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile064 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File064 - SetLabel.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "TBVendorVAT" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Action == "label" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "LABELS.labSIRET" );

		}


		// ---------------------------------------------------------------------------------



		[Test]
		public void ParseSampleFile065 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File065 - NestedConds.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.That ( e.Rules.First ().Conditions.Last ().Else.Count () == 1 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile066 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File066 - SetIndex.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsFalse ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.Count () == 2 );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.All ( x => x.IsResultAction ) );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.All ( x => x.Value.ToString () == "1" ) );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Reference == "FIELD_A" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.Last ().Reference == "FIELD_B" );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.All ( x => x.Token == Grammar.SetIndexActionSymbol.Token ) );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile067 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File067 - Remove.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsFalse ( e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.Count () == 2 );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.All ( x => x.IsResultAction ) );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.All ( x => x.Value.ToString () == "1" ) );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.First ().Reference == "FIELD_A.Items" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.Last ().Reference == "FIELD_B.Items" );

			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.All ( x => x.Token == Grammar.RemoveValueActionSymbol.Token ) );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile068 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File068 - SetLabel.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "TBVendorVAT" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Action == Grammar.SetLabelActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Property == PropertyType.LABEL );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "LABELS.labSIRET" );

			Assert.That ( e.Rules.Last ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.Last ().HasElseClause );
			Assert.That ( e.Rules.Last ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.Last ().Conditions.First ().Operator == Grammar.NotInOperator );

			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Reference == "TBVendorVAT" );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Action == Grammar.SetLabelActionSymbol.Token );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Property == PropertyType.LABEL );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Value.ToString () == "LABELS.labSIRET" );

			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.Last ().Reference == "TBVendorVAT" );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.Last ().Action == Grammar.SetValueActionSymbol.Token );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.Last ().Property == PropertyType.VALUE );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.Last ().Value.ToString () == "0000000000" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile069 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File069 - Return.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );
			Assert.That ( e.Rules.Count () == 3 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.EqualityOperator );
			Assert.That ( e.Rules.First ().Conditions.First ().Value.ToString () == "\"\"" );
			Assert.That ( e.Rules.First ().Conditions.Last ().ResultActions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.Last ().Results.Count () == 1 );
			// check the return statement has been interpreted
			Assert.IsTrue ( e.Rules.First ().IsFinalRule );

			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.ElementAt ( 1 ).HasElseClause );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.Count () == 1 );
			Assert.That ( e.Rules.ElementAt ( 1 ).Conditions.First ().Operator == Grammar.InOperator );
			Assert.IsTrue ( e.Rules.ElementAt ( 1 ).IsFinalRule );

			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.ElementAt ( 2 ).HasElseClause );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.Count () == 1 );
			Assert.That ( e.Rules.ElementAt ( 2 ).Conditions.First ().Operator == Grammar.NotInOperator );
			Assert.IsTrue ( e.Rules.ElementAt ( 2 ).IsFinalRule );


		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile070 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File070 - SetIndex.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "FIELD_A" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Action == Grammar.SetIndexActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Property == PropertyType.INDEX );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "1" );

			Assert.That ( e.Rules.Last ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.Last ().HasElseClause );
			Assert.That ( e.Rules.Last ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.Last ().Conditions.First ().Operator == Grammar.NotInOperator );

			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Reference == "FIELD_A" );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Action == Grammar.SetIndexActionSymbol.Token );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Property == PropertyType.INDEX );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Value.ToString () == "1" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile071 ()
		{

			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File071 - MoreTriggers.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorIFA", "Entity name should be 'TBVendorIFA' but is " + e.Name );

			Assert.That ( e.Rules.First ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.First ().HasElseClause );
			Assert.That ( e.Rules.First ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.First ().Conditions.First ().Operator == Grammar.InOperator );

			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Reference == "FIELD_A" );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Action == Grammar.SetIndexActionSymbol.Token );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Property == PropertyType.INDEX );
			Assert.That ( e.Rules.First ().Conditions.First ().ResultActions.First ().Value.ToString () == "1" );

			Assert.That ( e.Rules.Last ().Conditions.First ().Operand == "DDLVDCountry.value" );
			Assert.IsTrue ( !e.Rules.Last ().HasElseClause );
			Assert.That ( e.Rules.Last ().Conditions.Count () == 1 );
			Assert.That ( e.Rules.Last ().Conditions.First ().Operator == Grammar.NotInOperator );

			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Reference == "FIELD_A" );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Action == Grammar.SetIndexActionSymbol.Token );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Property == PropertyType.INDEX );
			Assert.That ( e.Rules.Last ().Conditions.First ().ResultActions.First ().Value.ToString () == "1" );

			Assert.That ( e.Triggers.Count () == 4 );
			Assert.That ( e.Triggers.All ( x => x.Event == "changed" ) );
			Assert.That ( e.Triggers.Take ( 2 ).All ( x => x.Target == "TBVendorIFA" ) );
			Assert.That ( e.Triggers.ElementAt ( 2 ).Target == "TBVendorIFA.value" );
			Assert.That ( e.Triggers.Last ().Target == "FIELD_A.Value" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile072 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File072 - Load data 2.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Defaults.Count () == 2, "should have 2 defaults but has " + e.Defaults.Count () );

			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).Action == Grammar.LoadDataUnaryActionSymbol.Token );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).IsResultAction );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Key == "CODE" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Value.ToString () == "FIELD_X.Value" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.First ().ControlName == "FIELD_A" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.First ().DataFieldName == "Column A" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.Last ().ControlName == "FIELD_B" );
			Assert.IsTrue ( ( ( ParameterizedResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.Last ().DataFieldName == "Column B" );

			Assert.That ( e.Triggers.Count () == 1, "should have 1 trigger(s)but has " + e.Triggers.Count () );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile073 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File073 - Output in Default.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Defaults.Count () == 3, "should have 3 defaults but has " + e.Defaults.Count () );
			Assert.That ( e.Defaults.First ().OutputArguments.Count () == 2 );

			Assert.IsTrue ( e.Defaults.First ().OutputArguments.First ().ControlName == "FIELD_A" );
			Assert.IsTrue ( e.Defaults.First ().OutputArguments.First ().DataFieldName == "Column A" );

			Assert.IsTrue ( e.Defaults.First ().OutputArguments.Last ().ControlName == "FIELD_B" );
			Assert.IsTrue ( e.Defaults.First ().OutputArguments.Last ().DataFieldName == "Column B" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ParseSampleFile074 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File074 - Load Data with Query.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Defaults.Count () == 2, "should have 2 defaults but has " + e.Defaults.Count () );

			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).Action == Grammar.LoadDataUnaryActionSymbol.Token );
			Assert.IsTrue ( e.ConditionlessActions.ElementAt ( 0 ).IsResultAction );
			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Key == "CODE" );
			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Arguments.First ().Value.ToString () == "FIELD_X.Value" );
			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.First ().ControlName == "FIELD_A" );
			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.First ().DataFieldName == "Column A" );
			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.Last ().ControlName == "FIELD_B" );
			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).OutputArguments.Last ().DataFieldName == "Column B" );

			Assert.IsTrue ( ( ( QueryResultAction ) e.ConditionlessActions.ElementAt ( 0 ) ).Query == "QUERY TEXT GOES HERE" );

			Assert.That ( e.Triggers.Count () == 1, "should have 1 trigger(s)but has " + e.Triggers.Count () );

		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void ParseSampleFile075 ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\single entity tests\File075.txt" );
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmMain", "Entity form should be 'frmMain' but is " + e.Form );
			Assert.That ( e.Name == "GD_Adr_Street", "Entity name should be 'GD_Adr_Street' but is " + e.Name );
			Assert.That ( e.Defaults.Count () == 5, "should have 5 defaults but has " + e.Defaults.Count () );
			Assert.That ( e.ConditionlessActions.Count () == 10, "should have 10 actions but has " + e.ConditionlessActions.Count () );

			Assert.That ( e.ConditionlessActions.First ().Reference == "ATTACHMENT" );

			Assert.That ( e.ConditionlessActions.ElementAt ( 1 ).Reference == "COMMENT" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 2 ).Reference == "FIELD_A" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 3 ).Reference == "FIELD_B" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 4 ).Reference == "FIELD_C" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 5 ).Reference == "FIELD_D" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 6 ).Reference == "FIELD_E" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 7 ).Reference == "FIELD_F" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 8 ).Reference == "FIELD_G" );
			Assert.That ( e.ConditionlessActions.ElementAt ( 9 ).Reference == "FIELD_H" );


			Assert.That ( e.Rules.Last ().Conditions.Last ().ResultActions.First ().Value.ToString () == "32" );
			Assert.That ( e.Rules.Last ().Conditions.Last ().ResultActions.First ().Reference == "GD_Adr_Street" );
			Assert.That ( e.Rules.Last ().Conditions.Last ().ResultActions.First ().Token == Grammar.MaxlengthDefaultClause );

			Assert.That ( e.ConditionlessActions.All ( x => x.Property != PropertyType.NOT_SET ), "There are NOT_SET props" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ShouldFindEntities_Vendor ()
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

			// TBVendorCity
			Assert.That ( parser.Entities.ElementAt ( 1 ).Name == "TBVendorCity" );
			Assert.That ( parser.Entities.ElementAt ( 1 ).Defaults.Count () == 3 );
			Assert.That ( parser.Entities.ElementAt ( 1 ).ConditionlessActions.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 1 ).Rules.Count () == 0 );
			Assert.That ( parser.Entities.ElementAt ( 1 ).ConditionlessActions.First ().ToString () == "hide btnCreateVendor" );
			Assert.That ( parser.Entities.ElementAt ( 1 ).ConditionlessActions.Last ().ToString () == "hide vendorSearchResultGrid" );
			Assert.That ( parser.Entities.ElementAt ( 1 ).Triggers.Count () == 1 );


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


			Assert.That ( parser.Entities.ElementAt ( 3 ).Name == "TBVendorIFA" );
			Assert.That ( parser.Entities.ElementAt ( 3 ).Defaults.Count () == 3 );
			Assert.That ( parser.Entities.ElementAt ( 3 ).ConditionlessActions.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 3 ).Rules.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 3 ).Triggers.Count () == 1 );
			Assert.That ( parser.Entities.ElementAt ( 3 ).Triggers.First ().Event == "changed" );
			Assert.That ( parser.Entities.ElementAt ( 3 ).Triggers.First ().Target == "TBVendorIFA.value" );


			Assert.That ( parser.Entities.ElementAt ( 4 ).Name == "DDLVDCountry" );
			Assert.That ( parser.Entities.ElementAt ( 4 ).Defaults.Count () == 5 );
			Assert.That ( parser.Entities.ElementAt ( 4 ).ConditionlessActions.Count () == 4 );
			Assert.That ( parser.Entities.ElementAt ( 4 ).Rules.Count () == 4 );
			Assert.That ( parser.Entities.ElementAt ( 4 ).Triggers.Count () == 1 );


			Assert.That ( parser.Entities.ElementAt ( 5 ).Name == "TBVendorVAT" );
			Assert.That ( parser.Entities.ElementAt ( 5 ).Defaults.Count () == 3 );
			Assert.That ( parser.Entities.ElementAt ( 5 ).ConditionlessActions.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 5 ).Rules.Count () == 0 );
			Assert.That ( parser.Entities.ElementAt ( 5 ).Triggers.Count () == 1 );


			Assert.That ( parser.Entities.ElementAt ( 6 ).Name == "DDLCDCountry" );
			Assert.That ( parser.Entities.ElementAt ( 6 ).Defaults.Count () == 4 );
			Assert.That ( parser.Entities.ElementAt ( 6 ).ConditionlessActions.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 6 ).Rules.Count () == 0 );
			Assert.That ( parser.Entities.ElementAt ( 6 ).Triggers.Count () == 1 );


			Assert.That ( parser.Entities.ElementAt ( 7 ).Name == "DDLCDCompany" );
			Assert.That ( parser.Entities.ElementAt ( 7 ).Defaults.Count () == 3 );
			Assert.That ( parser.Entities.ElementAt ( 7 ).ConditionlessActions.Count () == 1 );
			Assert.That ( parser.Entities.ElementAt ( 7 ).Rules.Count () == 0 );
			Assert.That ( parser.Entities.ElementAt ( 7 ).Triggers.Count () == 1 );


			Assert.That ( parser.Entities.ElementAt ( 8 ).Name == "DDLCDPurchOrg" );
			Assert.That ( parser.Entities.ElementAt ( 8 ).Defaults.Count () == 3 );
			Assert.That ( parser.Entities.ElementAt ( 8 ).ConditionlessActions.Count () == 0 );
			Assert.That ( parser.Entities.ElementAt ( 8 ).Rules.Count () == 0 );
			Assert.That ( parser.Entities.ElementAt ( 8 ).Triggers.Count () == 0 );


			Assert.That ( parser.Entities.ElementAt ( 9 ).Name == "btnSearch" );
			Assert.That ( parser.Entities.ElementAt ( 9 ).Defaults.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 9 ).ConditionlessActions.Count () == 2 );
			Assert.That ( parser.Entities.ElementAt ( 9 ).Rules.Count () == 9 );
			Assert.That ( parser.Entities.ElementAt ( 9 ).Triggers.Count () == 1 );

		}

		// ---------------------------------------------------------------------------------

		[Test]
		public void ShouldFindEntities_Customer ()
		{
			var parser = new Parser ();
			parser.ParseRuleSet ( @"..\..\sample files\customer-rules.txt" );

			//Assert.That ( parser.Entities.Count () == 18 );

			//var parser2 = new Parser ();
			//var eList = parser2.ParseRuleSet ( @"..\..\sample files\vendor-rules.txt" );

		}



	}
}
