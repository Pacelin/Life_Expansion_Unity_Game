using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawer
    {
        private const string NullChoiceDisplayName = "None";

        private SerializedProperty property;

        private Type[] types;

        private List<string> choices = new();
        private GUIContent[] choiceOptions;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;

            UpdateChoices();

            var oldIndex = GetDropDownIndex();

            var popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var newIndex = EditorGUI.Popup(popupRect, label, oldIndex, choiceOptions);
            
            if (oldIndex != newIndex)
                SetChoiceValue(choices[newIndex]);
            
            EditorGUI.indentLevel++;
            DrawProperty(position, property, label);
            EditorGUI.indentLevel--;
        }
        
        private void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null)
                return;

            float height = EditorGUI.GetPropertyHeight(property, label, true);
            
            var iter = property.Copy();
            var end = property.Copy();
            end.NextVisible(false);
            
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            while (iter.NextVisible(true) && iter.propertyPath != end.propertyPath)
            {
                if (iter.depth - property.depth >= 2)
                    continue;
                
                Rect newRect = new Rect(rect.x, rect.y + height + EditorGUIUtility.standardVerticalSpacing, rect.width, EditorGUI.GetPropertyHeight(iter, label, true));
                EditorGUI.PropertyField(newRect, iter, true);
                height += newRect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            
            var iter = property.Copy();
            var end = property.Copy();
            end.NextVisible(false);

            while (iter.NextVisible(true) && iter.propertyPath != end.propertyPath)
            {
                if (iter.depth - property.depth >= 2)
                    continue;
                totalHeight += EditorGUI.GetPropertyHeight(iter, label, true) +
                               EditorGUIUtility.standardVerticalSpacing;
            }

            return totalHeight;
        }

        private void UpdateTypes(SerializedProperty property, bool includeSelf)
        {
            types ??= DropdownChoiceMapper.GetDerivedTypes(property.GetFieldType(), includeSelf);
        }

        private void UpdateChoices()
        {
            var dropdownAttribute = (DropdownAttribute)attribute;

            UpdateTypes(property, dropdownAttribute.IncludeSelf);
            
            choices.Clear();

            for (int i = 0; i < types.Length; i++)
            {
                string name = TypeToDisplayName(types[i]);
                choices.Add(name);
                DropdownChoiceMapper.AddChoice(ChoiceNameToPath(name), types[i]);
            }
            if (dropdownAttribute.IncludeNone)
                choices.Add(NullChoiceDisplayName);

            if (choiceOptions != null) return;
            
            choiceOptions = new GUIContent[choices.Count];
            for (int i = 0; i < choiceOptions.Length; i++)
                choiceOptions[i] = new GUIContent(choices[i]);
        }

        private int GetDropDownIndex()
        {
            var choice = GetDropdownChoice();
            return choices.IndexOf(choice);
        }
        
        private string GetDropdownChoice()
        {
            var value = property.managedReferenceValue;
            if (value == null)
                return NullChoiceDisplayName;

            foreach (var choice in choices)
            {
                if (choice.Equals(NullChoiceDisplayName))
                    continue;
                var path = ChoiceNameToPath(choice);
                
                var type = DropdownChoiceMapper.GetChoiceType(path);

                
                if (value.GetType() == type)
                    return choice;
            }

            Debug.LogWarning(value.GetType());
            return NullChoiceDisplayName;
        }

        private void SetChoiceValue(string choice)
        {
            object value;

            if (choice.Equals(NullChoiceDisplayName))
                value = null;
            else
            {
                var path = ChoiceNameToPath(choice);
                Type type = DropdownChoiceMapper.GetChoiceType(path);
                value = Activator.CreateInstance(type);
            }

            property.managedReferenceValue = value;
            property.serializedObject.ApplyModifiedProperties();
        }

        private string ChoiceNameToPath(string name)
        {
            return $"{property.propertyPath}.{name}";
        }

        private string TypeToDisplayName(Type type)
        {
            var attr = type.GetCustomAttributes(typeof(DropdownDisplayAttribute), true);
            if (attr.Length > 0)
                return ((DropdownDisplayAttribute)attr[0]).value;
            return type.Name;
        }
    }
}
