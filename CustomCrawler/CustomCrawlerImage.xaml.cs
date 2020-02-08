/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace CustomCrawler
{
    public partial class CustomCrawlerImage : Window
    {
        string image_url;
        string referer;
        public CustomCrawlerImage(string image_url, string referer)
        {
            InitializeComponent();

            Loaded += CustomCrawlerImage_LoadedAsync;
            this.image_url = image_url;
            this.referer = referer;
        }

        bool load = false;
        private async void CustomCrawlerImage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            if (load) return;
            load = true;
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers.Add(HttpRequestHeader.Referer, referer);
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
                    var bytes = await wc.DownloadDataTaskAsync(image_url);
                    using (var stream = new MemoryStream(bytes))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = new MemoryStream(bytes);
                        bitmap.EndInit();
                        bitmap.Freeze();
                        Image.Source = bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
