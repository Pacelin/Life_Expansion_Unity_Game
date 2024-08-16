using UnityEngine;

namespace UniOwl
{
    public class DropdownAttribute : PropertyAttribute
    {
        public bool IncludeSelf { get; }
        public bool IncludeNone { get; }

        public DropdownAttribute(bool includeSelf = false, bool includeNone = true)
        {
            IncludeSelf = includeSelf;
            IncludeNone = includeNone;
        }
    }
}
