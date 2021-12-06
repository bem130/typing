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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;


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

            Uri uri = new Uri("TitlePage.xaml", UriKind.Relative);
            frame.Source = uri;
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

                // フレームを右からスライドさせるアニメーション
                ThicknessAnimation animation0 = new ThicknessAnimation();
                animation0.From = new Thickness(0, -frame.ActualHeight,0, frame.ActualHeight);
                animation0.To = new Thickness(0, 0, 0, 0);
                animation0.Duration = TimeSpan.FromMilliseconds(100);
                frame.BeginAnimation(MarginProperty, animation0);

                // 遷移前ページを画像可した要素を左にスライドするアニメーション
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
