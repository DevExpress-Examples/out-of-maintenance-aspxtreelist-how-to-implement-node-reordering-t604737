using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NodeReordering.Models
{
    public class DataHelper
    {

        private string _sessionKey = "ExampleDataSource";
        public List<SampleDataItem> Data
        {
            get
            {
                if (HttpContext.Current.Session[_sessionKey] == null)
                    HttpContext.Current.Session[_sessionKey] = this.GetSampleData();
                return (List<SampleDataItem>)HttpContext.Current.Session[_sessionKey];
            }
            set { }
        }

        private List<SampleDataItem> GetSampleData()
        {
            List<SampleDataItem> result = new List<SampleDataItem>();
            result.Add(new SampleDataItem("root", 0, null));
            result.Add(new SampleDataItem("a", 1, 0));
            result.Add(new SampleDataItem("b", 2, 0));
            result.Add(new SampleDataItem("a1", 3, 1));
            result.Add(new SampleDataItem("a2", 4, 1));
            result.Add(new SampleDataItem("a3", 5, 1));
            result.Add(new SampleDataItem("b1", 6, 2));
            result.Add(new SampleDataItem("b2", 7, 2));
            result.Add(new SampleDataItem("b1a", 8, 6));
            result.Add(new SampleDataItem("b1b", 9, 6));
            result.Add(new SampleDataItem("b1c", 10, 6));
            return result;
        }

        public void SwapDataItems(SampleDataItem item1, SampleDataItem item2)
        {
            if (item1 == null || item2 == null) return;
            int index1 = Data.IndexOf(item1);
            int index2 = Data.IndexOf(item2);
            Data[index1] = item2;
            Data[index2] = item1;
        }

    }

    public class SampleDataItem
    {
        public string Title { get; set; }
        public int Key { get; set; }
        public int? ParentKey { get; set; }

        public SampleDataItem(string title, int key, int? parentKey)
        {
            Title = title;
            Key = key;
            ParentKey = parentKey;
        }
    }
}