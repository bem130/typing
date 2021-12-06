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
using System.Windows.Threading;

using System.Diagnostics;

namespace typing
{
    /// <summary>
    /// Page2.xaml の相互作用ロジック
    /// </summary>
    public partial class TitlePage : Page
    {
        bool cpage = false;

        public TitlePage()
        {
            InitializeComponent();
            ProgressBar.IsIndeterminate = true;
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            String Version = asm.GetName().Version.ToString();
            {
                Debug.Print("\n\n");
                Debug.Print("Version:"+Version);
                versiondata.Text = "version "+Version;
                Debug.Print("CommandLineArgs:");
                //コマンドライン引数を表示する
                Debug.Print(System.Environment.CommandLine);

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                //コマンドライン引数を列挙する
                foreach (string cmd in cmds)
                {
                    Debug.Print(cmd);
                }
                Debug.Print("\n\n");
            }

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Start();
            timer.Tick += (s, args) =>
            {
                // タイマーの停止
                timer.Stop();
                ProgressBar.IsIndeterminate = false;

                // 以下に待機後の処理を書く
                if (cpage == false)
                {
                    var tpage = new HomePage();
                    NavigationService.Navigate(tpage);
                }
            };
        }

        private void Go_title(object sender, RoutedEventArgs e)
        {
            cpage = true;
            var tpage = new HomePage();
            NavigationService.Navigate(tpage);
        }
    }
}
