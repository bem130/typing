using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Linq;
using System.Data;
using System.Diagnostics;

namespace typing
{
    /// <summary>
    /// PlayPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayPage : Page
    {
        DataTable QAd;

        bool nowplay;

        string type;

        Dictionary<int, string> keylist;

        string bfa;
        string bfkey;
        string nextkey;
        int bflen;
        int allcnt;
        int nowcnt;
        int[] inputpart;
        int[] input_a;
        int partcnt;
        int ipartcnt;
        int typecnt;
        string[] ncparts;
        DataRow nowq;
        int iqacnt;
        int misscnt;
        Dictionary<string, int[][]> ckeys;
        System.Diagnostics.Stopwatch sw;

        public PlayPage()
        {
            InitializeComponent();
            setcolortheme();
            ckeys = cparts();
            keylist = keyname();
            read_file();
            start();


        }
        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }
        private void read_file()
        {
            string[] filepaths = ((string)Application.Current.Properties["FilePaths"]).Split('|');

            QAd = new DataTable("QAd");
            StreamReader reader;


            QAd.Columns.Add("id");
            QAd.Columns.Add("question");
            QAd.Columns.Add("answer");
            QAd.Columns.Add("r_answer");
            QAd.Columns.Add("title");
            QAd.Columns.Add("filelocation");
            QAd.Columns.Add("fileline");
            QAd.Columns.Add("type");
            int questionid = 0;
            foreach (string filePath in filepaths)
            {
                reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
                Dictionary<string, string> fprop = new Dictionary<string, string>()
                {
                    {"split","◊"},
                    {"title","noTitle"},
                    {"type",""},
                };
                Dictionary<string, string> dfprop = new Dictionary<string, string>()
                {
                    {"split","◊"},
                    {"title","noTitle"},
                    {"type",""},
                };
                int line = 0;
                while (reader.Peek() >= 0)
                {
                    string fileline = reader.ReadLine();
                    line++;
                    if (fileline.StartsWith("[set]")) //設定行の場合
                    {
                        string[] fprops = fileline.Substring(5,fileline.Length-5).Split(';');
                        foreach (string prop in fprops)
                        {
                            if (prop.Contains(":"))
                            {
                                string[] spprop = prop.Split(':');
                                if (spprop[1]=="default"+spprop[0])
                                {
                                    if (dfprop.ContainsKey(spprop[0]))
                                    {
                                        fprop[spprop[0]] = dfprop[spprop[0]];
                                    }
                                }
                                else
                                {
                                    fprop[spprop[0]] = spprop[1];
                                }
                            }
                        }
                    }
                    else if (fileline.StartsWith("[comment]")) //コメント行の場合
                    {
                    }
                    else //問題行の場合
                    {
                        if (fprop["type"] == "ja_word")
                        {
                            string[] qaline = fileline.Split(fprop["split"][0]);
                            questionid++;
                            QAd.Rows.Add(questionid, qaline[0], qaline[1], qaline[1], fprop["title"], filePath, line.ToString(), fprop["type"]);
                        }
                        if (fprop["type"] == "ja-en_word")
                        {
                            string[] qaline = fileline.Split(fprop["split"][0]);
                            questionid++;
                            QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                        }
                    }
                }
                reader.Close();
            }
            foreach (DataRow tr in QAd.Rows)
            {
                Debug.Print(string.Join(",", new List<string> { tr["id"].ToString(), tr["question"].ToString(), tr["answer"].ToString(), tr["title"].ToString(), tr["filelocation"].ToString(), tr["fileline"].ToString(), tr["type"].ToString() }));
                ncparts = splita(tr["answer"].ToString());
                Debug.Print("  "+string.Join(",", ncparts));
            }
        }
        private void keyarea_load(object sender, RoutedEventArgs e)
        {
            kinput.Focus();
        }
        private void keyc(int keyname_)
        {
            if (keyname_ < 0)
            {
                ((Border)FindName(keyname()[116])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
            }
            ((Border)FindName(keyname()[keyname_])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
        }
        private void keyb(int keyname_)
        {
            if (keyname_ < 0)
            {
                ((Border)FindName(keyname()[116])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
            }
            ((Border)FindName(keyname()[keyname_])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
        }
        async void missback()
        {
            KeyboardUI.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFD44444");
            await Task.Delay(50);
            KeyboardUI.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB0ABA4");
        }
        async void viewsw()
        {
            await Task.Delay(1000);
            while (nowplay)
            {
                await Task.Delay(100);
                Qstopwatch.Text = sw.Elapsed.ToString();
                Qtypespeed.Text = (typecnt/sw.Elapsed.TotalSeconds).ToString();
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            Key systemKey = e.SystemKey;
            KeyStates keyStates = e.KeyStates;
            bool isRepeat = e.IsRepeat;
            ModifierKeys modifierKeys = Keyboard.Modifiers;
            string Keyname = key.ToString();
            int Keycode = ((int)key);
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None)
            {
                Keyname += "_S";
                Keycode *= -1;
            }
            Debug.Print(Keyname+" "+Keycode.ToString());
            im(Keycode);
        }


        private void Keyboardkey_Click(object sender, RoutedEventArgs e)
        {
        }

        public void start()
        {
            allcnt = QAd.Rows.Count;
            nowcnt = 0;
            misscnt = 0;
            typecnt = 0;
            QAallcnt.Text = allcnt.ToString();
            keyc(18);
        }
        public void im(int keycode)
        {
            Debug.Print("nowcnt:" + nowcnt.ToString() + "partcnt:" + partcnt.ToString() + "ipartcnt:" + ipartcnt.ToString());
            if (nowcnt == 0 & keycode == 18)
            {
                nowcnt++;
                partcnt = 0;
                ipartcnt = 0;

                nowq = QAd.Select("id='"+nowcnt.ToString()+"'")[0];
                Debug.Print(string.Join(",", new List<string> { nowq["id"].ToString(), nowq["question"].ToString(), nowq["answer"].ToString(), nowq["title"].ToString(), nowq["filelocation"].ToString(), nowq["fileline"].ToString() }));
                ncparts = splita(nowq["answer"].ToString());
                Debug.Print(string.Join(",",ncparts));
                Qarea.Text = nowq["question"].ToString();
                QAfilename.Text = nowq["filelocation"].ToString();
                QAlinecnt.Text = nowq["fileline"].ToString();
                int mik = 0;
                int imik;
                foreach (string ch in ncparts)
                {
                    int[][] tchk = ckeys[ch];
                    imik = 0;
                    foreach (int[] c in tchk)
                    {
                        if (c.Length > imik)
                        {
                            imik = c.Length;
                        }
                    }
                    mik += imik;
                }
                input_a = new int[mik];
                imik = 0;
                foreach (int[] c in ckeys[ncparts[0]])
                {
                    if (c.Length > imik)
                    {
                        imik = c.Length;
                    }
                }
                inputpart = new int[imik];
            }
            else if (nowcnt>0)
            {

                typecnt++;

                inputpart[ipartcnt] = keycode;
                bool ipok = false;
                //for (int ck = 0; ck<ckeys[ncparts[partcnt]].Length; ck++)
                //{
                //    int[] t = ckeys[ncparts[partcnt]][ck];
                //    Debug.Print(string.Join(",", t)+" "+string.Join(",", inputpart));
                //    bool iipok = true;
                //    for (int i = 0; i<t.Length; i++)
                //    {
                //        if (inputpart[i] != t[0])
                //        {
                //            iipok = false;
                //            break;
                //        }
                //    }
                //    if (iipok)
                //    {
                //        ipok = true;
                //        if (ipartcnt+1 == t.Length)
                //        {
                //            ipok = false;
                //            partcnt++;
                //            ipartcnt = 0;
                //            int imik = 0;
                //            foreach (int[] c in ckeys[ncparts[0]])
                //            {
                //                if (c.Length > imik)
                //                {
                //                    imik = c.Length;
                //                }
                //            }
                //            inputpart = new int[imik];
                //        }
                //        break;
                //    }
                //}
                //if (ipok)
                //{
                //    ipartcnt++;
                //}
                foreach (int[] t in ckeys[ncparts[partcnt]])
                {
                    if (t.Length > ipartcnt)
                    {
                        int[] spt = new int[ipartcnt+1];
                        int[] spinp = new int[ipartcnt+1];
                        Array.Copy(t, 0, spt, 0, ipartcnt+1);
                        Array.Copy(inputpart, 0, spinp, 0, ipartcnt+1);
                        Debug.Print(" "+string.Join(",", spt)+";;"+string.Join(",", spinp));
                        if (spt.SequenceEqual(spinp))
                        {
                            if (t.Length == ipartcnt+1)
                            {
                                partcnt++;
                                ipartcnt = 0;
                                int imik = 0;
                                foreach (int[] c in ckeys[ncparts[0]])
                                {
                                    if (c.Length > imik)
                                    {
                                        imik = c.Length;
                                    }
                                }
                                inputpart = new int[imik];
                                break;
                            }
                            ipartcnt++;
                        }
                    }

                }
                Debug.Print("nowcnt:" + nowcnt.ToString() + "partcnt:" + partcnt.ToString() + "ipartcnt:" + ipartcnt.ToString() + "allparts:" + (ncparts.Length).ToString());
                if (partcnt == ncparts.Length)
                {
                    nowcnt++;
                    if (nowcnt <= allcnt)
                    {
                        partcnt = 0;
                        ipartcnt =0;

                        nowq = QAd.Select("id='"+nowcnt.ToString()+"'")[0];
                        Debug.Print(string.Join(",", new List<string> { nowq["id"].ToString(), nowq["question"].ToString(), nowq["answer"].ToString(), nowq["title"].ToString(), nowq["filelocation"].ToString(), nowq["fileline"].ToString() }));
                        ncparts = splita(nowq["answer"].ToString());
                        Debug.Print(string.Join(",", ncparts));
                        Qarea.Text = nowq["question"].ToString();
                        int mik = 0;
                        foreach (string ch in ncparts)
                        {
                            int[][] tchk = ckeys[ch];
                            int imik = 0;
                            foreach (int[] c in tchk)
                            {
                                if (c.Length > imik)
                                {
                                    imik = c.Length;
                                }
                            }
                            mik += imik;
                        }
                        inputpart = new int[mik];
                    }
                    Qprogress.Maximum = allcnt;
                    Qprogress.Value = nowcnt;
                    Aprogress.Maximum = allcnt;
                    Aprogress.Value = nowcnt;
                }
            }
        }
        public string[] splita(string str)
        {
            string[] rt = new string[str.Length];
            int lc = 0;
            string[] ckeyskeys = new string[ckeys.Count];
            ckeys.Keys.CopyTo(ckeyskeys, 0);
            while (str.Length > 0)
            {
                for (int i=0;i<ckeys.Count;i++)
                {
                    if (str.StartsWith(ckeyskeys[i]))
                    {
                        rt[lc] = ckeyskeys[i];
                        lc++;
                        str = str.Substring(ckeyskeys[i].Length);
                        break;
                    }
                    else if (i==ckeys.Count-1)
                    {
                        str = str.Substring(1);
                    }
                }
            }
            Array.Resize(ref rt, lc);
            return rt;
        }
        public Dictionary<int,string> keyname()
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
