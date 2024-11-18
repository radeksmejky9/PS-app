using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject LoadingOverlay;
    public Slider ProgressBar;
    private void OnEnable()
    {
        ContentLoader.OnDownloadProgressChanged += ProgressLoad;
        ContentLoader.OnDownloadStart += OnDownloadStart;
        ContentLoader.OnDownloadEnd += OnDownloadEnd;
    }
    private void OnDisable()
    {
        ContentLoader.OnDownloadProgressChanged -= ProgressLoad;
        ContentLoader.OnDownloadStart -= OnDownloadStart;
        ContentLoader.OnDownloadEnd -= OnDownloadEnd;
    }
    private void ProgressLoad(float progressValue)
    {
        ProgressBar.value = progressValue;
    }

    private void OnDownloadStart()
    {
        LoadingOverlay.SetActive(true);
    }
    private void OnDownloadEnd()
    {
        LoadingOverlay.SetActive(false);
    }

}
