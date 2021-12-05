using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string  sceneName = "";
    public DownloadPanel downloadPanelPrefab;
    private Button btn;
    private IEnumerator HandleLoadAsync()
    {
        bool isCached = false;
        var asyncTask = Addressables.GetDownloadSizeAsync(sceneName);
        while (!asyncTask.IsDone) yield return null;
        isCached = (asyncTask.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && asyncTask.Result == 0);
        if (!isCached)
            downloadPanelPrefab.Spawn(sceneName, LoadScene);
        else
            LoadScene();
    }
    public void LoadScene()
    {
         StartCoroutine(HandleLoadAsync());
    }
    public void OnClick()
    {
        Addressables.LoadSceneAsync(sceneName);
    }
}
