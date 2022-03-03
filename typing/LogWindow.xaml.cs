using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace typing
{
    /// <summary>
    /// LogWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LogWindow : Window
    {
        private ObservableCollection<LogData> logs = new ObservableCollection<LogData>();
        public LogWindow()
        {
            InitializeComponent();
            setcolortheme();
            Logs.ItemsSource = logs;
        }

        public void setText(int status,string contents,string mode="push")
        {
            if (mode=="set")
            {
                logs = new ObservableCollection<LogData>() { new LogData() { status=status.ToString(), contents=contents } };
            }
            if (mode=="add")
            {
                logs.Add(new LogData() { status=status.ToString() ,contents=contents });
            }
            if (mode=="push")
            {
                logs.Insert(0,new LogData() { status=status.ToString(), contents=contents });
            }
        }
        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }


        protected virtual void LogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            int keycode = ((int)key);

            Debug.Print(keycode.ToString());

            ModifierKeys modifierKeys = Keyboard.Modifiers;
            if ((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (keycode)
                {
                    case 44:
                        this.Show();
                        break;
                    case 45:
                        this.Hide();
                        break;
                }
            }
        }
    }
    public class LogData
    {
        public string status { get; set; }
        public DateTime datetime { get; set; } = DateTime.Now;
        public string contents { get; set; }
    }
}