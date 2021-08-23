using UnityEngine;
using System.Collections.Generic;

namespace Orbox.Utils
{
    public static class MonoExtensions 
    {
        private static List<Component> ComponentsCache = new List<Component>();

        public static T GetComponentAs<T>(this GameObject go) where T : class
        {
            if (go == null) return null;

            go.GetComponents(typeof(T), ComponentsCache);


            if (ComponentsCache.Count == 0 && go.transform.parent != null)
            {
                return GetComponentAs<T>(go.transform.parent.gameObject);
            }
            
            if(ComponentsCache.Count > 0)
            {
                return ComponentsCache[0] as T;
            }

            return default(T);

        }

        public static T MakeComponent<T>(Transform parent = null) where T : Component
        {
            var obj = new GameObject();
            obj.name = typeof(T).Name;

            if (parent != null)
                obj.transform.SetParent(parent);

            var component = obj.AddComponent<T>();
            return component;
        }

        public static T AddChild<T>(this Component parent) where T : Component
        {
            var child = MakeComponent<T>();
            child.transform.parent = parent.transform;

            return child;
        }

        static public T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponentAs<T>();

            if (comp == null)
                comp = go.AddComponent<T>();

            return comp;
        }

    }
}