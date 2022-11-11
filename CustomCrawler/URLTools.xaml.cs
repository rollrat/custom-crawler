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
    /// URLTools.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class URLTools : Window
    {
        public URLTools()
        {
            InitializeComponent();
        }

        private void URLButton_Click(object sender, RoutedEventArgs e)
        {

            var uh = new URLHelper(URLText.Text);

            foreach (var param in uh.Parameters)
            {
                Process.Text += $"{param.Key} = {param.Value}\n";
            }
        }
    }
}
