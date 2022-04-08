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
        MainWindow mainwindow;

        keyboard keyb;
        DataTable QAd;

        int bftkey;
        int keyf;

        bool nowplay;
        bool nowpause;

        Dictionary<int, string> keylist;

        int allcnt;
        int nowcnt;
        int[] inputpart;
        int[] input_a;
        int partcnt;
        int ipartcnt;
        int typecnt;
        string filenames;
        double alltime;
        string[] ncparts;
        DataRow nowq;
        int miscnt;
        List<int> correctans;
        Dictionary<string, int[][]> ckeys;
        string[] ckeyskeys;
        int smax;

        List<string> ranss;
        System.Diagnostics.Stopwatch sw;

        public PlayPage()
        {
            mainwindow = (MainWindow)Application.Current.MainWindow;
            mainwindow.setText(0,"Open PlayPage");

            InitializeComponent();

            nowpause = false;

            keyb = new keyboard();

            settheme();
            keyb.setcparts();
            ckeys = keyb.ckeys;
            ckeyskeys = keyb.ckeyskeys;
            keylist = keyb.keyname();
            smax = keyb.smax;
            bftkey = 0;
            keyf = 0;
            read_file();
            start();


            var window = (MainWindow)Application.Current.MainWindow;

        }
        string Int_to_String(int number,int n = 10)
        {
            string r = "";
            if (number < 0)
            {
                r = "-";
            }
            r += Convert.ToString(Math.Abs(number), n);
            return r;
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
            return rt;
        }
        void finished()
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
        public void settheme()
        {
            try
            {
                string dicPath = Properties.Settings.Default.colortheme;
                ResourceDictionary dic = new ResourceDictionary();
                dic.Source = new Uri(dicPath, UriKind.Relative);
                this.Resources.MergedDictionaries.Add(dic);

                dicPath = Properties.Settings.Default.langtheme;
                dic = new ResourceDictionary();
                dic.Source = new Uri(dicPath, UriKind.Relative);
                this.Resources.MergedDictionaries.Add(dic);
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
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
            filenames = "";

            QAd.Columns.Add("id");
            QAd.Columns.Add("question");
            QAd.Columns.Add("answer");
            QAd.Columns.Add("w_answer");
            QAd.Columns.Add("title");
            QAd.Columns.Add("filelocation");
            QAd.Columns.Add("fileline");
            QAd.Columns.Add("type");
            QAd.Columns.Add("backimage");
            QAd.Columns.Add("information");
            int questionid = 0;
            foreach (string filePath in filepaths)
            {
                reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
                Dictionary<string, string> fprop = new Dictionary<string, string>()
                {
                    {"name",""},
                    {"split",","},
                    {"title","noTitle"},
                    {"information",""},
                    {"type",""},
                    {"backimage",""},
                };
                Dictionary<string, string> dfprop = new Dictionary<string, string>()
                {
                    {"name",""},
                    {"split",","},
                    {"title","noTitle"},
                    {"information",""},
                    {"type",""},
                    {"backimage",""},
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
                                    fprop[spprop[0]] = prop.Substring(spprop[0].Length+1,prop.Length-spprop[0].Length-1);
                                    Debug.Print("filesetting "+spprop[0]+": "+fprop[spprop[0]]);
                                }
                            }
                        }
                    }
                    else if (fileline=="") //空白行の場合
                    {
                    }
                    else if (fileline.StartsWith("[block]")) //ブロック行の場合
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
                            line++;
                        }
                        while (fileline.StartsWith("[/comments]") == false & reader.Peek() >= 0);
                    }
                    else //問題行の場合
                    {
                        if (fileline.Length > 0)
                        {
                            {
                                if (fprop["type"] == "ja_word")
                                {
                                    string[] qaline = fileline.Split(fprop["split"][0]);
                                    questionid++;
                                    QAd.Rows.Add(questionid, qaline[0], qaline[1], "", fprop["title"], filePath, line.ToString(), fprop["type"],fprop["backimage"],fprop["information"]);
                                }
                                if (fprop["type"] == "ja_sentence")
                                {
                                    string[] qaline = fileline.Split(fprop["split"][0]);
                                    questionid++;
                                    QAd.Rows.Add(questionid, qaline[0], qaline[1], "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                }
                                if (fprop["type"] == "en_word")
                                {
                                    string[] qaline = fileline.Split(fprop["split"][0]);
                                    questionid++;
                                    QAd.Rows.Add(questionid, qaline[0], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                }
                                if (fprop["type"] == "en_sentence")
                                {
                                    string[] qaline = fileline.Split(fprop["split"][0]);
                                    questionid++;
                                    QAd.Rows.Add(questionid, qaline[0], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                }
                            }
                            {
                                if (fprop["type"] == "question_word")
                                {
                                    string[] qaline = fileline.Split(fprop["split"][0]);
                                    questionid++;
                                    QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                }
                                if (fprop["type"] == "question_sentence")
                                {
                                    string[] qaline = fileline.Split(fprop["split"][0]);
                                    questionid++;
                                    QAd.Rows.Add(questionid, qaline[1], qaline[0], "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                }
                            }
                            {
                                {
                                    if (fprop["type"] == "dec-dec_math-addition")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        questionid++;
                                        QAd.Rows.Add(questionid, qaline[0]+" + "+qaline[1], (double.Parse(qaline[0])+double.Parse(qaline[1])).ToString(), "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                    }
                                    if (fprop["type"] == "dec-dec_math-subtraction")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        questionid++;
                                        QAd.Rows.Add(questionid, qaline[0]+" - "+qaline[1], (double.Parse(qaline[0])-double.Parse(qaline[1])).ToString(), "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                    }
                                    if (fprop["type"] == "dec-dec_math-multiplication")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        questionid++;
                                        QAd.Rows.Add(questionid, qaline[0]+" × "+qaline[1], (double.Parse(qaline[0])*double.Parse(qaline[1])).ToString(), "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                    }
                                    if (fprop["type"] == "dec-dec_math-division")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        questionid++;
                                        QAd.Rows.Add(questionid, (double.Parse(qaline[0])*double.Parse(qaline[1])).ToString()+" ÷ "+qaline[1], qaline[0].ToString(), "", fprop["title"], filePath, line.ToString(), fprop["type"], fprop["backimage"], fprop["information"]);
                                    }
                                }
                                {
                                    string npt1 = "dec";
                                    string npt2 = "dec";
                                    int pn1 = 10;
                                    int pn2 = 10;
                                    if (fprop["type"].Substring(0,3) == "hex")
                                    {
                                        npt1 = "hex";
                                        pn1 = 16;
                                    }
                                    if (fprop["type"].Substring(4,3) == "hex")
                                    {
                                        npt2 = "hex";
                                        pn2 = 16;
                                    }
                                    if (fprop["type"].Substring(0, 3) == "bin")
                                    {
                                        npt1 = "bin";
                                        pn1 = 2;
                                    }
                                    if (fprop["type"].Substring(4, 3) == "bin")
                                    {
                                        npt2 = "bin";
                                        pn2 = 2;
                                    }
                                    string npt = npt1 + "-" + npt2;
                                    if (fprop["type"].Substring(7) == "_math-addition_rm")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        int num = int.Parse(qaline[4]);
                                        int min1 = int.Parse(qaline[0]);
                                        int min2 = int.Parse(qaline[2]);
                                        int max1 = int.Parse(qaline[1]);
                                        int max2 = int.Parse(qaline[3]);
                                        for (int c = 0; c<num; c++)
                                        {
                                            Random r = new Random(Environment.TickCount+questionid*5+line*5);
                                            int num1 = r.Next(min1, max1);
                                            int num2 = r.Next(min2, max2);
                                            questionid++;
                                            QAd.Rows.Add(questionid, Convert.ToString(num1, pn1)+" + "+Convert.ToString(num2, pn1), Convert.ToString((num1 + num2),pn2), "", fprop["title"], filePath, line.ToString(), npt+"_math-addition_rm", fprop["backimage"], fprop["information"]);
                                        }
                                    }
                                    if (fprop["type"].Substring(7) == "_math-subtraction_rm")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        int num = int.Parse(qaline[4]);
                                        int min1 = int.Parse(qaline[0]);
                                        int min2 = int.Parse(qaline[2]);
                                        int max1 = int.Parse(qaline[1]);
                                        int max2 = int.Parse(qaline[3]);
                                        for (int c = 0; c<num; c++)
                                        {
                                            Random r = new Random(Environment.TickCount+questionid*5+line*5);
                                            int num1 = r.Next(min1, max1);
                                            int num2 = r.Next(min2, max2);
                                            questionid++;
                                            QAd.Rows.Add(questionid, Convert.ToString(num1, pn1)+" - "+Convert.ToString(num2, pn1), Int_to_String((num1 - num2), pn2), "", fprop["title"], filePath, line.ToString(), npt+"_math-addition_rm", fprop["backimage"], fprop["information"]);
                                        }
                                    }
                                    if (fprop["type"].Substring(7) == "_math-multiplication_rm")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        int num = int.Parse(qaline[4]);
                                        int min1 = int.Parse(qaline[0]);
                                        int min2 = int.Parse(qaline[2]);
                                        int max1 = int.Parse(qaline[1]);
                                        int max2 = int.Parse(qaline[3]);
                                        for (int c = 0; c<num; c++)
                                        {
                                            Random r = new Random(Environment.TickCount+questionid*5+line*5);
                                            int num1 = r.Next(min1, max1);
                                            int num2 = r.Next(min2, max2);
                                            questionid++;
                                            QAd.Rows.Add(questionid, Convert.ToString(num1, pn1)+" × "+Convert.ToString(num2, pn1), Convert.ToString((num1 * num2), pn2), "", fprop["title"], filePath, line.ToString(), npt+"_math-multiplicationvvv_rm", fprop["backimage"], fprop["information"]);
                                        }
                                    }
                                    if (fprop["type"].Substring(7) == "_math-division_rm")
                                    {
                                        string[] qaline = fileline.Split(fprop["split"][0]);
                                        int num = int.Parse(qaline[4]);
                                        int min1 = int.Parse(qaline[0]);
                                        int min2 = int.Parse(qaline[2]);
                                        int max1 = int.Parse(qaline[1]);
                                        int max2 = int.Parse(qaline[3]);
                                        for (int c = 0; c<num; c++)
                                        {
                                            Random r = new Random(Environment.TickCount+questionid*5+line*5);
                                            int num1 = r.Next(min1, max1);
                                            int num2 = r.Next(min2, max2);
                                            if (num2 != 0)
                                            {
                                                questionid++;
                                                QAd.Rows.Add(questionid, Convert.ToString((num1*num2),pn1)+" ÷ "+Convert.ToString(num2, pn1), Convert.ToString(num1, pn2), "", fprop["title"], filePath, line.ToString(), npt+"_math-division_rm", fprop["backimage"], fprop["information"]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                reader.Close();

                if (fprop["name"]=="")
                {
                    filenames+=fprop["title"]+",";
                }
                else
                {
                    filenames+=fprop["name"]+",";
                }
                Application.Current.Properties["Filenames"] = filenames.Substring(0,filenames.Length-1);
            }
            foreach (DataRow tr in QAd.Rows)
            {
                ncparts = splita(tr["answer"].ToString());
            }
        }
        private void keyarea_load(object sender, RoutedEventArgs e)
        {
            kinput.Focus();
        }
        async private void keyc(int keyname_)
        {
            if (FindName("kb"+Math.Abs(keyname_).ToString()) == null)
            {
                return;
            }
            ((Border)FindName("kb"+Math.Abs(keyname_).ToString())).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
            int nkeyf = keyf;
            await Task.Delay(500);
            if (nkeyf==keyf)
            {
                keybr(bftkey);
            }
        }
        private void keybr(int keyname_)
        {
            if (FindName("kb"+Math.Abs(keyname_).ToString()) == null)
            {
                return;
            }
            ((Border)FindName("kb"+Math.Abs(keyname_).ToString())).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
        }
        async void missback()
        {
            KeyboardUI.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFD44444");
            await Task.Delay(50);
            KeyboardUI.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB0ABA4");
        }
        async void viewsw() // タイマーの表示
        {
            await Task.Delay(10);
            while (nowplay)
            {
                if (!nowpause)
                {
                }
                Qstopwatch.Content = sw.Elapsed.ToString();
                Qtypespeed.Text = cutnumber(typecnt/sw.Elapsed.TotalSeconds,100).ToString();
                await Task.Delay(100);
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e) // キーボード入力受付
        {
            Key key = e.Key;
            Key systemKey = e.SystemKey;
            KeyStates keyStates = e.KeyStates;
            bool isRepeat = e.IsRepeat;
            string Keyname = key.ToString();
            int Keycode = ((int)key);
            ModifierKeys modifierKeys = Keyboard.Modifiers;
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None)
            {
                Keyname += "_S";
                Keycode *= -1;
            }
            Latestkey.Text = Keycode.ToString();

            if (nowpause == false)
            {
                if (Keycode == 13 & nowplay)
                {
                    pause();
                }
                im(Keycode);
            }
            else
            {
                if (Keycode == 18 & nowplay)
                {
                    resume();
                }
            }

        }

        private void Keyboardkey_Click(object sender, RoutedEventArgs e)
        {
        }

        public void start()
        {
            PauseMenu.Visibility = Visibility.Hidden;
            allcnt = QAd.Rows.Count;
            nowcnt = 0;
            miscnt = 0;
            typecnt = 0;
            QAallcnt.Text = allcnt.ToString();
            QAsuccesscnt.Text = (typecnt-miscnt).ToString();
            keyc(18);
            kinput.Focus();
        }

        /// <summary>
        /// キーボード入力時の判定
        /// </summary>
        public void im(int keycode)
        {
            Random r = new Random(Environment.TickCount+keycode);
            keyf = r.Next(0,100)+keycode*100;
            keybr(bftkey);
            bftkey = keycode;

            string[] nowa;
            if (nowcnt == 0 & (keycode == 18 | keycode == 6))
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
                QAfilename.Text = Path.GetFileName(nowq["filelocation"].ToString());
                QAlinecnt.Text = nowq["fileline"].ToString();
                Qtitle.Text = nowq["title"].ToString();
                QAtype.Text = nowq["type"].ToString();
                QAnowcnt.Text = nowcnt.ToString();

                Information.Text = nowq["information"].ToString();


                string bimgpath = nowq["backimage"].ToString();
                string dir = System.IO.Path.GetDirectoryName(nowq["filelocation"].ToString());
                if (bimgpath.StartsWith("this:/")|bimgpath.StartsWith("this:\\"))
                {
                    bimgpath = dir+bimgpath.Substring(5, bimgpath.Length-5);
                }
                if (bimgpath.StartsWith("this:"))
                {
                    bimgpath = dir+"\\"+bimgpath.Substring(5, bimgpath.Length-5);
                }
                Debug.Print(bimgpath);
                try
                {
                    if (bimgpath.ToString().Length>0)
                    {
                        ImageBrush img = new ImageBrush();
                        img.ImageSource = new BitmapImage(new Uri(bimgpath.ToString()));
                        Backimg.Background = img;
                        Backimgcover.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Backimgcover.Visibility = Visibility.Hidden;
                    }
                }
                catch (FileNotFoundException e)
                {
                    Backimgcover.Visibility = Visibility.Hidden;
                    Console.WriteLine("NotFound: " + e.FileName);
                }
                catch (Exception e)
                {
                    Backimgcover.Visibility = Visibility.Hidden;
                }


                correctans = new List<int>();
                for (int i=0;i<ncparts.Length;i++)
                {
                    correctans.Add(0);
                }


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

                keyc(keycode);
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
                        correctans[partcnt] = 1;
                        Debug.Print(String.Join(",", correctans)+" "+(ncparts.Length*smax).ToString());
                        inputpart[ipartcnt] = 0;
                        PlaySound(Properties.Resources.mis);
                        miscnt++;
                    }
                }
                QAsuccesscnt.Text = (typecnt-miscnt).ToString();

                nowa = new string[partcnt];
                Array.Copy(ncparts, 0, nowa, 0, partcnt);

                TextBlock a = Aarea;
                Run b;
                Aarea.Text = "";
                int i;
                for (i = 0; i<nowa.Length; i++)
                {
                    b = new Run(ncparts[i]);
                    if (correctans[i]==0)
                    {
                        b.Foreground = (Brush)this.FindResource("cAreaTrue");
                    }
                    else
                    {
                        b.Foreground = (Brush)this.FindResource("cAreaFalse");
                    }
                    a.Inlines.Add(b);
                }
                b = new Run(keyb.keycodes_to_string(inputpart));
                b.Foreground = (Brush)FindResource("cAreaDef");
                a.Inlines.Add(b);


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

                        ncparts = splita(nowq["answer"].ToString());
                        QAfilename.Text = Path.GetFileName(nowq["filelocation"].ToString());
                        QAlinecnt.Text = nowq["fileline"].ToString();
                        Qtitle.Text = nowq["title"].ToString();
                        QAtype.Text = nowq["type"].ToString();
                        QAnowcnt.Text = nowcnt.ToString();
                        Information.Text = nowq["information"].ToString();

                        string bimgpath = nowq["backimage"].ToString();
                        string dir = Path.GetDirectoryName(nowq["filelocation"].ToString());
                        if (bimgpath.StartsWith("this:/")|bimgpath.StartsWith("this:\\"))
                        {
                            bimgpath = dir+bimgpath.Substring(5, bimgpath.Length-5);
                        }
                        if (bimgpath.StartsWith("this:"))
                        {
                            bimgpath = dir+"\\"+bimgpath.Substring(5, bimgpath.Length-5);
                        }
                        Debug.Print(bimgpath);
                        try
                        {
                            if (bimgpath.ToString().Length>0)
                            {
                                ImageBrush img = new ImageBrush();
                                img.ImageSource = new BitmapImage(new Uri(bimgpath.ToString()));
                                Backimg.Background = img;
                                Backimgcover.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                Backimgcover.Visibility = Visibility.Hidden;
                            }
                        }
                        catch (FileNotFoundException e)
                        {
                            Backimgcover.Visibility = Visibility.Hidden;
                            Console.WriteLine("NotFound: " + e.FileName);
                        }
                        catch (Exception e)
                        {
                            Backimgcover.Visibility = Visibility.Hidden;
                        }
                        correctans = new List<int>();
                        for (i = 0; i<ncparts.Length; i++)
                        {
                            correctans.Add(0);
                        }
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

        /// <summary>
        /// 一時停止
        /// </summary>
        private void Pause_button(object sender, RoutedEventArgs e)
        {
            if (nowplay)
            {
                pause();
            }
        }
        private void Resume_button(object sender, RoutedEventArgs e)
        {
            resume();
            kinput.Focus();
        }
        private void pause()
        {
            sw.Stop();
            nowpause = true;
            PauseMenu.Visibility = Visibility.Visible;
        }
        private void resume()
        {
            PauseMenu.Visibility = Visibility.Hidden;
            nowpause = false;
            kinput.Focus();
            sw.Start();
        }
        private void Retire_button(object sender, RoutedEventArgs e)
        {
            finished();
        }
    }
}
