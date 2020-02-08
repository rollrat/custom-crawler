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
    public class CustomCrawlerClusterDataGridItemViewModel : INotifyPropertyChanged
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
        public string Count
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
        public string Accuracy
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _specific;
        public string Header
        {
            get { return _specific; }
            set
            {
                if (_specific == value) return;
                _specific = value;
                OnPropertyChanged();
            }
        }
    }

    public class CustomCrawlerClusterDataGridViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<CustomCrawlerClusterDataGridItemViewModel> _items;
        public ObservableCollection<CustomCrawlerClusterDataGridItemViewModel> Items => _items;

        public CustomCrawlerClusterDataGridViewModel(IEnumerable<CustomCrawlerClusterDataGridItemViewModel> collection = null)
        {
            if (collection == null)
                _items = new ObservableCollection<CustomCrawlerClusterDataGridItemViewModel>();
            else
                _items = new ObservableCollection<CustomCrawlerClusterDataGridItemViewModel>(collection);
        }
    }
}
