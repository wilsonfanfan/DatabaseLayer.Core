using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{

    /// <summary>
    /// A pure string dictionary structure that conforms to DatabaseLayer
    /// </summary>
    /// <remarks>wilson add</remarks>
    public class DatabaseLayerDictionary : Dictionary<string, object>
    {
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss:fff";

        public DatabaseLayerDictionary() { }

        public DatabaseLayerDictionary(IDictionary<string, object> dictionary)
            : base(dictionary) { }
     
        /// <summary>
        /// Add a new key-value pair.Key-value pairs with null keys or null values ​​will be ignored.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"> The value corresponding to the key, currently supported：string, int, long, double, bool, DateTime类型</param>
        public new void Add(string key, object value)
        {
            string strValue;

            if (value == null)
            {
                strValue = null;
            }
            else if (value is string)
            {
                strValue = (string)value;
            }
            else if (value is Nullable<DateTime>)
            {
                Nullable<DateTime> dateTime = value as Nullable<DateTime>;
                strValue = dateTime.Value.ToString(DATE_TIME_FORMAT);
            }
            else if (value is Nullable<int>)
            {
                strValue = (value as Nullable<int>).Value.ToString();
            }
            else if (value is Nullable<long>)
            {
                strValue = (value as Nullable<long>).Value.ToString();
            }
            else if (value is Nullable<double>)
            {
                strValue = (value as Nullable<double>).Value.ToString();
            }
            else if (value is Nullable<bool>)
            {
                strValue = (value as Nullable<bool>).Value.ToString().ToLower();
            }
            else
            {
                strValue = value.ToString();
            }

            this.Add(key, strValue);
        }

        public void Add(string key, string value)
        {
            HashSet<object> hash = new HashSet<object>();
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                base.Add(key, value);
            }
        }
    }

}
