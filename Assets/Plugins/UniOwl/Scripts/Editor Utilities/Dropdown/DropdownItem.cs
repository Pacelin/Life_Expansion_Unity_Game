using System;
using UnityEngine;

namespace UniOwl
{
    [Serializable]
    public class DropdownItem<T>
    {
        [SerializeReference, Dropdown]
        public T item;
    }
}
