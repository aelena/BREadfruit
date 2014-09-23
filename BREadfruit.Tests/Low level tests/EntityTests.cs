using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class EntityTests
    {
        [TestCase ( "only_ascii", Result = true )]
        [TestCase ( "only_numbers", Result = true )]
        [TestCase ( "only_letters", Result = true )]
        public bool Entity_AddConstraint_Tests ( string constraintToken )
        {
            var c = new Constraint ( constraintToken );
            var e = new Entity ( "sample", "TextBox" );
            e.AddConstraint ( c );

            Assert.That ( e.Constraints.Count () == 1 );
            return e.Constraints.First ().Name == constraintToken;
        }

        [TestCase ( "only_ascii_", Result = false )]
        [TestCase ( "only_cucumbers", Result = false )]
        [TestCase ( "just_letters", Result = false )]
        [ExpectedException(typeof(InvalidOperationException))]
        public bool Entity_AddConstraint_Tests_Exceptions ( string constraintToken )
        {
            var c = new Constraint ( constraintToken );
            var e = new Entity ( "sample", "TextBox" );
            e.AddConstraint ( c );

            Assert.That ( e.Constraints.Count () == 0 );
            return e.Constraints.First ().Name == constraintToken;
        }


        [TestCase ( "Entity XYZ is TextBox", Result = true )]
        [TestCase ( "Entity XYZ is DropDownList", Result = true )]
        [TestCase ( "Entity XYZ is Label", Result = true )]
        [TestCase ( "Entity XYZ is Div", Result = true )]
        [TestCase ( "Entity XYZ is Button  ", Result = true )]
        [TestCase ( "Entity XYZ is Button   ", Result = true )]
        [TestCase ( "Entity XYZ is FakeEntity", Result = false )]
        [TestCase ( "Entity XYZ is", Result = false )]
        [TestCase ( " Entity XYZ is", Result = false )]
        [TestCase ( "   Entity XYZ is", Result = false )]
        public bool ValidateEntityStatements(string statement)
        {
            var lp = new LineParser ();
            return lp.ValidateEntityStatement ( statement );
        }

    }
}
