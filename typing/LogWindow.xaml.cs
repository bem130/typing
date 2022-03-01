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
using System.Diagnostics;
using System.Windows.Shapes;

namespace typing
{
    /// <summary>
    /// LogWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        public void setText(string t,string mode="set")
        {
            if (mode=="set")
            {
                text.Text = t;
            }
            if (mode=="add")
            {
                text.Text += t + "\n";
            }
            if (mode=="push")
            {
                text.Text = t + "\n" + text.Text;
            }
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
                    case 46:
                        this.UpdateLayout();
                        break;
                }
            }
        }
    }
}