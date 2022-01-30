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
using System.Resources;

namespace typing
{
    /// <summary>
    /// PlayPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayPage : Page
    {
        keyboard keyb;
        DataTable QAd;

        bool nowplay;

        string type;

        Dictionary<int, string> keylist;

        int allcnt;
        int nowcnt;
        int[] inputpart;
        int[] input_a;
        int partcnt;
        int ipartcnt;
        int typecnt;
        double alltime;
        string[] ncparts;
        DataRow nowq;
        int iqacnt;
        int miscnt;
        Dictionary<string, int[][]> ckeys;
        string[] ckeyskeys;

        List<string> ranss;
        System.Diagnostics.Stopwatch sw;

        public PlayPage()
        {
            keyb = new keyboard();

            InitializeComponent();


            setcolortheme();
            keyb.setcparts();
            ckeys = keyb.ckeys;
            ckeyskeys = keyb.ckeyskeys;
            keylist = keyb.keyname();
            read_file();
            start();
            keyb._cparts();


            var window = (MainWindow)Application.Current.MainWindow;

        }
        public string sdic_to_string(Dictionary<string,string> dic)
        {
            string rt = "";
            string[] dickeys = new string[dic.Count];
            dic.Keys.CopyTo(dickeys, 0);
            foreach (string key in dickeys)
            {
                rt += key + ":" + dic[key] + ";";
            }
            Debug.Print(rt);
            return rt;
        }
        async void finished()
        {
            alltime = sw.Elapsed.TotalSeconds;
            nowplay = false;
            Dictionary<string, string> senddata = new Dictionary<string, string>()
            {
                {"allcnt",allcnt.ToString()},
                {"typecnt",typecnt.ToString()},
                {"miscnt",miscnt.ToString()},
                {"time",alltime.ToString()},
            };
            Application.Current.Properties["Result"] = sdic_to_string(senddata);
            var tpage = new ResultPage();
            NavigationService.Navigate(tpage);
        }
        /// <summary>
        /// カラーテーマの設定
        /// </summary>
        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }
        /// <summary>
        /// 問題ファイルの読み込み
        /// </summary>
        private void read_file()
        {
            ranss = new List<string>()
            {
                ""
            };
            string[] filepaths = ((string)Application.Current.Properties["FilePaths"]).Split('|');

            QAd = new DataTable("QAd");
            StreamReader reader;

            QAd.Columns.Add("id");
            QAd.Columns.Add("question");
            QAd.Columns.Add("answer");
            QAd.Columns.Add("w_answer");
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
                        if (fileline.Length > 0)
                        {
                            if (fprop["type"] == "ja_word")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[0], qaline[1], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
                            if (fprop["type"] == "ja_sentence")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[0], qaline[1], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
                            if (fprop["type"] == "ja-en_word")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
                            if (fprop["type"] == "ja-en_sentence")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
                            if (fprop["type"] == "en-en_word")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
                            if (fprop["type"] == "en-en_sentence")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
                            if (fprop["type"] == "dec-dec_math-addition")
                            {
                                string[] qaline = fileline.Split(fprop["split"][0]);
                                questionid++;
                                QAd.Rows.Add(questionid, qaline[0]+"+"+qaline[1] , (double.Parse(qaline[0])+double.Parse(qaline[1])).ToString(), "", fprop["title"], filePath, line.ToString(), fprop["type"]);
                            }
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
                ((Border)FindName(keyb.keyname()[116])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
            }
            //((Border)FindName(keyb.keyname()[keyname_])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
        }
        private void keybr(int keyname_)
        {
            if (keyname_ < 0)
            {
                ((Border)FindName(keyb.keyname()[116])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
            }
            //((Border)FindName(keyb.keyname()[keyname_])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
        }
        async void missback()
        {
            KeyboardUI.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFD44444");
            await Task.Delay(50);
            KeyboardUI.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB0ABA4");
        }
        async void viewsw() // タイマーの表示
        {
            await Task.Delay(100);
            while (nowplay)
            {
                await Task.Delay(100);
                Qstopwatch.Content = sw.Elapsed.ToString();
                Qtypespeed.Text = cutnumber(typecnt/sw.Elapsed.TotalSeconds,100).ToString();
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e) // キーボード入力受付
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
            Latestkey.Text = Keycode.ToString();
            im(Keycode);
        }


        private void Keyboardkey_Click(object sender, RoutedEventArgs e)
        {
        }

        public void start()
        {
            allcnt = QAd.Rows.Count;
            nowcnt = 0;
            miscnt = 0;
            typecnt = 0;
            QAallcnt.Text = allcnt.ToString();
            QAmiscnt.Text = miscnt.ToString();
            keyc(18);
            kinput.Focus();
        }


        /// <summary>
        /// キーボード入力時の判定
        /// </summary>
        public void im(int keycode)
        {
            string[] nowa;
            if (nowcnt == 0 & keycode == 18)
            {
                nowcnt++;
                partcnt = 0;
                ipartcnt = 0;

                nowplay = true;
                sw = new Stopwatch();
                sw.Start();
                viewsw();

                nowq = QAd.Select("id='"+nowcnt.ToString()+"'")[0];
                ncparts = splita(nowq["answer"].ToString());
                while (ncparts.Length < 1)
                {
                    nowcnt++;
                    if (nowcnt > QAd.Rows.Count)
                    {
                        finished();
                        return;
                    }
                    nowq = QAd.Select("id='"+nowcnt.ToString()+"'")[0];
                    ncparts = splita(nowq["answer"].ToString());
                }
                Qarea.Text = nowq["question"].ToString();
                QAfilename.Text = nowq["filelocation"].ToString();
                QAlinecnt.Text = nowq["fileline"].ToString();
                Qtitle.Text = nowq["title"].ToString();
                QAtype.Text = nowq["type"].ToString();
                QAnowcnt.Text = nowcnt.ToString();



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
                imik = 1;
                foreach (int[] c in ckeys[ncparts[0]])
                {
                    if (c.Length > imik)
                    {
                        imik = c.Length;
                    }
                }
                inputpart = new int[imik];
            }
            else if (nowcnt>0 & keyb.passim(keycode))
            {

                typecnt++;
                QAtypecnt.Text = typecnt.ToString();
                nowa = new string[partcnt];
                Array.Copy(ncparts, 0, nowa, 0, partcnt);
                Aarea.Text = string.Join("", nowa)+ keyb.keycodes_to_string(inputpart);

                inputpart[ipartcnt] = keycode;

                if (ncparts.Length > 0)
                {
                    bool inptt = false;
                    foreach (int[] t in ckeys[ncparts[partcnt]])
                    {
                        if (t.Length > ipartcnt)
                        {
                            int[] spt = new int[ipartcnt+1];
                            int[] spinp = new int[ipartcnt+1];
                            Array.Copy(t, 0, spt, 0, ipartcnt+1);
                            Array.Copy(inputpart, 0, spinp, 0, ipartcnt+1);

                            if (spt.SequenceEqual(spinp))
                            {
                                inptt = true;
                                if (t.Length == ipartcnt+1)
                                {
                                    partcnt++;
                                    if (partcnt == ncparts.Length)
                                    {
                                        break;
                                    }

                                    int imik = 1;
                                    foreach (int[] c in ckeys[ncparts[partcnt]])
                                    {
                                        if (c.Length > imik)
                                        {
                                            imik = c.Length;
                                        }
                                    }
                                    inputpart = new int[imik];
                                    ipartcnt = 0;
                                    break;
                                }
                                ipartcnt++;
                            }
                        }

                    }
                    if (inptt)
                    {
                        PlaySound(Properties.Resources.type);
                    }
                    else
                    {
                        PlaySound(Properties.Resources.mis);
                        miscnt++;
                        QAmiscnt.Text = miscnt.ToString();
                    }
                }
                nowa = new string[partcnt];
                Array.Copy(ncparts, 0, nowa, 0, partcnt);
                Aarea.Text = string.Join("", nowa)+ keyb.keycodes_to_string(inputpart);
                if (partcnt == ncparts.Length)
                {
                    nowcnt++;
                    if (nowcnt > QAd.Rows.Count)
                    {
                        finished();
                        return;
                    }
                    if (nowcnt <= allcnt)
                    {
                        Aarea.Text = "";
                        partcnt = 0;
                        ipartcnt =0;

                        nowq = QAd.Select("id='"+nowcnt.ToString()+"'")[0];
                        Qarea.Text = nowq["question"].ToString();
                        while (ncparts.Length < 1)
                        {
                            nowcnt++;
                            if (nowcnt > QAd.Rows.Count)
                            {
                                finished();
                                return;
                            }
                            nowq = QAd.Select("id='"+nowcnt.ToString()+"'")[0];
                            ncparts = splita(nowq["answer"].ToString());
                        }

                        Debug.Print((ncparts.Length).ToString());
                        ncparts = splita(nowq["answer"].ToString());
                        QAfilename.Text = nowq["filelocation"].ToString();
                        QAlinecnt.Text = nowq["fileline"].ToString();
                        Qtitle.Text = nowq["title"].ToString();
                        QAtype.Text = nowq["type"].ToString();
                        QAnowcnt.Text = nowcnt.ToString();

                        int mik = 1;
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
                    Aprogress.Maximum = allcnt;
                    Aprogress.Value = nowcnt;
                }
            }
        }
        public string[] splita(string str)
        {
            string[] rt = new string[str.Length];
            int lc = 0;
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

        private double cutnumber(double num,int len)
        {
            return ((double)((int)(num*len))/len);
        }

        /// <summary>
        /// 効果音の再生
        /// </summary>
        // 参考 https://qiita.com/Oichan/items/b93e8e8ba8211b925d0a
        // 参考 http://xn--u9j207iixgbigp2p.xn--tckwe/archives/3383

        private void PlaySound(UnmanagedMemoryStream stream)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(stream);
            player.Play();
        }
        private void PlaySound(string path)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);
            player.Play();
        }

    }
}
