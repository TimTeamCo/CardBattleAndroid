using Logic.Connection;
using UnityEngine;
using UnityEngine.UI;

public class ViewPopup : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    private ConnectionManager _connectionManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        gameObject.SetActive(false);
        _connectionManager = (ConnectionManager) ApplicationController.Instance.ConnectionManager;
        _connectionManager.HaveInternet += HaveInternet;
        _connectionManager.NoInternet += NoInternet;
        _retryButton.onClick.AddListener(RetryButton);
    }

    private void NoInternet()
    {
        if (gameObject.activeInHierarchy)
        {
            return;
        }
        
        gameObject.SetActive(true);
    }

    private void HaveInternet()
    {
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        
        gameObject.SetActive(false);
    }

    private void RetryButton()
    {
        ApplicationController.Instance.ConnectionManager.HardCheckInternetConnection();
    }

    private void OnDestroy()
    {
        _connectionManager.HaveInternet -= HaveInternet;
        _connectionManager.NoInternet -= NoInternet;
    }
}
