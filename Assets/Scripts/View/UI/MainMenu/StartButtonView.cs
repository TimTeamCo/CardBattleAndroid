using DG.Tweening;
using NetCodeTT.Lobby;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _bottomCircle;
    [SerializeField] private GameObject _middleCircle;
    [SerializeField] private GameObject _upCircle;
    [SerializeField] private TextMeshProUGUI _hamsterDialog;
    private bool isSelected;
    private Sequence _searchSequence;
    private LobbyManager _lobbyManager;

    private void Start()
    {
        _button.onClick.AddListener(OnClickStartButton);
        _lobbyManager = (LobbyManager) ApplicationController.Instance.LobbyManager;
    }
    
    private void OnClickStartButton()
    {
        AnimateOnClick();
    }

    private void AnimateOnClick()
    {
        Sequence pressedSequence = DOTween.Sequence();
        pressedSequence.Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f));

        if (isSelected == false)
        {
            StartSearchAnimation();
            ApplicationController.Instance.GameManager.onPressStartButton.Invoke();
            isSelected = true;
        }
        else
        {
            StopSearchAnimation();
            ApplicationController.Instance.GameManager.onExitSearchingButton.Invoke();
            isSelected = false;
        }
    }

    private void StopSearchAnimation()
    {
        if (_searchSequence == null)
        {
            return;
        }

        _searchSequence.Kill();
        _searchSequence = null;
        ResetButton();
    }

    private void ResetButton()
    {
        _middleCircle.transform.localEulerAngles = Vector3.zero;
        _bottomCircle.transform.localEulerAngles = Vector3.zero;
        _upCircle.transform.localEulerAngles = Vector3.zero;
        ApplicationController.Instance.LobbyManager.LeaveLobby();
    }

    private void StartSearchAnimation()
    {
        ApplicationController.Instance.LobbyManager.QuickJoin();
        _searchSequence = DOTween.Sequence();
        _searchSequence.Append(_middleCircle.transform.DORotate(new Vector3(0, 0,
                _middleCircle.transform.localRotation.z + 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_bottomCircle.transform.DOLocalRotate(new Vector3(0, 0, 
                _bottomCircle.transform.localRotation.z - 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_upCircle.transform.DOLocalRotate(new Vector3(0, 0,
                _upCircle.transform.localRotation.z - 15f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Incremental);
    }

    public async void ShowLobbyPlayers()
    {
         await _lobbyManager.GetLobby(_lobbyManager._lobbyID, lobby =>
         {
             if (lobby == null)
             {
                 Debug.LogError($"Lobby is null");
                 return;
             }

             DataObject playerDataObject;
             DataObject opponentDataObject;
             
             if (_lobbyManager.isClient)
             {
                 playerDataObject = lobby.Data["ClientData"];
                 opponentDataObject = lobby.Data["HostData"];
             }
             else
             {
                 playerDataObject = lobby.Data["HostData"];
                 opponentDataObject = lobby.Data["ClientData"];
             }
             
             Debug.LogWarning($"Player Nick {playerDataObject.Value} -- Opponent Nick {opponentDataObject.Value}");
         });
    }
}