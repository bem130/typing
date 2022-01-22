/*!

MIT License

Copyright (c) 2022 haruki1234

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
/*!

If you want to contact me, you can probably find how to do it here:
   https://github.com/haruki1234

*/
/*!

Reference sites:
   https://dobon.net/
   https://qiita.com/
   https://itsakura.com/
   https://baba-s.hatenablog.com/
   https://usefuledge.com/
     etc.
 [apologize] There may be license violation etc.
   because I'm not used to license etc.
   so if I have any violation, please contact me

*/


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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace typing
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            setscale();
            setcolortheme();

            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            this.Title = asm.GetName().Name;
            this.Title += " v" + asm.GetName().Version.ToString();

            string name = "AssemblyName.Name : " + asm.GetName().Name + "\r\n";
            string version = "AssemblyName.Version : " + asm.GetName().Version.ToString() + "\r\n";
            string fullname = "AssemblyName.FullName : " + asm.GetName().FullName + "\r\n";
            string processor = "AssemblyName.ProcessorArchitecture : " + asm.GetName().ProcessorArchitecture + "\r\n";
            string runtime = "Assembly.ImageRuntimeVersion : " + asm.ImageRuntimeVersion + "\r\n";

            {
                Debug.Print("\n\n");
                Debug.Print(name + version + fullname + processor + runtime);
                
                Debug.Print("CommandLineArgs:");
                //コマンドライン引数を表示する
                Debug.Print(System.Environment.CommandLine);

                //コマンドライン引数を配列で取得する
                List<string> cmds = new List<string>(); ;
                cmds.AddRange(System.Environment.GetCommandLineArgs());
                cmds.RemoveAt(0);
                //コマンドライン引数を列挙する
                foreach (string cmd in cmds)
                {
                    Debug.Print(cmd);
                }
                Debug.Print("\n\n");

                Uri uri;
                Debug.Print(cmds.Count.ToString());
                if (cmds.Count > 0)
                {
                    Application.Current.Properties["FilePaths"] = string.Join("|", cmds);
                    Debug.Print("filepaths "+ Application.Current.Properties["FilePaths"]);
                    uri = new Uri("PlayPage.xaml", UriKind.Relative);
                    frame.Source = uri;
                }
                else
                {
                    uri = new Uri("TitlePage.xaml", UriKind.Relative);
                    frame.Source = uri;
                }
            }
        }

        public void setcolortheme()
        {
            string dicPath = Properties.Settings.Default.colortheme;
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new Uri(dicPath, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dic);
        }

        public void setscale()
        {
            int[] dmsize = { 800, 600 };

            double scale = Properties.Settings.Default.scale;
            Scaletrans.ScaleX = scale;
            Scaletrans.ScaleY = scale;
            MinHeight = dmsize[1]*scale;
            MinWidth = dmsize[0]*scale;
        }

        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;

        /// <summary>
        /// 画面遷移前にFrameから発生するイベント
        /// デフォルトの処理をキャンセルし、アニメーションしながら画面遷移するようにします。
        /// </summary>
        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (frame.Content != null && !_allowDirectNavigation)
            {
                e.Cancel = true;
                _navArgs = e;

                // 遷移前のページを画像に変換しイメージに設定
                var visual = frame;
                var bounds = VisualTreeHelper.GetDescendantBounds(visual);
                var bitmap = new RenderTargetBitmap(
                    (int)bounds.Width,
                    (int)bounds.Height,
                    96.0,
                    96.0,
                    PixelFormats.Pbgra32);
                var dv = new DrawingVisual();
                using (var dc = dv.RenderOpen())
                {
                    var vb = new VisualBrush(visual);
                    dc.DrawRectangle(vb, null, bounds);
                }
                bitmap.Render(dv);
                bitmap.Freeze();
                image.Source = bitmap;

                // フレームに遷移先のページを設定
                _allowDirectNavigation = true;
                frame.Navigate(_navArgs.Content);

                ThicknessAnimation animation0 = new ThicknessAnimation();
                animation0.From = new Thickness(0, -frame.ActualHeight,0, frame.ActualHeight);
                animation0.To = new Thickness(0, 0, 0, 0);
                animation0.Duration = TimeSpan.FromMilliseconds(100);
                frame.BeginAnimation(MarginProperty, animation0);

                ThicknessAnimation animation1 = new ThicknessAnimation();
                animation1.From = new Thickness(0, 0, 0, 0);
                animation1.To = new Thickness(0,frame.ActualHeight, 0, -frame.ActualHeight);
                animation1.Duration = TimeSpan.FromMilliseconds(200);

                image.BeginAnimation(MarginProperty, animation1);
            }

            _allowDirectNavigation = false;
        }

    }
}
