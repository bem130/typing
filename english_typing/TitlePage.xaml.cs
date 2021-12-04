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

namespace english_typing
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
