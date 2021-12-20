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

using System.Diagnostics;

namespace typing
{
    /// <summary>
    /// PlayPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayPage : Page
    {
        bool nowplay;

        string type;
        Dictionary<string, List<string>> QAd;

        Dictionary<string, string> keylist;

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

            im();

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

            QAd = new Dictionary<string, List<string>>();

            foreach (string filePath in filepaths)
            {
                StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
                var fprop = new Dictionary<string, string>();
                int line = 1;
                string[] fprops = reader.ReadLine().Split(';');
                foreach (string prop in fprops)
                {
                    if (prop.Contains(":"))
                    {
                        string[] spprop = prop.Split(':');
                        fprop[spprop[0]] = spprop[1];
                    }
                }

                char split_l = '◊';
                if (fprop.ContainsKey("split"))
                {
                    split_l = fprop["split"][0];
                }

                type = "";
                if (fprop.ContainsKey("type"))
                {
                    type = fprop["type"];
                }

                if (type == "ja-en")
                {
                    while (reader.Peek() >= 0)
                    {
                        line++;
                        string[] qaline = reader.ReadLine().Split(split_l);
                        QAd[qaline[0]] = new List<string>() { qaline[1], filePath ,line.ToString(),type};
                    }
                }
                reader.Close();
            }

        }
        private void keyarea_load(object sender, RoutedEventArgs e)
        {
            kinput.Focus();
        }
        private void keyc(string keyname_)
        {
            if (char.IsUpper(keyname_[0]))
            {
                ((Border)FindName(keyname()["shift"])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
            }
            ((Border)FindName(keyname()[keyname_.ToLower()])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
        }
        private void keyb(string keyname_)
        {
            if (char.IsUpper(keyname_[0]))
            {
                ((Border)FindName(keyname()["shift"])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
            }
            ((Border)FindName(keyname()[keyname_.ToLower()])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDCD1D1");
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
        async private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            //Debug.Print("keydown");

            Key key = e.Key;
            Key systemKey = e.SystemKey;
            KeyStates keyStates = e.KeyStates;
            bool isRepeat = e.IsRepeat;
            string s = "";
            s += string.Format("  Key={0}  KeyStates={1}  IsRepeat={2}", key, keyStates, isRepeat);
            ModifierKeys modifierKeys = Keyboard.Modifiers;
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None) s += "_Shift";
            //Debug.Print(s);
            string key_name = key.ToString().ToLower();
            string Keyname = key.ToString();
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None) Keyname += "_S";

            int Keycode = ((int)key);


            Debug.Print(Keyname+" "+Keycode.ToString());

            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None)
            {
                key_name = key_name.ToUpper();
            }
            if (type == "ja-en")
            {
                if (nowcnt == 0)
                {
                    if (key_name == "space")
                    {
                        QAnowcnt.Text = nowcnt.ToString();
                        QAmisscnt.Text = misscnt.ToString();
                        bfa = QAd.Keys.ToList()[nowcnt];
                        Qarea.Text = QAd.Values.ToList()[nowcnt][0];
                        bflen = QAd.Keys.ToList()[nowcnt].Length;
                        QAfilename.Text = QAd.Values.ToList()[nowcnt][1];
                        QAlinecnt.Text = QAd.Values.ToList()[nowcnt][2];
                        type = QAd.Values.ToList()[nowcnt][3];
                        bfkey = "space";
                        nextkey = bfa.Substring(iqacnt, 1);
                        iqacnt = 0;
                        //Debug.Print(bfa + " " + bflen);
                        nowcnt++;
                        keyb(bfkey);
                        keyc(nextkey);
                        Qprogress.Maximum = allcnt;
                        Qprogress.Value = nowcnt;
                        Aprogress.Maximum = bflen;
                        Aprogress.Value = iqacnt;
                        nowplay = true;
                        sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        viewsw();
                    }
                }
                else
                {
                    keylist = keyname();
                    if (keylist.ContainsKey(key_name.ToLower()))
                    {
                        typecnt++;
                        if (key_name == nextkey)
                        {
                            if (iqacnt < bflen - 1)
                            {
                                QAtypecnt.Text = typecnt.ToString();
                                bfkey = nextkey;
                                iqacnt++;
                                nextkey = bfa.Substring(iqacnt, 1);
                                Aarea.Text = bfa.Substring(0, iqacnt);
                                keyb(bfkey);
                                keyc(nextkey);
                                Aprogress.Value = iqacnt;
                            }
                            else
                            {
                                if (nowcnt < allcnt)
                                {
                                    QAnowcnt.Text = nowcnt.ToString();
                                    Aarea.Text = bfa;
                                    sw.Stop();
                                    await Task.Delay(5);
                                    sw.Start();
                                    Qarea.Text = QAd.Values.ToList()[nowcnt][0];
                                    Aarea.Text = "";
                                    bflen = QAd.Keys.ToList()[nowcnt].Length;
                                    QAfilename.Text = QAd.Values.ToList()[nowcnt][1];
                                    QAlinecnt.Text = QAd.Values.ToList()[nowcnt][2];
                                    type = QAd.Values.ToList()[nowcnt][3];
                                    iqacnt = 0;
                                    bfkey = nextkey;
                                    bfa = QAd.Keys.ToList()[nowcnt];
                                    nowcnt++;
                                    nextkey = bfa.Substring(iqacnt, 1);
                                    keyb(bfkey);
                                    keyc(nextkey);
                                    Aprogress.Maximum = bflen;
                                    Aprogress.Value = iqacnt;
                                    Qprogress.Value = nowcnt;
                                    //Debug.Print(bfa + " " + bflen);
                                }
                                else
                                {
                                    Qarea.Text = "終了！";
                                    Aarea.Text = "";
                                    nowplay = false;
                                    await Task.Delay(5000);
                                    var tpage = new HomePage();
                                    NavigationService.Navigate(tpage);
                                }
                            }
                        }
                        else
                        {
                            missback();
                            misscnt++;
                            QAmisscnt.Text = misscnt.ToString();
                        }
                    }
                }
            }
        }


        private void Keyboardkey_Click(object sender, RoutedEventArgs e)
        {
        }

        public void start()
        {
            allcnt = QAd.Keys.Count;
            nowcnt = 0;
            misscnt = 0;
            typecnt = 0;
            QAallcnt.Text = allcnt.ToString();
            keyc("space");
        }
        public void ime(string keyname)
        {

        }
        public Dictionary<string,string> keyname()
        {
            return new Dictionary<string, string>()
            {
                {"shift","klshift_b"},
                {"space","kspace_b"},
                {"a","ka_b"},
                {"b","kb_b"},
                {"c","kc_b"},
                {"d","kd_b"},
                {"e","ke_b"},
                {"f","kf_b"},
                {"g","kg_b"},
                {"h","kh_b"},
                {"i","ki_b"},
                {"j","kj_b"},
                {"k","kk_b"},
                {"l","kl_b"},
                {"m","km_b"},
                {"n","kn_b"},
                {"o","ko_b"},
                {"p","kp_b"},
                {"q","kq_b"},
                {"r","kr_b"},
                {"s","ks_b"},
                {"t","kt_b"},
                {"u","ku_b"},
                {"v","kv_b"},
                {"w","kw_b"},
                {"x","kx_b"},
                {"y","ky_b"},
                {"z","kz_b"},
                {"1","k1_b"},
                {"2","k2_b"},
                {"3","k3_b"},
                {"4","k4_b"},
                {"5","k5_b"},
                {"6","k6_b"},
                {"7","k7_b"},
                {"8","k8_b"},
                {"9","k9_b"},
                {"0","k0_b"},
                {".","kpgt_b"},
                {",","kclt_b"},
                {"\\","kby_b"},
                {"/","ksqs_b"},
            };
        }
        public void im()
        {
            string ans = "ab";

            Debug.Print(ans);

            short[][] redata = new short[0][];

            for (int cnt=0;cnt<ans.Length;cnt++)
            {
                string letter = ans.Substring(cnt, 1);
                short[] rekeycode = new short[0];

                switch (letter)
                {
                    case "a":
                        Array.Resize(ref rekeycode, 1);
                        rekeycode = new short[] { 44 };
                        break;
                    case "b":
                        Array.Resize(ref rekeycode, 1);
                        rekeycode = new short[] { 45 };
                        break;
                    default:
                        break;
                }

                if (rekeycode.Length>0)
                {
                    short[][] redataed = redata;
                    for (int rkcc=0;rkcc<rekeycode.Length; rkcc++)
                    {
                        short[][] redataedtm = redataed;
                        if (redata.Length>0)
                        {
                            for (int rdcnt = 0; rdcnt<redata.Length; rdcnt++)
                            {
                                redataedtm[rdcnt] = redataed[rdcnt].Concat(new short[] { rekeycode[rkcc] }).ToArray();
                            }
                            redata = redata.Concat(redataedtm).ToArray();
                        }
                        else
                        {

                            Array.Resize(ref redata, 1);
                            redata[0] = new short[] { rekeycode[rkcc] };
                        }
                    }
                }
            }

            foreach(short[] tmd in redata)
            {
                Debug.Print(String.Join(",", tmd));
            }

        }
    }
}
