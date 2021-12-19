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

namespace typing
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            setcolortheme();

            colortheme.ItemsSource = new Dictionary<string, string>()
            {
                { "colorthemes/default.xaml", "default" },
                { "colorthemes/dark.xaml", "dark" },
            };
            colortheme.SelectedValue = Properties.Settings.Default.colortheme;
            serchdir.Text = Properties.Settings.Default.questions_dir;
            scale.Value = Properties.Settings.Default.scale;
        }

        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }

        private void Go_home(object sender, RoutedEventArgs e)
        {
            var tpage = new HomePage();
            NavigationService.Navigate(tpage);
        }

        private void save_data(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.colortheme = colortheme.SelectedValue.ToString();
            Properties.Settings.Default.questions_dir = serchdir.Text;
            Properties.Settings.Default.scale = scale.Value;
            Properties.Settings.Default.Save();
            setscale();
            setcolortheme();
        }

        public void setscale()
        {
            int[] dmsize = { 800, 600 };

            var window = (MainWindow)Application.Current.MainWindow;
            double scale = Properties.Settings.Default.scale;
            window.Scaletrans.ScaleX = scale;
            window.Scaletrans.ScaleY = scale;
            window.MinHeight = dmsize[1]*scale;
            window.MinWidth = dmsize[0]*scale;
        }

    }
}
