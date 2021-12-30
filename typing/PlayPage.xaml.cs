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
using System.Linq;
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
        int iqacnt;
        int misscnt;
        int typecnt;
        System.Diagnostics.Stopwatch sw;

        public PlayPage()
        {
            InitializeComponent();
            setcolortheme();
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


            QAd.Columns.Add("id");
            QAd.Columns.Add("question");
            QAd.Columns.Add("answer");
            QAd.Columns.Add("title");
            QAd.Columns.Add("filelocation");
            QAd.Columns.Add("fileline");
            QAd.Columns.Add("type");
            int questionid = 0;
            foreach (string filePath in filepaths)
            {
                StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
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
                }; ;


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
                    else //問題行の場合
                    {
                        if (fprop["type"] == "ja-en")
                        {
                            string[] qaline = fileline.Split(fprop["split"][0]);
                            questionid++;
                            QAd.Rows.Add(questionid, qaline[0], qaline[1], fprop["title"], filePath, line.ToString(), fprop["type"]);
                        }
                    }
                }

                reader.Close();


                foreach (DataRow tr in QAd.Rows)
                {
                    Debug.Print(string.Join(",",new List<string>{ tr["id"].ToString(), tr["question"].ToString(), tr["answer"].ToString() , tr["title"].ToString() , tr["filelocation"].ToString() , tr["fileline"].ToString() }));
                }

                //QAorder = QAd.Keys.ToList();
                //Debug.Print(string.Join(" , ", QAorder));
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
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None) Keyname += "_S";
            int Keycode = ((int)key);
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
            Debug.Print("nowcnt:" + nowcnt.ToString());
            if (nowcnt == 0 & keycode == 18)
            {
                nowcnt++;
            }
            else if (nowcnt>0)
            {
                typecnt++;

                //string nowQuesv = QAorder[nowcnt];
                //List<string> nowQues = QAd[QAorder[nowcnt]];

                //Debug.Print(nowQuesv+ " " + string.Join(" , ", nowQues));
            }
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
    }
}
