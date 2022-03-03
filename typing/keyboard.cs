using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;

namespace typing
{
    internal class keyboard
    {
        MainWindow mainwindow;


        public keyboard()
        {
            mainwindow = (MainWindow)Application.Current.MainWindow;
            mainwindow.setText(0, "Read keyboard");
        }

        public Dictionary<string, int[][]> ckeys;
        public string[] ckeyskeys;
        public string keycodes_to_string(int[] keycodes)
        {
            Dictionary<int, string> dic = keyname();
            string rtt = "";
            foreach (int keycode in keycodes)
            {
                if (dic.ContainsKey(Math.Abs(keycode)))
                {
                    if (keycode > 0)
                    {
                        rtt += dic[keycode];
                    }
                    else
                    {
                        rtt += dic[-keycode].ToUpper();
                    }
                }
            }
            return rtt;
        }
        public string[] s_sort(string[] strl)
        {
            string[] rtl = new string[strl.Length];
            int li = 0;
            foreach (string tm in strl)
            {
                if (tm.Length > li)
                {
                    li = tm.Length;
                }
            }
            //Debug.Print(li.ToString());
            uint nic = 0;
            while (li >= 0)
            {
                foreach (string tm in strl)
                {
                    //Debug.Print(tm);
                    if (tm.Length == li)
                    {
                        rtl[nic] = tm;
                        nic++;
                    }
                }
                li--;
            }
            return rtl;
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

            };
        }
        public bool passim(int keycode)
        {
            List<int> rt = new List<int>{};

            string[] ckeyskeys = new string[ckeys.Count];
            ckeys.Keys.CopyTo(ckeyskeys, 0);

            for (int i = 0; i<ckeys.Count; i++)
            {
                int[][] ch = ckeys[ckeyskeys[i]];
                foreach (int[] ck in ch)
                {
                    foreach (int cka in ck)
                    {
                        if (rt.Contains(Math.Abs(cka)) == false)
                        {
                            rt.Add(Math.Abs(cka));
                        }
                    }
                }
            }


            return rt.Contains(Math.Abs(keycode));
        }
        public void setcparts()
        {
            ckeys = cparts();

            ckeyskeys = new string[ckeys.Count];
            ckeys.Keys.CopyTo(ckeyskeys, 0);
            ckeyskeys = s_sort(ckeyskeys);
        }

        public Dictionary<string,int[][]> cparts()
        {
            Dictionary<string, int[][]> rtd = new Dictionary<string, int[][]>();

            string[] filepaths = (Properties.Settings.Default.keyboard_dir).Split('|');

            if (filepaths[0] == "")
            {
                return _cparts();
            }

            StreamReader reader = null;
            foreach (string filePath in filepaths)
            {
                try
                {
                    reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
                }
                catch (Exception ex)
                {
                    mainwindow.setText(1, "Failed reading keyboard file : "+filePath);
                }
                if (reader != null)
                {
                    while (reader.Peek() >= 0)
                    {
                        string fileline = reader.ReadLine();


                        if (fileline=="") //空白行の場合
                        {
                        }
                        else if (fileline.StartsWith("[comment]")) //コメント行の場合
                        {
                        }
                        else if (fileline.StartsWith("[comments]")) //複数コメント行の場合
                        {
                            do
                            {
                                fileline = reader.ReadLine();
                            }
                            while (fileline.StartsWith("[/comments]") == false & reader.Peek() >= 0);
                        }
                        else //通常行の場合
                        {
                            //Debug.Print(fileline);
                            string[] c_p = fileline.Split(':');
                            string chars = c_p[0];
                            //Debug.Print(String.Join(",", c_p));
                            string[] props = c_p[1].Split(';');
                            //Debug.Print(String.Join(" ; ", props));

                            int[][] il;
                            int ilc = 0;
                            if (rtd.ContainsKey(c_p[0]))
                            {
                                il = new int[props.Length][];
                            }
                            else
                            {
                                il = new int[props.Length][];
                            }
                            foreach (string prop in props)
                            {
                                string[] spp = prop.Split(',');

                                int[] i = new int[spp.Length];
                                int ic = 0;
                                foreach (string s in spp)
                                {
                                    i[ic] = int.Parse(s);
                                    ic++;
                                }
                                il[ilc] = i;
                                ilc++;
                            }
                            rtd[c_p[0]] = il;
                        }
                    }
                    reader.Close();
                    mainwindow.setText(0, "Read keyboard file : "+filePath);
                }
            }
            return rtd;
        }

        public Dictionary<string, int[][]> _cparts()
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


                {",",new int[][] { new int[] { 142 } }},
                {".",new int[][] { new int[] { 144 } }},

                {"0",new int[][] { new int[] { 34 } }},
                {"1",new int[][] { new int[] { 35 } }},
                {"2",new int[][] { new int[] { 36 } }},
                {"3",new int[][] { new int[] { 37 } }},
                {"4",new int[][] { new int[] { 38 } }},
                {"5",new int[][] { new int[] { 39 } }},
                {"6",new int[][] { new int[] { 40 } }},
                {"7",new int[][] { new int[] { 41 } }},
                {"8",new int[][] { new int[] { 42 } }},
                {"9",new int[][] { new int[] { 43 } }},
            };
        }
    }
}
