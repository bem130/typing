using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace typing
{
    public class Log
    {
        public bool windowshow;
        public Log(int STATUS = 0, string EXPLANATION = "", string CATEGORY = "",string TIME = "")
        {
            status = STATUS;
            explanation = EXPLANATION;
            category = CATEGORY;
            time = TIME;
        }

        public int status { get; set; }
        public string explanation { get; set; }
        public string category { get; set; }
        public string time { get; set; }

        public Dictionary<int, string> status_code = new Dictionary<int, string>
        {
            {0 , "success" },
            {1 , "notice" },
            {2 , "alert" },
            {3 , "error" },
        };
    }
}