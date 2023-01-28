using DG.Tweening;
using TMPro;
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

    private void Start()
    {
        _button.onClick.AddListener(OnClickStartButton);
    }
    
    private void OnClickStartButton()
    {
        AnimateOnClick();
    }

    private void AnimateOnClick()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .PrependCallback(() =>
            {
                StopSearchAnimation();
                isSelected = !isSelected;
            })
            .Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .AppendCallback(StartSearchAnimation);
    }

    private void StopSearchAnimation()
    {
        if (_searchSequence == null)
        {
            return;
        }

        if (isSelected == false) return;
        
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
        _hamsterDialog.text = $"Bla - bla - bla - bla - bla - bla...";
    }

    private void StartSearchAnimation()
    {
        if (isSelected == false) return;
        _hamsterDialog.text = $"Searching...";
        ApplicationController.Instance.LobbyManager.QuickJoin(result =>
        {
            _hamsterDialog.text = $"{result}";
        });
        _searchSequence = DOTween.Sequence();
        _searchSequence.Append(_middleCircle.transform.DORotate(new Vector3(0, 0,
                _middleCircle.transform.localRotation.z + 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_bottomCircle.transform.DOLocalRotate(new Vector3(0, 0, 
                _bottomCircle.transform.localRotation.z - 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_upCircle.transform.DOLocalRotate(new Vector3(0, 0,
                _upCircle.transform.localRotation.z - 15f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Incremental);
    }
}