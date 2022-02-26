using System;
using System.IO;
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
using Microsoft.Win32;
// using System.Windows.Forms;
using System.Diagnostics;

namespace typing
{
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            //TextBlock a = title;
            //var b = new Run(" Home Page");
            //b.Foreground = new SolidColorBrush(Color.FromArgb(250,0,252,25));
            //a.Inlines.Add(b);

            setcolortheme();

            serchdir.Text = Properties.Settings.Default.questions_dir;

            Menu.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
            makemenu();

        }

        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }
        private void makemenu()
        {
            List<string> files = new List<string>();
            try
            {
                IEnumerable<FileInfo> names = Files(serchdir.Text, "*.ntd");
                foreach (FileInfo name in names)
                {
                    files.Add(name.FullName);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
            Menu.ItemsSource = files;
        }

        private void Go_title(object sender, RoutedEventArgs e)
        {
            var tpage = new TitlePage();
            NavigationService.Navigate(tpage);
        }

        private void Go_setting(object sender, RoutedEventArgs e)
        {
            var tpage = new SettingPage();
            NavigationService.Navigate(tpage);
        }

        private void menu_update(object sender, RoutedEventArgs e)
        {
            makemenu();
        }

        private void start(object sender, RoutedEventArgs e)
        {
            if (Menu.SelectedItems.Count == 0)
            {
                // 選択項目がないので処理をせず抜ける
                return;
            }
            List<string> menupath = new List<string>();
            foreach (string path_ in Menu.SelectedItems)
            {
                menupath.Add(path_.ToString());
            }

            Application.Current.Properties["FilePaths"] = string.Join("|", menupath);
            Debug.Print("filepaths " + Application.Current.Properties["FilePaths"]);
            var tpage = new PlayPage();
            NavigationService.Navigate(tpage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // フォルダー参照ダイアログのインスタンスを生成
            var dlg = new System.Windows.Forms.FolderBrowserDialog();

            // 説明文を設定
            dlg.SelectedPath = serchdir.Text;
            dlg.Description = "フォルダーを選択してください。";

            // ダイアログを表示
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスをメッセージボックスに表示
                serchdir.Text = dlg.SelectedPath;
            }
        }

        // https://resanaplaza.com/%E3%80%90c%E3%80%91%E3%83%95%E3%82%A9%E3%83%AB%E3%83%80%E9%85%8D%E4%B8%8B%E3%81%AE%E5%85%A8%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%81%8B%E3%82%89%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E6%83%85%E5%A0%B1/
        // files serch
        static public IEnumerable<FileInfo> Files(string directory, string filter, SearchOption searchOption = SearchOption.AllDirectories)
        {
            List<FileInfo> infos = new List<FileInfo>();

            //ディレクトリが未指定ならカレントディレクトリを対象とする。
            directory = (directory.Trim() == "") ? System.IO.Directory.GetCurrentDirectory() : directory;

            //指定されたディレクトリの情報を取得
            DirectoryInfo dir_top = new DirectoryInfo(directory);
            try
            {
                //指定されたディレクトリ直下に存在するファイル情報を取得
                foreach (var info in dir_top.EnumerateFiles(filter))
                {
                    infos.Add(info);
                }
            }
            catch { }

            //取得したファイル情報を返す
            foreach (var info in infos)
            {
                yield return info;
            }

            //サーチオプションが配下のディレクトリを検索対象にしている場合
            if (searchOption == SearchOption.AllDirectories)
            {
                //指定されたディレクトリ直下にあるディレクトリを全て取得
                foreach (var dir_info in dir_top.EnumerateDirectories("*"))
                {
                    infos.Clear();

                    try
                    {
                        //取得したディレクトリ直下から末端までの階層をたぐって全てのファイル情報を取得
                        foreach (var info in dir_info.EnumerateFiles(filter, SearchOption.AllDirectories))
                        {
                            infos.Add(info);
                        }
                    }
                    catch { }

                    //取得したファイル情報を返す
                    foreach (var info in infos)
                    {
                        yield return info;
                    }
                }
            }
        }

    }
}
