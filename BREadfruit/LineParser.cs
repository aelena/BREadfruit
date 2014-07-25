using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit
{
    internal class LineParser
    {

        public LineInfo ParseLine(string line)
        {

            var indentLevel = line.ToCharArray ().Where ( x => x == '\t' ).Count ();


            return new LineInfo ( indentLevel, null );

        }

    }
}
