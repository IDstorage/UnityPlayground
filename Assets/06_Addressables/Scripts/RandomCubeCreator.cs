namespace UP06
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class RandomCubeCreator : MonoBehaviour
    {
        [SerializeField]
        private string path;

        [SerializeField]
        private AssetReferenceGameObject target;

        public void Create()
        {
            // Instantiate object with loaded bundle.
            Addressables.InstantiateAsync(path).Completed += (obj) =>
            {
                if (obj.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError("Fail");
                    return;
                }

                obj.Result.GetComponent<RandomCubeInitializer>().SetRandom();
            };


            // Get size of bundle will be downloaded.
            //Addressables.GetDownloadSizeAsync(target.RuntimeKey).Completed += (size) => { Debug.Log($"Load size: {size.Result}"); };

            // Load bundle in cache.
            //Addressables.LoadAssetAsync<GameObject>(path).Completed += OnCubeLoaded;

            // void OnCubeLoaded(AsyncOperationHandle<GameObject> result)
            // {
            //     if (result.Status != AsyncOperationStatus.Succeeded)
            //     {
            //         Debug.LogError("Fail");
            //         return;
            //     }

            //     // Instantiate object with loaded bundle.
            //     Addressables.InstantiateAsync(path).Completed += (obj) =>
            //     {
            //         obj.Result.GetComponent<RandomCubeInitializer>().SetRandom();
            //     };
            // }
        }
    }
}