using RankOne.Interfaces;
using System;
using System.Collections.Generic;

namespace RankOne.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private Dictionary<string, object> _cachedItems;

        public CacheHelper()
        {
            _cachedItems = new Dictionary<string, object>();
        }

        public bool Exists(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return _cachedItems.ContainsKey(key);
        }

        public object GetValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return _cachedItems[key];
        }

        public void SetValue(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (Exists(key))
            {
                _cachedItems[key] = value;
            }
            else
            {
                _cachedItems.Add(key, value);
            }
        }
    }
}