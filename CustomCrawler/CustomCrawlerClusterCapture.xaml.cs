/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

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

namespace CustomCrawler
{
    /// <summary>
    /// CustomCrawlerClusterCapture.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerClusterCapture : Window
    {
        public CustomCrawlerClusterCapture()
        {
            InitializeComponent();

            Loaded += CustomCrawlerClusterCapture_Loaded;
        }

        private void CustomCrawlerClusterCapture_Loaded(object sender, RoutedEventArgs e)
        {
            Tag.Focus();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            insert();
        }

        private void Tag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                insert();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void insert()
        {
            (Owner as CustomCrawlerCluster).AppendCapture(Tag.Text);
            Close();
        }
    }
}
