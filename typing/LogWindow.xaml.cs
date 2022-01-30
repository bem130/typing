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
using System.Windows.Shapes;


namespace typing
{
    /// <summary>
    /// LogWindow.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class LogWindow : Window
    {
        List<Log> Logdatas;
        public LogWindow(List<Log> logdatas = null,string title = "LogWindow")
        {
            InitializeComponent();
            Title = title;
            Logdatas = new List<Log>();
            if (logdatas != null)
            {
                Logdatas = logdatas;
            }
            logView.ItemsSource = Logdatas;
        }
        public void update(List<Log> logdatas = null, string title = "LogWindow")
        {
            Title = title;
            Logdatas = logdatas;
            logView.ItemsSource = Logdatas;
        }
    }
}