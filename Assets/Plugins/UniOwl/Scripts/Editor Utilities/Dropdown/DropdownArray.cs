using System;
using System.Linq;

namespace UniOwl
{
    [Serializable]
    public class DropdownArray<T>
    {
        public DropdownItem<T>[] items;

        public T[] ToArray()
        {
            return items.Select(item => item.item).ToArray();
        }
        
        public static implicit operator T[](DropdownArray<T> array)
        {
            return array.ToArray();
        }
    }
}
