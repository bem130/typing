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

            Menu.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
            makemenu();
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

            Uri uri;
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
