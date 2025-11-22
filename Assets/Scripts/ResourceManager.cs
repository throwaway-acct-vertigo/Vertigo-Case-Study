using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DefaultNamespace
{
    public static class ResourceManager
    {
        public static AsyncOperationHandle<IList<T>> LoadAssets<T>(string key, Action<T> callback = null)
        {
            return Addressables.LoadAssetsAsync(key, callback, true);
        }
    }
}