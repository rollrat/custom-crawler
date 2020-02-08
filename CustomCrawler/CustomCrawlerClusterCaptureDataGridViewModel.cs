/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler
{
    public class CustomCrawlerClusterCaptureDataGridItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HtmlNode Node;

        private string _index;
        public string Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                OnPropertyChanged();
            }
        }

        private string _depth;
        public string Info
        {
            get { return _depth; }
            set
            {
                if (_depth == value) return;
                _depth = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string DateTime
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public class CustomCrawlerClusterCaptureDataGridViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<CustomCrawlerClusterCaptureDataGridItemViewModel> _items;
        public ObservableCollection<CustomCrawlerClusterCaptureDataGridItemViewModel> Items => _items;

        public CustomCrawlerClusterCaptureDataGridViewModel(IEnumerable<CustomCrawlerClusterCaptureDataGridItemViewModel> collection = null)
        {
            if (collection == null)
                _items = new ObservableCollection<CustomCrawlerClusterCaptureDataGridItemViewModel>();
            else
                _items = new ObservableCollection<CustomCrawlerClusterCaptureDataGridItemViewModel>(collection);
        }
    }
}
