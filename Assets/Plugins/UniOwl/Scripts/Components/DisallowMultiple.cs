using JetBrains.Annotations;
using System;
using System.Diagnostics;

namespace UniOwl.Components
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [BaseTypeRequired(typeof(ScriptableComponent))]
    [Conditional("UNITY_EDITOR")]
    public class DisallowMultiple : Attribute
    {
        
    }
}