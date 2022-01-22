using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace typing
{
    internal class keyboard
    {
        public Dictionary<int, string> keyname()
        {
            return new Dictionary<int, string>()
            {
                {18,"kspace_b"},
                {116,"klshift_b"},
                {117,"krshift_b"},
                {44,"ka_b"},
            };
        }
        public Dictionary<string, int[][]> cparts()
        {
            return new Dictionary<string, int[][]>()
            {
                {"a",new int[][] { new int[] { 44 } }},
                {"b",new int[][] { new int[] { 45 } }},
                {"c",new int[][] { new int[] { 46 } }},
                {"d",new int[][] { new int[] { 47 } }},
                {"e",new int[][] { new int[] { 48 } }},
                {"f",new int[][] { new int[] { 49 } }},
                {"g",new int[][] { new int[] { 50 } }},
                {"h",new int[][] { new int[] { 51 } }},
                {"i",new int[][] { new int[] { 52 } }},
                {"j",new int[][] { new int[] { 53 } }},
                {"k",new int[][] { new int[] { 54 } }},
                {"l",new int[][] { new int[] { 55 } }},
                {"m",new int[][] { new int[] { 56 } }},
                {"n",new int[][] { new int[] { 57 } }},
                {"o",new int[][] { new int[] { 58 } }},
                {"p",new int[][] { new int[] { 59 } }},
                {"q",new int[][] { new int[] { 60 } }},
                {"r",new int[][] { new int[] { 61 } }},
                {"s",new int[][] { new int[] { 62 } }},
                {"t",new int[][] { new int[] { 63 } }},
                {"u",new int[][] { new int[] { 64 } }},
                {"v",new int[][] { new int[] { 65 } }},
                {"w",new int[][] { new int[] { 66 } }},
                {"x",new int[][] { new int[] { 67 } }},
                {"y",new int[][] { new int[] { 68 } }},
                {"z",new int[][] { new int[] { 69 } }},


                {"しゃ",new int[][] { new int[] { 62,68,44 },new int[] { 62,51,52,55,54 },new int[] { 62,52,55,44 },new int[] { 62,68,44 }}},
                {"し",new int[][] { new int[] { 62,52},new int[] { 62,51,52} }},
                {"ゃ",new int[][] { new int[] { 55,54 } }},
                {"ん",new int[][] { new int[] { 57,57 },new int[] { 67,57 }  }},
            };
        }
    }
}
