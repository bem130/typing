using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace typing
{
    internal class keyboard
    {

        public string keycodes_to_string(int[] keycodes)
        {
            Dictionary<int, string> dic = keyname();
            string rtt = "";
            foreach (int keycode in keycodes)
            {
                if (dic.ContainsKey(Math.Abs(keycode))) {
                    if (keycode > 0)
                    {
                        rtt += dic[keycode];
                    }
                    else
                    {
                        rtt += dic[keycode].ToUpper();
                    }
                }
            }
            return rtt;
        }

        public Dictionary<int, string> keyname()
        {
            return new Dictionary<int, string>()
            {
                {44,"a"},
                {45,"b"},
                {46,"c"},
                {47,"d"},
                {48,"e"},
                {49,"f"},
                {50,"g"},
                {51,"h"},
                {52,"i"},
                {53,"j"},
                {54,"k"},
                {55,"l"},
                {56,"m"},
                {57,"n"},
                {58,"o"},
                {59,"p"},
                {60,"q"},
                {61,"r"},
                {62,"s"},
                {63,"t"},
                {64,"u"},
                {65,"v"},
                {66,"w"},
                {67,"x"},
                {68,"y"},
                {69,"z"},

                {18,"kspace"},
                {116,"klshift"},
                {117,"krshift"},

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

                {"A",new int[][] { new int[] { -44 } }},
                {"B",new int[][] { new int[] { -45 } }},
                {"C",new int[][] { new int[] { -46 } }},
                {"D",new int[][] { new int[] { -47 } }},
                {"E",new int[][] { new int[] { -48 } }},
                {"F",new int[][] { new int[] { -49 } }},
                {"G",new int[][] { new int[] { -50 } }},
                {"H",new int[][] { new int[] { -51 } }},
                {"I",new int[][] { new int[] { -52 } }},
                {"J",new int[][] { new int[] { -53 } }},
                {"K",new int[][] { new int[] { -54 } }},
                {"L",new int[][] { new int[] { -55 } }},
                {"M",new int[][] { new int[] { -56 } }},
                {"N",new int[][] { new int[] { -57 } }},
                {"O",new int[][] { new int[] { -58 } }},
                {"P",new int[][] { new int[] { -59 } }},
                {"Q",new int[][] { new int[] { -60 } }},
                {"R",new int[][] { new int[] { -61 } }},
                {"S",new int[][] { new int[] { -62 } }},
                {"T",new int[][] { new int[] { -63 } }},
                {"U",new int[][] { new int[] { -64 } }},
                {"V",new int[][] { new int[] { -65 } }},
                {"W",new int[][] { new int[] { -66 } }},
                {"X",new int[][] { new int[] { -67 } }},
                {"Y",new int[][] { new int[] { -68 } }},
                {"Z",new int[][] { new int[] { -69 } }},


                {"あ",new int[][] { new int[] { 44 } }},
                {"い",new int[][] { new int[] { 52 } }},
                {"う",new int[][] { new int[] { 64 } }},
                {"え",new int[][] { new int[] { 48 } }},
                {"お",new int[][] { new int[] { 58 } }},

                {"か",new int[][] { new int[] { 54,44 } }},
                {"き",new int[][] { new int[] { 54,52 } }},
                {"く",new int[][] { new int[] { 54,64 } }},
                {"け",new int[][] { new int[] { 54,48 } }},
                {"こ",new int[][] { new int[] { 54,58 } }},

                {"さ",new int[][] { new int[] { 62,44 } }},
                {"し",new int[][] { new int[] { 62,52 }, new int[] { 62,51,52 } }},
                {"す",new int[][] { new int[] { 62,64 } }},
                {"せ",new int[][] { new int[] { 62,48 } }},
                {"そ",new int[][] { new int[] { 62,58 } }},

                {"た",new int[][] { new int[] { 63,44 } }},
                {"ち",new int[][] { new int[] { 63,52 } }},
                {"つ",new int[][] { new int[] { 63,64 } }},
                {"て",new int[][] { new int[] { 63,48 } }},
                {"と",new int[][] { new int[] { 63,58 } }},

                {"な",new int[][] { new int[] { 57,44 } }},
                {"に",new int[][] { new int[] { 57,52 } }},
                {"ぬ",new int[][] { new int[] { 57,64 } }},
                {"ね",new int[][] { new int[] { 57,48 } }},
                {"の",new int[][] { new int[] { 57,58 } }},

                {"は",new int[][] { new int[] { 51,44 } }},
                {"ひ",new int[][] { new int[] { 51,52 } }},
                {"ふ",new int[][] { new int[] { 51,64 } }},
                {"へ",new int[][] { new int[] { 51,48 } }},
                {"ほ",new int[][] { new int[] { 51,58 } }},

                {"ま",new int[][] { new int[] { 56,44 } }},
                {"み",new int[][] { new int[] { 56,52 } }},
                {"む",new int[][] { new int[] { 56,64 } }},
                {"め",new int[][] { new int[] { 56,48 } }},
                {"も",new int[][] { new int[] { 56,58 } }},

                {"や",new int[][] { new int[] { 68,44 } }},
                {"ゐ",new int[][] { new int[] { 66,68, 52 } }},
                {"ゆ",new int[][] { new int[] { 68,64 } }},
                {"ゑ",new int[][] { new int[] { 66,68, 48 } }},
                {"よ",new int[][] { new int[] { 68,58 } }},

                {"ら",new int[][] { new int[] { 61,44 } }},
                {"り",new int[][] { new int[] { 61,52 } }},
                {"る",new int[][] { new int[] { 61,64 } }},
                {"れ",new int[][] { new int[] { 61,48 } }},
                {"ろ",new int[][] { new int[] { 61,58 } }},

                {"わ",new int[][] { new int[] { 66,44 } }},
                {"を",new int[][] { new int[] { 66,58 } }},
                {"ん",new int[][] { new int[] { 57,57 },new int[] { 67,57 } }},
            };
        }
    }
}
