using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class LineInfoTests
    {
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", Result = false )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with args", Result = true )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with_args", Result = true )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments", Result = true )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with argments", Result = false )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with argUments", Result = false )]
        public bool LineInfo_HasSymbolTests_1 ( string line )
        {
            var li = new LineInfo ( line );
            return li.HasSymbol ( Grammar.WithArgumentsSymbol );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", true, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", false, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with args XX_XX", true, Result = 3 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with args XX_XX", false, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with_args", true, Result = 3 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with_args", false, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments", true, Result = 3 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments", false, Result = 2 )]
        // this and next are 4 because token is misspelled and thus not recognized during TokenizeMultiplePartOperators' run
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with argUments", true, Result = 4 )]      
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with argUments", false, Result = 4 )]
        public int LineInfo_RemoveTokensAfterSymbolTests_1 ( string line, bool includeCurrentToken )
        {
            var li = LineParser.ParseLine ( LineParser.TokenizeMultiplePartOperators ( new LineInfo ( line ) ) );
            return li.RemoveTokensAfterSymbol ( Grammar.WithArgumentsSymbol, includeCurrentToken ).Count ();
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "load data from DATASORCE.ENTERPRISE.WORLD_COUNTRIES", Result = false )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with args", Result = true )]
        [TestCase ( "load data from DATAsource.ENTERPRISE.WORLD_COUNTRIES with arguments", Result = false )]
        public bool LineInfo_HasSymbolTests_2 ( string line )
        {
            var li = new LineInfo ( line );
            return li.HasSymbol ( Grammar.DataSourceSymbol );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", 1, Result = 1 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", 2, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", 3, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with args", 2, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with_args", 2, Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with_args", 5, Result = 3 )]
        public int LineInfo_RemoveTokensFromIndex_Tests ( string line, int index )
        {
            var li = LineParser.ParseLine ( LineParser.TokenizeMultiplePartOperators ( new LineInfo ( line ) ) );
            li.RemoveTokensFromIndex ( index );
            return li.Tokens.Count ();
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", Result = -1 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with args", Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with_args", Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments", Result = 2 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with argments", Result = -1 )]
        [TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with argUments", Result = -1 )]
        public int LineInfo_IndexOfSymbol_Tests1 ( string line )
        {
            var li = LineParser.ParseLine ( LineParser.TokenizeMultiplePartOperators ( new LineInfo ( line ) ) );
            return li.IndexOfSymbol ( Grammar.WithArgumentsSymbol );
        }


        // ---------------------------------------------------------------------------------


    }
}
