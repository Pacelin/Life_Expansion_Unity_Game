using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniOwl.Components.Editor
{
    public class ScriptableComponentSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private ScriptableComponentListEditor _editor;
        
        private Texture2D indentationIcon;

        public event Action<Type> componentTypeChosen; 
        
        public void Initialize(ScriptableComponentListEditor editor)
        {
            _editor = editor;
            
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public void Open(VisualElement element)
        {
            Rect bounds = GUIUtility.GUIToScreenRect(element.worldBound);
            Vector2 position = new((bounds.xMin + bounds.xMax) / 2f, bounds.yMax + 20f);
            
            SearchWindow.Open(new SearchWindowContext(position), this);
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTree = new();
            searchTree.Add(new SearchTreeGroupEntry(new GUIContent("Components"), 0));
            HashSet<string> groups = new();
            
            // Obj : ComponentList<T>, Get T.
            var componentBaseType = _editor.target.GetType().BaseType!.GetGenericArguments()[0];
            var types = GetTypes(componentBaseType);

            foreach ((string path, Type type) in types)
            {
                var splitPath = path.Split("/");
                string groupName = string.Empty;

                for (int i = 0; i < splitPath.Length - 1; i++)
                {
                    groupName += splitPath[i];

                    if (!groups.Contains(groupName))
                    {
                        searchTree.Add(new SearchTreeGroupEntry(new GUIContent(splitPath[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }

                var iconAttr = type.GetCustomAttribute<IconAttribute>();
                var icon = iconAttr?.path != null ? AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/UniOwl/Editor Default Resources/T_Icon.png") : indentationIcon;
                
                var entry = new SearchTreeEntry(new GUIContent(splitPath[^1], icon))
                {
                    level = splitPath.Length,
                    userData = type,
                };
                searchTree.Add(entry);
            }
            
            return searchTree;
        }

        private IEnumerable<(string, Type)> GetTypes(Type baseType)
        {
            var types = TypeCache.GetTypesDerivedFrom(baseType);
            return types.
                   Where(type =>
                   {
                       // No abstracts
                       if (type.IsAbstract)
                           return false;

                       // If has DisallowMultiple, skip if already present 
                       if (type.GetCustomAttributes(typeof(DisallowMultiple), true).Length > 0)
                           return _editor.targetComponentList.Components == null || _editor.targetComponentList.Components.FirstOrDefault(component => component.GetType() == type) == null;

                       return true;
                   }).
                   Select(type =>
                   {
                       var path = ScriptableComponentUtils.GetSearchPath(type); 
                       var name = ScriptableComponentUtils.GetDisplayName(type); 
                       return (path + "/" + name, type);
                   }).
                   OrderBy(tuple => tuple.Item1);
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Type type = (Type)searchTreeEntry.userData;
            componentTypeChosen?.Invoke(type);
            return true;
        }
    }
}