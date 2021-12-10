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
            kspace.Focus();
            kl_b.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#AA5588D1");
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
    }
}
