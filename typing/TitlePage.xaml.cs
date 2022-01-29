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
            setcolortheme();
            ProgressBar.IsIndeterminate = true;
            versiondata.Text = "version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

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

        private void Go_home(object sender, RoutedEventArgs e)
        {
            cpage = true;
            var tpage = new HomePage();
            NavigationService.Navigate(tpage);
        }

        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }

    }
}
