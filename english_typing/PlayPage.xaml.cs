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

using System.Diagnostics;

namespace english_typing
{
    /// <summary>
    /// PlayPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();
        }
        private void keyarea_load(object sender, RoutedEventArgs e)
        {
            keyarea.Focus();
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
