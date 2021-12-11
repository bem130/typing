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
        public PlayPage()
        {
            InitializeComponent();
            read_file();
            keyc("space");

            keyc("a");
            keyc("b");
            keyc("c");
            keyc("d");
            keyc("e");
            keyc("f");
            keyc("g");
            keyc("h");
            keyc("i");
            keyc("j");
            keyc("k");
            keyc("l");
            keyc("m");
            keyc("n");
            keyc("o");
            keyc("p");
            keyc("q");
            keyc("r");
            keyc("s");
            keyc("t");
            keyc("u");
            keyc("v");
            keyc("w");
            keyc("x");
            keyc("y");
            keyc("z");
            keyc("1");
            keyc("2");
            keyc("3");
            keyc("4");
            keyc("5");
            keyc("6");
            keyc("7");
            keyc("8");
            keyc("9");
            keyc("0");
            keyc(".");
            keyc(",");
            keyc("/");
            keyc("\\");
        }
        private void read_file()
        {
            string[] filepaths = ((string)Application.Current.Properties["FilePaths"]).Split('|');

            var QAd = new Dictionary<string, string>();

            foreach (string filePath in filepaths)
            {
                StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
                var fprop = new Dictionary<string, string>();
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

                while (reader.Peek() >= 0)
                {
                    string[] qaline = reader.ReadLine().Split(split_l);
                    QAd[qaline[0]] = qaline[1];
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
            s += string.Format("  Key={0}\n  KeyStates={1}\n  IsRepeat={2}\n",
                    key, keyStates, isRepeat);
            ModifierKeys modifierKeys = Keyboard.Modifiers;
            if ((modifierKeys & ModifierKeys.Alt) != ModifierKeys.None)
                s += "  Alt ";
            if ((modifierKeys & ModifierKeys.Control) != ModifierKeys.None)
                s += "  Control ";
            if ((modifierKeys & ModifierKeys.Shift) != ModifierKeys.None)
                s += "  Shift ";
            if ((modifierKeys & ModifierKeys.Windows) != ModifierKeys.None)
                s += "  Windows";
            if (key == Key.System)
                s += systemKey;
            Debug.Print(s);
        }


        private void Keyboardkey_Click(object sender, RoutedEventArgs e)
        {
            Debug.Print("buttonc");
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
