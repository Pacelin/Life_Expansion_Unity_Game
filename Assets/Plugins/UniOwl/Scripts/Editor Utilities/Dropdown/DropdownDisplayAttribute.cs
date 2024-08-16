using System;

namespace UniOwl
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DropdownDisplayAttribute : Attribute
    {
        public string value { get; set; }

        public DropdownDisplayAttribute(string value)
        {
            this.value = value;
        }
    }
}
