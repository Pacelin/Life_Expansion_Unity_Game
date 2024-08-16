using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Editor
{
    public static class TypeUtils
    {
        private static readonly Dictionary<string, Type> nameToTypeMap = new();

        static TypeUtils()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var type in assembly.GetTypes())
                {
                    if (nameToTypeMap.ContainsKey(type.FullName))
                        continue;
                    nameToTypeMap.Add(type.FullName, type);
                }
        }

        public static Type GetTypeFromFullName(string name)
        {
            return nameToTypeMap[name];
        }

        public static FieldInfo GetFieldFromPropertyPath(this Type type, string propertyPath)
        {
            Type parentType = type;
            FieldInfo field = type.GetField(propertyPath);

            string[] perDot = propertyPath.Split('.');
            foreach (string fieldName in perDot)
            {
                field = parentType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field != null)
                    parentType = field.FieldType;
                else
                    return null;
            }
            return field;
        }

        public static Type GetFieldType(this SerializedProperty property)
        {
            var type = GetTypeFromFullName(property.managedReferenceFieldTypename.Split(" ")[1]);
            return type;

            /*var parentType = property.serializedObject.targetObject.GetType();
            var field = parentType.GetFieldFromPropertyPath(GetPropertyPath(property));
            return field.FieldType;*/
        }

        public static object GetValue(this SerializedProperty property)
        {
            var parentType = property.serializedObject.targetObject.GetType();
            var field = parentType.GetFieldFromPropertyPath(GetPropertyPath(property));
            return field.GetValue(property.serializedObject.targetObject);
        }

        public static void SetValue(this SerializedProperty property, object value)
        {
            var parentType = property.serializedObject.targetObject.GetType();
            var field = parentType.GetFieldFromPropertyPath(GetPropertyPath(property));
            field.SetValue(property.serializedObject.targetObject, value);
        }

        private static string GetPropertyPath(SerializedProperty property)
        {
            return property.propertyPath;
        }
    }
}
