using System;
using System.Collections.Generic;
using UniOwl.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniOwl.Components.Editor
{
    [CustomEditor(typeof(ScriptableComponentList<>), editorForChildClasses:true)]
    public class ScriptableComponentListEditor : UnityEditor.Editor
    {
        private static readonly string[] ExcludeFields = { "m_Script", "_components", "_root", "_prefab", };
        
        private ScriptableComponentSearchWindow _searchWindow;
        
        [SerializeField]
        private StyleSheet _celestialStyleSheet;

        private VisualElement _propertiesContainer;
        private VisualElement _componentsContainer;

        private SerializedProperty m_components;
        protected List<ScriptableComponentEditor> componentEditors = new();
        
        public ScriptableComponentList targetComponentList => (ScriptableComponentList)target;

        public event Action listOrChildUpdated;
        
        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected virtual void Awake()
        {
            _searchWindow = CreateInstance<ScriptableComponentSearchWindow>();
            _searchWindow.Initialize(this);
            _searchWindow.componentTypeChosen += AddComponent;
        }

        protected virtual void OnEnable()
        {
            m_components = serializedObject.FindProperty("_components");
        }

        private void RefreshEditors()
        {
            foreach (ScriptableComponentEditor editor in componentEditors)
                DestroyImmediate(editor, true);
            componentEditors.Clear();

            int arraySize = m_components.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                var componentProp = m_components.GetArrayElementAtIndex(i);
                var component = (ScriptableComponent)componentProp.objectReferenceValue;
                CreateComponentEditor(component);
            }
        }
        
        private ScriptableComponentEditor CreateComponentEditor(ScriptableComponent component, int index = -1)
        {
            var editor = (ScriptableComponentEditor)CreateEditor(component);
            editor.Init(this);
            
            if (index == -1)
                componentEditors.Add(editor);
            else
                componentEditors[index] = editor;

            editor.componentDestroyed += RemoveComponent;
            editor.componentActiveStateChanged += EnableStateChanged;
            editor.componentStateChanged += so => ComponentChanged((ScriptableComponent)so.targetObject);

            return editor;
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();
            
            RefreshEditors();
            
            // Add root
            VisualElement root = new();
            root.styleSheets.Add(_celestialStyleSheet);
            root.AddToClassList("custom-editor");

            _propertiesContainer = new VisualElement();
            _propertiesContainer.AddToClassList("properties-container");
            root.Add(_propertiesContainer);
            
            _componentsContainer = new VisualElement();
            root.Add(_componentsContainer);
            
            // Add properties
            foreach (var property in serializedObject.GetChildren(ExcludeFields))
            {
                var propField = new PropertyField(property);
                propField.BindProperty(property);
                _propertiesContainer.Add(propField);
            }
            
            // Add foldouts
            var size = m_components.arraySize;
            for (int i = 0; i < size; i++)
            {
                var foldout = componentEditors[i].CreateInspectorGUI();
                _componentsContainer.Add(foldout);
            }
            
            // Add "Add" Button
            var addButtonContainer = new VisualElement();
            addButtonContainer.AddToClassList("add-button-container");
            root.Add(addButtonContainer);
            
            var addButton = new Button
            {
                text = "Add...",
            };
            
            addButton.clicked += () => _searchWindow.Open(addButton);
            addButtonContainer.Add(addButton);
            
            return root;
        }

        /// <summary>
        /// Add component to last element of the list.
        /// </summary>
        /// <param name="type"></param>
        protected virtual void AddComponent(Type type)
        {
            serializedObject.Update();
            
            var lastElementIndex = m_components.arraySize;
            m_components.InsertArrayElementAtIndex(lastElementIndex);

            var component = CreateComponent(type, targetComponentList);
            component.Initialize(targetComponentList);
            new SerializedObject(component).ApplyModifiedProperties();

            if (EditorUtility.IsPersistent(serializedObject.targetObject))
                AssetDatabase.AddObjectToAsset(component, serializedObject.targetObject);
            
            var componentProp = m_components.GetArrayElementAtIndex(lastElementIndex);
            componentProp.objectReferenceValue = component;
            
            serializedObject.ApplyModifiedProperties();

            var editor = CreateComponentEditor(component);
            editor.OnComponentCreate();
            var foldout = editor.CreateInspectorGUI();
            _componentsContainer.Add(foldout);
            
            if (EditorUtility.IsPersistent(serializedObject.targetObject))
            {
                EditorUtility.SetDirty(serializedObject.targetObject);
                AssetDatabase.SaveAssets();
            }
        }

        protected virtual ScriptableComponent CreateComponent(Type type, ScriptableComponentList list)
        {
            ScriptableComponent component = (ScriptableComponent)CreateInstance(type);
            component.name = ScriptableComponentUtils.GetDisplayName(component.GetType());
            component.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            
            return component;
        }

        protected virtual int RemoveComponent(ScriptableComponent component)
        {
            var index = Array.IndexOf(targetComponentList.Components, component);
            
            if (EditorUtility.IsPersistent(targetComponentList))
                AssetDatabase.RemoveObjectFromAsset(component);
            
            DestroyImmediate(component);
            m_components.DeleteArrayElementAtIndex(index);
            
            componentEditors[index].OnComponentDestroy();
            DestroyImmediate(componentEditors[index]);
            componentEditors.RemoveAt(index);
            
            if (EditorUtility.IsPersistent(targetComponentList))
                AssetDatabase.SaveAssets();

            serializedObject.ApplyModifiedProperties();

            return index;
        }
        
        protected virtual void EnableStateChanged(ScriptableComponent component, bool value)
        {
            
        }

        protected virtual void ListChanged(SerializedObject _)
        {
            listOrChildUpdated?.Invoke();
        }
        
        protected virtual void ComponentChanged(ScriptableComponent component)
        {
            listOrChildUpdated?.Invoke();
        }
    }
}