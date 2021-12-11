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
        string type;
        Dictionary<string, List<string>> QAd;

        string bfa;
        int bflen;
        int allcnt;
        int nowcnt;
        int iqacnt;
        int hitcnt;
        int misscnt;
        int typecnt;
        public PlayPage()
        {
            InitializeComponent();
            read_file();
            start();

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
                        QAd[qaline[0]] = new List<string>() { qaline[1], filePath ,line.ToString()};
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
            ((Border)FindName(keyname()[keyname_])).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            Debug.Print("keydown");
            Key key = e.Key;
            Key systemKey = e.SystemKey;
            KeyStates keyStates = e.KeyStates;
            bool isRepeat = e.IsRepeat;
            string s = "";
            s += string.Format("  Key={0}  KeyStates={1}  IsRepeat={2}", key, keyStates, isRepeat);
            ModifierKeys modifierKeys = Keyboard.Modifiers;
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None)
                s += "  Shift ";
            Debug.Print(s);

            if (type == "ja-en")
            {
                if (nowcnt == 0)
                {
                    if (key.ToString() == "Space")
                    {
                        bfa = QAd.Keys.ToList()[nowcnt];
                        Qarea.Text = QAd.Values.ToList()[nowcnt][0];
                        bflen = QAd.Keys.ToList()[nowcnt].Length;
                        QAfilename.Text = QAd.Values.ToList()[nowcnt][1];
                        QAlinecnt.Text = QAd.Values.ToList()[nowcnt][2];
                        iqacnt = 0;
                        Debug.Print(bfa + " " + bflen);
                        nowcnt++;
                    }
                }
                else
                {
                    if (iqacnt < bflen)
                    {
                        iqacnt++;
                        Aarea.Text = bfa.Substring(0, iqacnt);
                    }
                    else
                    {
                        bfa = QAd.Keys.ToList()[nowcnt];
                        Qarea.Text = QAd.Values.ToList()[nowcnt][0];
                        bflen = QAd.Keys.ToList()[nowcnt].Length;
                        QAfilename.Text = QAd.Values.ToList()[nowcnt][1];
                        QAlinecnt.Text = QAd.Values.ToList()[nowcnt][2];
                        iqacnt = 0;
                        nowcnt++;
                        Debug.Print(bfa + " " + bflen);
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
            hitcnt = 0;
            misscnt = 0;
            typecnt = 0;
            QAallcnt.Text = allcnt.ToString();
            keyc("space");
        }
        public Dictionary<string,string> keyname()
        {
            return new Dictionary<string, string>()
            {
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
    }
}
