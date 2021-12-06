using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class DownloadPanel : MonoBehaviour
{
    public string assetLabel, packName;
    public UnityEngine.Events.UnityAction callback;
    [SerializeField] private Text percentDownload;
    [SerializeField] private GameObject continueBtn, cancelBtn;
    private AsyncOperationHandle downloadAsyncTask;
    public void Spawn(string assetLabel, string packname, UnityEngine.Events.UnityAction callback)
    {
        GameObject go = Instantiate(gameObject);
        go.GetComponent<DownloadPanel>().assetLabel = assetLabel;
        go.GetComponent<DownloadPanel>().callback = callback;
        go.GetComponent<DownloadPanel>().packName = packName;
    }
    IEnumerator Start()
    {
        bool isCached = false;
        // var asyncTask = Addressables.GetDownloadSizeAsync(assetLabel);
        // while (!asyncTask.IsDone) yield return null;
        // isCached = (asyncTask.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && asyncTask.Result == 0);
        var asyncTask = UnityEngine.Android.AndroidAssetPacks.GetAssetPackStateAsync(new string[] { packName });
        yield return asyncTask;
        isCached = asyncTask.states[0].status == UnityEngine.Android.AndroidAssetPackStatus.Completed;
        continueBtn.SetActive(isCached);
        cancelBtn.SetActive(!isCached);
        if (!isCached)
        {
            percentDownload.text = "0%";
            downloadAsyncTask = Addressables.DownloadDependenciesAsync(assetLabel);
            while (!downloadAsyncTask.IsDone)
            {
                percentDownload.text = (int)(downloadAsyncTask.PercentComplete * 100) + "%";
                yield return null;
            }
            percentDownload.text = "100%";
            continueBtn.SetActive(true);
            cancelBtn.SetActive(false);
        }
    }
    public void OnClickContinue()
    {
        Destroy(gameObject);
        callback?.Invoke();
    }
    public void OnClickCancel()
    {
        Destroy(gameObject);
    }
}
