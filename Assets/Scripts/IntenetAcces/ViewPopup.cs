using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPopup : MonoBehaviour
{
    [SerializeField] private Initer _initer;
    [SerializeField] private Image _banner;
    [SerializeField] private Button _retryButton;

    void Start()
    {
        _retryButton.onClick.AddListener(RetryButton);
    }

    public void ShowPopup( bool value)
    {
        _banner.gameObject.SetActive(value);
        _retryButton.gameObject.SetActive(value);
    }

    private void RetryButton()
    {
        StopCoroutine(_initer.TestInternetConection());
        StartCoroutine(_initer.TestInternetConection());
    }
}
