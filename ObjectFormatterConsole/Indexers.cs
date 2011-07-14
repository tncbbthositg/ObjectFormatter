using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectFormatterConsole
{
    class Indexers
    {
        public string this[int i]
        {
            get
            {
                return string.Format("{{{0}}}", i);
            }
        }

        public string this[string s]
        {
            get
            {
                return string.Format("\"{0}\"", s);
            }
        }

        public string this[int i, string s]
        {
            get
            {
                return string.Format("{0}:{1}", this[i], this[s]);
            }
        }
    }
}
