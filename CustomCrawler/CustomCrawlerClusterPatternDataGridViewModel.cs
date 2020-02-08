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
    public class CustomCrawlerClusterPatternDataGridItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustomCrawlerCluster.Pattern Patterns;

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

        private string _name;
        public string Pattern
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

    public class CustomCrawlerClusterPatternDataGridViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<CustomCrawlerClusterPatternDataGridItemViewModel> _items;
        public ObservableCollection<CustomCrawlerClusterPatternDataGridItemViewModel> Items => _items;

        public CustomCrawlerClusterPatternDataGridViewModel(IEnumerable<CustomCrawlerClusterPatternDataGridItemViewModel> collection = null)
        {
            if (collection == null)
                _items = new ObservableCollection<CustomCrawlerClusterPatternDataGridItemViewModel>();
            else
                _items = new ObservableCollection<CustomCrawlerClusterPatternDataGridItemViewModel>(collection);
        }
    }
}
