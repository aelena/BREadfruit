using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit
{
    public class Trigger
    {
        private readonly string _triggerName;
        /// <summary>
        /// Gets an event descriptor that indicates the event 
        /// that this trigger represents
        /// </summary>
        public string Event
        {
            get { return _triggerName; }
        }

        // ---------------------------------------------------------------------------------


        private readonly string _elementName;
        /// <summary>
        /// Gets an event descriptor that indicates the element 
        /// on which the event is declared
        /// </summary>
        public string Target
        {
            get { return _elementName; }
        }

        // ---------------------------------------------------------------------------------


        public Trigger ( string elementName, string eventName )
        {
            this._elementName = elementName;
            this._triggerName = eventName;
        }

        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return String.Format ( "{0} {1}", this.Target, this.Event );
        }

        // ---------------------------------------------------------------------------------

    }
}
