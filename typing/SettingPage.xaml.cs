﻿using System;
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
using System.Configuration;
using System.Windows.Shapes;

namespace typing
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : Page
    {
        MainWindow mainwindow;
        public SettingPage()
        {
            mainwindow = (MainWindow)Application.Current.MainWindow;
            mainwindow.setText(0,"Open Setting");

            InitializeComponent();
            settheme();

            colortheme.ItemsSource = new Dictionary<string, string>()
            {
                { "colorthemes/default.xaml", "default" },
                { "colorthemes/lightgreen.xaml", "lightgreen" },
                { "colorthemes/dark.xaml", "dark" },
                { "colorthemes/yellow.xaml", "yellow" },
            };
            langtheme.ItemsSource = new Dictionary<string, string>()
            {
                { "languages/japanese.xaml", "japanese" },
                { "languages/english.xaml", "english" },
            };
            show_setting();
        }

        public void settheme()
        {
            try
            {
                string dicPath = Properties.Settings.Default.colortheme;
                ResourceDictionary dic = new ResourceDictionary();
                dic.Source = new Uri(dicPath, UriKind.Relative);
                this.Resources.MergedDictionaries.Add(dic);

                dicPath = Properties.Settings.Default.langtheme;
                dic = new ResourceDictionary();
                dic.Source = new Uri(dicPath, UriKind.Relative);
                this.Resources.MergedDictionaries.Add(dic);
            }
            catch (Exception e)
            {
            }
        }

        public void show_setting()
        {
            username.Text = Properties.Settings.Default.username;
            colortheme.SelectedValue = Properties.Settings.Default.colortheme;
            langtheme.SelectedValue = Properties.Settings.Default.langtheme;
            serchdir.Text = Properties.Settings.Default.questions_dir;
            keyboardfiles.Text = Properties.Settings.Default.keyboard_dir;
            scale.Value = Properties.Settings.Default.scale;
            posturl.Text = Properties.Settings.Default.posturl;
        }


        private void save()
        {
            Properties.Settings.Default.username = username.Text;
            Properties.Settings.Default.colortheme = colortheme.SelectedValue.ToString();
            Properties.Settings.Default.langtheme = langtheme.SelectedValue.ToString();
            Properties.Settings.Default.questions_dir = serchdir.Text;
            Properties.Settings.Default.keyboard_dir = keyboardfiles.Text;
            Properties.Settings.Default.scale = scale.Value;
            Properties.Settings.Default.posturl = posturl.Text;

            Properties.Settings.Default.Save();
            setscale();
            settheme();
            mainwindow.call_settheme_logwindow();
        }
        private void save(object sender, RoutedEventArgs e) {save();}
        private void save_Go_home(object sender, RoutedEventArgs e)
        {
            save();
            setscale();
            settheme();
            var tpage = new HomePage();
            NavigationService.Navigate(tpage);
        }
        private void upgrade()
        {
            Properties.Settings.Default.Upgrade();

            show_setting();
        }
        private void upgrade(object sender, RoutedEventArgs e) { upgrade(); }

        private void cancel(object sender, RoutedEventArgs e)
        {
            settheme();

            show_setting();
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
