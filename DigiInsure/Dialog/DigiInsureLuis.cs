using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiInsure.Dialog
{
        public class Action
        {
            public bool triggered { get; set; }
            public string name { get; set; }
            public IList<object> parameters { get; set; }
        }

        public class Intent
        {
            public string intent { get; set; }
            public double score { get; set; }
            public IList<Action> actions { get; set; }
        }

        public class DigiInsureLuis
        {
            public string query { get; set; }
            public IList<Intent> intents { get; set; }
            public IList<object> entities { get; set; }
        }
}