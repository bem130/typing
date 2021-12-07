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

            // CSVファイルの読み込み
            //foreach (string filepath in filepaths) {
            //     streamreaderクラスをインスタンス化
            //    streamreader reader = new streamreader(filepath, encoding.getencoding("utf-8"));
            //     最後まで読み込む
            //    while (reader.peek() >= 0)
            //    {
            //         読み込んだ文字列をカンマ区切りで配列に格納
            //        string[] cols = reader.readline().split('◊');
            //        for (int n = 0; n < cols.length; n++)
            //        {
            //             表示
            //            debug.print(cols[n] + ",");
            //        }
            //    }
            //    reader.close();
            //}

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
