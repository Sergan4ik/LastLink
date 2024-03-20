using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class TestDownloadVRM : MonoBehaviour
    {
        private async void Start()
        {
            var bt = await LoadVRMBytes("https://runtimevrmimporttest.blob.core.windows.net/test/testav.vrm");
        }

        public static readonly string VRM_CACHE_PATH = $"CacheInternal/VRM/";
        public async Task<byte[]> LoadVRMBytes(string url)
        {
            var urlMod = url.Replace(".", "_").Replace(":","_").Replace("/","_");
            var path = $"{VRM_CACHE_PATH}{urlMod}.vrm";
            if (System.IO.File.Exists(path))
            {
                Debug.Log($"VRM load from cached");
                var bytes1 = await System.IO.File.ReadAllBytesAsync(path);
                return bytes1;
            }
            
            /*
            var bytes = await File.ReadAllBytesAsync(url);
            var file = File.Open(path, FileMode.Create, FileAccess.Write);
            await file.DisposeAsync();
            await File.WriteAllBytesAsync(path, bytes);
            return bytes;
            */

            // download from url
            UnityWebRequest req = UnityWebRequest.Get(url);
            DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
            req.downloadHandler = dh;
            await req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download VRM from {url}");
                return null;
            }
            return req.downloadHandler.data;
        }
    }
}