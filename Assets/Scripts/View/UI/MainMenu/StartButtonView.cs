using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _bottomCircle;
    [SerializeField] private GameObject _middleCircle;
    [SerializeField] private GameObject _upCircle;
    [SerializeField] private GameObject _triangle;
    [SerializeField] private GameObject _ready;
    private bool isSelected;
    private Sequence _sequence;
    private LocalLobby _localLobby;

    private void Start()
    {
        _button.onClick.AddListener(OnClickSearchButton);
        _localLobby = ApplicationController.Instance.GameManager.LocalLobby;
        _localLobby.onUserJoined += OnUserJoined;
        _triangle.SetActive(true);
        _ready.SetActive(false);
    }

    private void OnUserJoined(LocalPlayer obj)
    {
        if (_localLobby.PlayerCount != 2) return;
        StopAnimation();
        _button.onClick.RemoveListener(OnClickSearchButton);
        AnimateReadyBehaviour();
        _button.onClick.AddListener(OnClickReadyButton);
    }

    private void AnimateReadyBehaviour()
    {
        _triangle.SetActive(false);
        _ready.SetActive(true);
        _sequence = DOTween.Sequence();
        _sequence.Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Join(_ready.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_ready.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .SetLoops(-1);
    }

    private void OnClickReadyButton()
    {
        StopAnimation();
        ApplicationController.Instance.GameManager.SetLocalUserStatus(PlayerStatus.Ready);
        
    }

    private void OnClickSearchButton()
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
            StopAnimation();
            ApplicationController.Instance.GameManager.onExitSearchingButton.Invoke();
            isSelected = false;
        }
    }

    private void StopAnimation()
    {
        if (_sequence == null)
        {
            return;
        }

        _sequence.Kill();
        _sequence = null;
        ResetButton();
    }

    private void ResetButton()
    {
        _middleCircle.transform.localEulerAngles = Vector3.zero;
        _bottomCircle.transform.localEulerAngles = Vector3.zero;
        _upCircle.transform.localEulerAngles = Vector3.zero;
    }

    private void StartSearchAnimation()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(_middleCircle.transform.DORotate(new Vector3(0, 0,
                _middleCircle.transform.localRotation.z + 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_bottomCircle.transform.DOLocalRotate(new Vector3(0, 0, 
                _bottomCircle.transform.localRotation.z - 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_upCircle.transform.DOLocalRotate(new Vector3(0, 0,
                _upCircle.transform.localRotation.z - 15f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Incremental);
    }
}