using System;
using System.Collections.Generic;

namespace UniOwl.Editor
{
    public static class DropdownChoiceMapper
    {
        private static readonly Dictionary<Type, Type[]> typeMap = new();

        private static readonly Dictionary<string, Type> choiceToTypeMap = new();

        public static Type[] GetDerivedTypes(Type baseType, bool includeSelf)
        {
            if (typeMap.TryGetValue(baseType, out var types))
                return types;

            var newTypes = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var type in assembly.GetTypes())
                    if (baseType.IsAssignableFrom(type))
                    {
                        if (baseType == type)
                        {
                            if (!includeSelf || type.IsAbstract || type.IsInterface)
                                continue;
                        }
                        newTypes.Add(type);
                    }

            typeMap.Add(baseType, newTypes.ToArray());
            return typeMap[baseType];
        }

        public static void AddChoice(string choicePath, Type choiceType)
        {
            choiceToTypeMap.TryAdd(choicePath, choiceType);
        }

        public static Type GetChoiceType(string choicePath)
        {
            if (choiceToTypeMap.TryGetValue(choicePath, out Type type))
                return type;
            return null;
        }
    }
}
