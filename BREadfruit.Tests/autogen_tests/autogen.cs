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
/* ************************************************************
 *                                                            *
 * AUTOGENERATED ON : 19/12/2014 10:39:28	
 *                                                            *
************************************************************* */
namespace BREadfruit.Tests.Autogenerated
{
	[TestFixture]
	public class EntityTests
	{
		[Test]
		public void TestEntity_rbSpiridon ()
		{
			var parser = new Parser ();
			parser.ParseRuleSetAsString ( @"; TESTGEN : DEFAULTS=4
Entity rbSpiridon is RadioButton in frmSearch
	with defaults
		enabled false
		label ""Spiridon""
		value true				; means checked
; ----------------------------------------------------------------------------	
" 
			);
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "rbSpiridon", "Entity name should be 'rbSpiridon' but is " + e.Name );
			Assert.That ( e.Defaults.Count() == 4, "Should have '4' default clauses but has " + e.Defaults.Count());
			Assert.That ( e.ConditionlessActions.Count() == 0, "Should have '0' Conditionless Actions but has " + e.ConditionlessActions.Count());
			Assert.That ( e.Rules.Count() == 0, "Should have '0' Rules but has " + e.Rules.Count());
			Assert.That ( e.Triggers.Count() == 0, "Should have '0' Triggers but has " + e.Triggers.Count());
			Assert.That ( e.Constraints.Count() == 0, "Should have '0' Constraints but has " + e.Constraints.Count());
		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void TestEntity_rbEPass ()
		{
			var parser = new Parser ();
			parser.ParseRuleSetAsString ( @"; TESTGEN : DEFAULTS=4
Entity rbEPass is RadioButton in frmSearch
	with defaults
		enabled false
		label ""Epass""
		value false				; means unchecked
; ----------------------------------------------------------------------------	
" 
			);
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "rbEPass", "Entity name should be 'rbEPass' but is " + e.Name );
			Assert.That ( e.Defaults.Count() == 4, "Should have '4' default clauses but has " + e.Defaults.Count());
			Assert.That ( e.ConditionlessActions.Count() == 0, "Should have '0' Conditionless Actions but has " + e.ConditionlessActions.Count());
			Assert.That ( e.Rules.Count() == 0, "Should have '0' Rules but has " + e.Rules.Count());
			Assert.That ( e.Triggers.Count() == 0, "Should have '0' Triggers but has " + e.Triggers.Count());
			Assert.That ( e.Constraints.Count() == 0, "Should have '0' Constraints but has " + e.Constraints.Count());
		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void TestEntity_TBVendorName ()
		{
			var parser = new Parser ();
			parser.ParseRuleSetAsString ( @"; TESTGEN : DEFAULTS=4, ACTIONS=3, TRIGGERS=1
Entity TBVendorName is TextBox in frmSearch
	with defaults
		enabled true
		visible
		value   ''
		label LABELS.NAME
	with actions
		hide btnCreateVendor
		set hidden vendorSearchResultGrid
		clear vendorSearchResultGrid
	with rules
	with triggers
		TBVendorName.value changes
; ----------------------------------------------------------------------------	
" 
			);
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorName", "Entity name should be 'TBVendorName' but is " + e.Name );
			Assert.That ( e.Defaults.Count() == 4, "Should have '4' default clauses but has " + e.Defaults.Count());
			Assert.That ( e.ConditionlessActions.Count() == 3, "Should have '3' Conditionless Actions but has " + e.ConditionlessActions.Count());
			Assert.That ( e.Rules.Count() == 0, "Should have '0' Rules but has " + e.Rules.Count());
			Assert.That ( e.Triggers.Count() == 1, "Should have '1' Triggers but has " + e.Triggers.Count());
			Assert.That ( e.Constraints.Count() == 0, "Should have '0' Constraints but has " + e.Constraints.Count());
		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void TestEntity_TBVendorCity ()
		{
			var parser = new Parser ();
			parser.ParseRuleSetAsString ( @"; TESTGEN : DEFAULTS=4, ACTIONS=3, TRIGGERS=1
Entity TBVendorCity is TextBox in frmSearch
	with defaults
		visible true
		value """"
		label LABELS.labVendorCity
	with actions
		hide btnCreateVendor
		set hidden vendorSearchResultGrid
		clear vendorSearchResultGrid
	with triggers
		this.value on change
; ----------------------------------------------------------------------------	
" 
			);
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorCity", "Entity name should be 'TBVendorCity' but is " + e.Name );
			Assert.That ( e.Defaults.Count() == 4, "Should have '4' default clauses but has " + e.Defaults.Count());
			Assert.That ( e.ConditionlessActions.Count() == 3, "Should have '3' Conditionless Actions but has " + e.ConditionlessActions.Count());
			Assert.That ( e.Rules.Count() == 0, "Should have '0' Rules but has " + e.Rules.Count());
			Assert.That ( e.Triggers.Count() == 1, "Should have '1' Triggers but has " + e.Triggers.Count());
			Assert.That ( e.Constraints.Count() == 0, "Should have '0' Constraints but has " + e.Constraints.Count());
		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void TestEntity_TBVendorNumber ()
		{
			var parser = new Parser ();
			parser.ParseRuleSetAsString ( @"; TESTGEN : DEFAULTS=7, ACTIONS=3, RULES=2, CONSTRAINTS=1,TRIGGERS=1
Entity TBVendorNumber is TextBox in frmSearch
	with defaults
		visible true
		value ''
		max length 10
		min length 10
		label LABELS.labVendorNumber
		validation ^[0-9]{10}$
	with actions
		hide btnCreateVendor
		set hidden vendorSearchResultGrid
		clear vendorSearchResultGrid
	with rules
		; show this message ""This partner is not managed by Madam. Please contact GSS master data team."" if starting with 0
		TBVendorNumber.text starts with '0' then
			set value MESSAGES.VALIDATIONS.SEARCH.NOTMANAGED in MESSAGE_POPUP
		TBVendorNumber.text not starts with '0' then
			set value '' in MESSAGE_POPUP
	with triggers
		this.value changes
	with constraints
		only numbers
" 
			);
			Assert.That ( parser.Entities.Count () == 1 );
			var e = parser.Entities.First ();
			Assert.That ( e.Form == "frmSearch", "Entity form should be 'frmSearch' but is " + e.Form );
			Assert.That ( e.Name == "TBVendorNumber", "Entity name should be 'TBVendorNumber' but is " + e.Name );
			Assert.That ( e.Defaults.Count() == 7, "Should have '7' default clauses but has " + e.Defaults.Count());
			Assert.That ( e.ConditionlessActions.Count() == 3, "Should have '3' Conditionless Actions but has " + e.ConditionlessActions.Count());
			Assert.That ( e.Rules.Count() == 2, "Should have '2' Rules but has " + e.Rules.Count());
			Assert.That ( e.Triggers.Count() == 1, "Should have '1' Triggers but has " + e.Triggers.Count());
			Assert.That ( e.Constraints.Count() == 1, "Should have '1' Constraints but has " + e.Constraints.Count());
		}


		// ---------------------------------------------------------------------------------

	}
}