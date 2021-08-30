using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


namespace Orbox.Utils
{
    //Parameter 'resource' is the prefab name. You should put prefabs to 'Resources' folder. T must be enum type. 
    //Usage example: enum Cars{ Audi, Ferrari}; var car = ResourceManager.GetPrefab<Cars>(Cars.Audi);

    public class ResourceManager : IResourceManager
    {
        private class PoolItem
        {
            public Type EnumType;
            public int EnumValue;

            public GameObject GameObject;
            public System.Object Component;
        }

        private List<PoolItem> Pool = new List<PoolItem>();
        private Dictionary<EnumComparerKey, UnityEngine.Object> ResourceCache;

        public ResourceManager()
        {
            ResourceCache = new Dictionary<EnumComparerKey, UnityEngine.Object>(new EnumEqualityComparer());
        }


        public UnityEngine.Object GetAsset<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var asset = GetAsset(type, value, name);
            return asset;
        }

        public GameObject GetPrefab<TEnum>(TEnum resource) 
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var go = (GameObject)GetAsset(type, value, name);
            return go;
        }

        public GameObject CreatePrefabInstance<TEnum>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var go = Instantiate(type, value, name, parent);
            return go;
        }


        public TResult CreatePrefabInstance<TEnum, TResult>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
            where TResult : class
        {
            var type = typeof(TEnum);
            var value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var go = Instantiate(type, value, name, parent);
            var component = go.GetComponent(typeof(TResult)) as TResult;
            return (TResult)component;
        }

        //Use this method to avoid heap memory allocation.
        //You have to disable GameObject to make it available from pool.

        public GameObject GetFromPool<TEnum>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            int value = EnumInt32ToInt.Convert(resource);

            for (int i = 0; i < Pool.Count; i++)
            {
                var item = Pool[i];

                if (item.EnumType == type && item.EnumValue == value && item.GameObject.activeSelf == false)
                {
                    item.GameObject.transform.parent = null;
                    item.GameObject.SetActive(true);
                    return item.GameObject;
                }
            }

            //create new Item
            var poolItem = new PoolItem();
            var name = resource.ToString();

            poolItem.EnumType = type;
            poolItem.EnumValue = value;
            poolItem.GameObject = Instantiate(type, value, name, parent);

            Pool.Add(poolItem);

            return poolItem.GameObject;
        }

        public TResult GetFromPool<TEnum, TResult>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
            where TResult : class
        {
            var type = typeof(TEnum);
            int value = EnumInt32ToInt.Convert(resource);

            for (int i = 0; i < Pool.Count; i++)
            {
                var item = Pool[i];

                if (item.Component != null && item.EnumType == type && item.EnumValue == value && item.GameObject.activeSelf == false)
                {
                    item.GameObject.SetActive(true);
                    return (TResult)item.Component;
                }
            }

            var poolItem = new PoolItem();
            var name = resource.ToString();

            poolItem.EnumType = type;
            poolItem.EnumValue = value;
            poolItem.GameObject = Instantiate(type, value, name, parent);
            poolItem.Component = poolItem.GameObject.GetComponent(typeof(TResult));

            Pool.Add(poolItem);

            return (TResult)poolItem.Component;
        }

        //load form fbx
        public AnimationClip LoadAnimationClip<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            int value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var key = new EnumComparerKey(type, value);

            if (ResourceCache.ContainsKey(key) == false)
            {
                var path = GetPathFromNamespace(type, name);
                var asset = Resources.Load<GameObject>(path);

                if (asset == null)
                    throw new UnityException("Can't load resource '" + name + "'");

                ResourceCache[key] = asset.GetComponent<Animation>().clip;
            }

            return ResourceCache[key] as AnimationClip;
        }


        public AudioClip LoadAudioClip<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            int value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var clip = (AudioClip)GetAsset(type, value, name);
            return clip;
        }

        public void Warm<TEnum>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            int value = EnumInt32ToInt.Convert(resource);
            var name = resource.ToString();

            var gameObject = GetFromPool(type, value, name, parent);
            gameObject.SetActive(false);
        }

        public void WarmAll<TEnum>(Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);

            foreach (TEnum prefab in Enum.GetValues(type))
            {
                int value = EnumInt32ToInt.Convert(prefab);
                var name = prefab.ToString();

                var gameObject = GetFromPool(type, value, name, parent);
                gameObject.SetActive(false);
            }
        }

        public void Release<TEnum>(TEnum resource) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(TEnum);
            int value = EnumInt32ToInt.Convert(resource);
            var key = new EnumComparerKey(type, value);

            ResourceCache.Remove(key);

            for (int i = Pool.Count - 1; i >= 0; i--)
            {
                var item = Pool[i];

                if (item.EnumType == type && item.EnumValue == value)
                {
                    Pool.RemoveAt(i);
                    break;
                }
            }
        }

        public void ClearAll()
        {
            Pool.Clear();
            ResourceCache.Clear();
        }

        public void ClearPool()
        {
            Pool.Clear();
        }

        // --- private ---

        private static string GetPathFromNamespace(Type type, string name)
        {
            string path = "";
            if (type.Namespace != null)
            {
                path = type.Namespace.Replace('.', '/') + @"/";
            }
            path += type.Name + @"/" + name;
            return path;
        }

        private UnityEngine.Object GetAsset(Type type, int value, string name)
        {
            var key = new EnumComparerKey(type, value);

            if (ResourceCache.ContainsKey(key) == false)
            {
                var path = GetPathFromNamespace(type, name);
                var asset = Resources.Load<UnityEngine.Object>(path);

                if (asset == null)
                    throw new UnityException("Can't load resource '" + path + "'");

                ResourceCache[key] = asset;
            }

            return ResourceCache[key];
        }

        private GameObject Instantiate(Type type, int value, string name, Transform parent)
        {
            var template = GetAsset(type, value, name);
            var instance = GameObject.Instantiate(template) as GameObject;
            instance.SetActive(true);
            instance.transform.SetParent(parent);

            return instance;
        }

        private GameObject GetFromPool(Type type, int value, string name, Transform parent)
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                var item = Pool[i];

                if (item.EnumType == type && item.EnumValue == value && item.GameObject.activeSelf == false)
                {
                    item.GameObject.SetActive(true);
                    return item.GameObject;
                }
            }

            var poolItem = new PoolItem();

            poolItem.EnumType = type;
            poolItem.EnumValue = value;
            poolItem.GameObject = Instantiate(type, value, name, parent);

            Pool.Add(poolItem);

            return poolItem.GameObject;
        }


    }
}