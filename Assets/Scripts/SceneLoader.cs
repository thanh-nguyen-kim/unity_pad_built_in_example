using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName = "", packName = "";
    public DownloadPanel downloadPanelPrefab;
    private Button btn;
    private IEnumerator HandleLoadAsync()
    {
        bool isCached = false;
        yield return null;
        if (packName != null && packName.Length > 0)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var asyncTask = AndroidAssetPacks.GetAssetPackStateAsync(new string[] { packName });
            yield return asyncTask;
            for (int i = 0; i < asyncTask.states.Length; i++)
                Debug.Log(asyncTask.states[i].name + " " + asyncTask.states[i].status.ToString());
            isCached = asyncTask.states[0].status == UnityEngine.Android.AndroidAssetPackStatus.Completed;
#else
            isCached = true;
#endif
        }
        else
            isCached = true;
        if (!isCached)
            downloadPanelPrefab.Spawn(sceneName, packName, LoadScene);
        else
            LoadScene();
    }
    public void LoadScene()
    {
        Addressables.LoadSceneAsync(sceneName);
    }
    public void OnClick()
    {
        StartCoroutine(HandleLoadAsync());
    }
}
