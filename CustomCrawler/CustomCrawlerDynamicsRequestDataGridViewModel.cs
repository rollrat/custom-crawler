/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using MasterDevs.ChromeDevTools.Protocol.Chrome.Network;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Runtime;
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
    public class CustomCrawlerDynamicsRequestDataGridItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestWillBeSentEvent Request { get; set; }
        public ResponseReceivedEvent Response { get; set; }
        public string AnonymouseCode { get; set; }
        public CallFrame AnonymouseSource { get; set; }
        public MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger.ScriptParsedEvent Parsed { get; set; }

        private string _index;
        public string Id
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
        public string Type
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _depth;
        public string Url
        {
            get { return _depth; }
            set
            {
                if (_depth == value) return;
                _depth = value;
                OnPropertyChanged();
            }
        }

        private string _names;
        public string ContentType
        {
            get { return _names; }
            set
            {
                if (_names == value) return;
                _names = value;
                OnPropertyChanged();
            }
        }
    }

    public class CustomCrawlerDynamicsRequestDataGridViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<CustomCrawlerDynamicsRequestDataGridItemViewModel> _items;
        public ObservableCollection<CustomCrawlerDynamicsRequestDataGridItemViewModel> Items => _items;

        public CustomCrawlerDynamicsRequestDataGridViewModel(IEnumerable<CustomCrawlerDynamicsRequestDataGridItemViewModel> collection = null)
        {
            if (collection == null)
                _items = new ObservableCollection<CustomCrawlerDynamicsRequestDataGridItemViewModel>();
            else
                _items = new ObservableCollection<CustomCrawlerDynamicsRequestDataGridItemViewModel>(collection);
        }
    }
}
