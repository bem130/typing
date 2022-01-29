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
    /// ResultPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ResultPage : Page
    {
        public ResultPage()
        {
            InitializeComponent();
            show();
        }
        private void Go_home(object sender, RoutedEventArgs e)
        {
            var tpage = new HomePage();
            NavigationService.Navigate(tpage);
        }
        public void show()
        {
            Dictionary<string, string> dic = get();

            allcntv.Text = dic["allcnt"];
            typecntv.Text = dic["typecnt"];
            miscntv.Text = dic["miscnt"];
            timev.Text = dic["time"];
        }
        public Dictionary<string,string> get()
        {
            string strdata = ((string)Application.Current.Properties["Result"]);

            Dictionary<string,string> dic = new Dictionary<string, string>()
            {
                {"allcnt","0"},
                {"typecnt","0"},
                {"miscnt","0"},
                {"time","0"},
            };
            string[] sstrdata = strdata.Split(';');
            foreach (string s in sstrdata)
            {
                string[] ss = s.Split(':');
                if (ss.Length > 1)
                {
                    dic[ss[0]] = ss[1];
                }
            }
            return dic;
        }
    }
}
