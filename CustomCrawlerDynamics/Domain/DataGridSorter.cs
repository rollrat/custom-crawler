// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CustomCrawlerDynamics.Domain
{
    public static class SortAlgorithm
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        static extern int StrCmpLogicalW(string psz1, string psz2);

        public static int ComparePath(string addr1, string addr2)
        {
            return StrCmpLogicalW(addr1, addr2);
        }
    }

    public class DataGridSorter<T> where T : new()
    {
        DataGrid data_grid;

        public class SortComparer : IComparer
        {
            private bool @ascending;
            private string column;
            private T comparator;

            public SortComparer(bool asc, string column)
            {
                this.@ascending = asc;
                comparator = new T();
                this.column = column;
            }

            public int Compare(object x, object y)
            {
                if ((x == null) | (y == null))
                    return 0;

                T xItem = (T)x;
                T yItem = (T)y;

                var x_value = xItem.GetType().GetProperty(column, BindingFlags.Public | BindingFlags.Instance).GetValue(xItem);
                var y_value = yItem.GetType().GetProperty(column, BindingFlags.Public | BindingFlags.Instance).GetValue(yItem);

                string xText = x_value != null ? x_value.ToString().Replace(",", "") : "";
                string yText = y_value != null ? y_value.ToString().Replace(",", "") : "";

                return SortAlgorithm.ComparePath(xText, yText) * (this.@ascending ? 1 : -1);
            }
        }

        public DataGridSorter(DataGrid data_grid)
        {
            this.data_grid = data_grid;
        }

        public void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;
            IComparer comparer = null;

            ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending)
                ? ListSortDirection.Ascending : ListSortDirection.Descending;
            column.SortDirection = direction;

            ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(data_grid.ItemsSource);

            comparer = new SortComparer(direction == 0 ? false : true, e.Column.SortMemberPath);
            lcv.CustomSort = comparer;

            e.Handled = true;
        }
    }
}
