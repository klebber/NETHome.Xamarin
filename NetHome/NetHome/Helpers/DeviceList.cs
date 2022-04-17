using NetHome.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NetHome.Helpers
{
    public class DeviceList<T> : List<T> where T : DeviceModel
    {
        private static readonly object _locker = new();
        public new void Add(T t)
        {
            lock (_locker)
            {
                base.Add(t);
            }
        }

        public new void Remove(T t)
        {
            lock (_locker)
            {
                base.Remove(t);
            }
        }

        public void Update(T t)
        {
            int index = FindIndex(d => d.Id == t.Id);
            if (index is not -1)
            {
                lock (_locker)
                {
                    this[index] = t;
                }
            }
        }
    }
}
