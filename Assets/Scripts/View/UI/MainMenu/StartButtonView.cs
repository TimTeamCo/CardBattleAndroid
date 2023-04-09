    using System.Collections.Generic;
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
    [SerializeField] AudioClip clickSFX;
    [SerializeField] AudioClip searchSFX;

    private bool isSelected;
    private List<Sequence> _sequences = new ();
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
        Sequence _readySequence = DOTween.Sequence();
        _readySequence.Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Join(_ready.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_ready.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .SetLoops(-1);
        _sequences.Add(_readySequence);
    }

    private void OnClickReadyButton()
    {
        ApplicationController.Instance.AudioController.SetSFX(clickSFX);
        StopAnimation();
        ApplicationController.Instance.GameManager.SetLocalUserStatus(PlayerStatus.Ready);
    }

    private void OnClickSearchButton()
    {
        AnimateOnClick();
    }

    private void AnimateOnClick()
    {
        if (isSelected)
        {
            StopAnimation();
        }
        
        ApplicationController.Instance.AudioController.SetSFX(clickSFX);
        Sequence pressedSequence = DOTween.Sequence();
        _sequences.Add(pressedSequence);
        pressedSequence.Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .AppendCallback(() =>
            {
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
            });
    }

    private void StopAnimation()
    {
        if (_sequences.Count == 0)
        {
            return;
        }
        else
        {
            foreach (var sequence in _sequences)
            {
                sequence.Kill();
                _sequences.Remove(sequence);
            }
        }
        ResetButton();
    }

    private void ResetButton()
    {
        _middleCircle.transform.localEulerAngles = Vector3.zero;
        _bottomCircle.transform.localEulerAngles = Vector3.zero;
        _upCircle.transform.localEulerAngles = Vector3.zero;
        ApplicationController.Instance.AudioController.RemoveClip(clickSFX);
        ApplicationController.Instance.AudioController.RemoveClip(searchSFX);
    }

    private void StartSearchAnimation()
    {
        Sequence _searchSequence = DOTween.Sequence();
        _sequences.Add(_searchSequence);
        _searchSequence
            .Append(_middleCircle.transform.DORotate(new Vector3(0, 0,
                _middleCircle.transform.localRotation.z + 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_bottomCircle.transform.DOLocalRotate(new Vector3(0, 0, 
                _bottomCircle.transform.localRotation.z - 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_upCircle.transform.DOLocalRotate(new Vector3(0, 0,
                _upCircle.transform.localRotation.z - 15f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Incremental);
        ApplicationController.Instance.AudioController.SetSFX(searchSFX, true, true);
    }
}