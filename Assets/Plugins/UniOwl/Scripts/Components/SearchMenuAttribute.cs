using System;
using System.Diagnostics;

namespace UniOwl.Components
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    public class SearchMenuAttribute : Attribute
    {
        public string Path { get; }
        public string Name { get; }

        public SearchMenuAttribute(string name)
        {
            Name = name;
        }
        
        public SearchMenuAttribute(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }
}