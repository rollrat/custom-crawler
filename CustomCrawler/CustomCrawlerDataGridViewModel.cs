/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

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
    public class CustomCrawlerDataGridItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int i;
        public int j;

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
        public string Depth
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
        public string Tag
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
        public string Contents
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

    public class CustomCrawlerDataGridViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<CustomCrawlerDataGridItemViewModel> _items;
        public ObservableCollection<CustomCrawlerDataGridItemViewModel> Items => _items;

        public CustomCrawlerDataGridViewModel(IEnumerable<CustomCrawlerDataGridItemViewModel> collection = null)
        {
            if (collection == null)
                _items = new ObservableCollection<CustomCrawlerDataGridItemViewModel>();
            else
                _items = new ObservableCollection<CustomCrawlerDataGridItemViewModel>(collection);
        }
    }
}
