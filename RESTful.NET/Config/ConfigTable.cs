using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Config
{
    public class ConfigTable
    {
        private string[] _header;
        private int _columns;

        private IList<IList<ConfigItem>> _rows;

        public ConfigTable(string[] header)
        {
            _columns = header.Length;
            _header = header;

            _rows = new List<IList<ConfigItem>>();
        }

        public ConfigTable(IList<string> header)
        {
            _columns = header.Count;
            _header = header.ToArray();

            _rows = new List<IList<ConfigItem>>();
        }

        public ConfigTable(int columns) : this(new string[columns])
        {

        }

        public string[] Header
        {
            get
            {
                return _header;
            }
        }

        public int Columns
        {
            get
            {
                return _columns;
            }
        }

        public int Rows
        {
            get
            {
                return _rows.Count;
            }
        }

        public IList<ConfigItem> GetRow(int index)
        {
            if (index < Rows && index >= 0)
            {
                return _rows[index];
            }
            else
            {
                throw new Exception("Index out of bound");
            }
        }

        public ConfigItem GetFieldItem(int index, int column)
        {
            return null;
        }

        public void AddRow(IList<ConfigItem> rowData)
        {

        }

        public void InsertRowAt(IList<ConfigItem> rowData, int index)
        {

        }

        public void UpdateRow(IList<ConfigItem> rowData, int index)
        {

        }

        public void UpdateFieldItem(ConfigItem item, int index, int column)
        {

        }
        public void RemoveAt(int index)
        {

        }

        public void Clear()
        {

        }
    }
}
