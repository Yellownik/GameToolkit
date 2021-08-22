using UnityEngine;
using System;

namespace Orbox.Utils
{

    public interface IResourceManager 
    {

        UnityEngine.Object GetAsset<TEnum>(TEnum resource) 
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        GameObject GetPrefab<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable;


        AudioClip LoadAudioClip<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        AnimationClip LoadAnimationClip<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        
        GameObject CreatePrefabInstance<TEnum>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        TResult CreatePrefabInstance<TEnum, TResult>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
            where TResult : class;


        //Use this methods to avoid heap memory allocation and reduce prefab creation time.
        //You have to disable GameObject to make it available from pool.

        GameObject GetFromPool<TEnum>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        TResult GetFromPool<TEnum, TResult>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
            where TResult : class;

        //Warm up the cache

        void WarmAll<TEnum>(Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        void Warm<TEnum>(TEnum resource, Transform parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable;

        
        //Release prefab assets 

        void Release<TEnum>(TEnum resource)
            where TEnum : struct, IComparable, IConvertible, IFormattable;


        // Release all items from cache and pool
        void ClearAll();

        // Release items from pool only
        void ClearPool();


    }
}