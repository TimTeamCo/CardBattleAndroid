using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _rectTransform;
    private Sequence _optionSequence;

    private void Start()
    {
        _button.onClick.AddListener(OnClickOptionsButton);
    }
    
    private void OnClickOptionsButton()
    {
        if (_optionSequence.IsPlaying())
        {
            return;
        }

        var position = _rectTransform.anchoredPosition;
        _optionSequence = DOTween.Sequence();
        _optionSequence.Append(_button.transform.DOLocalRotate(
            new Vector3(_button.transform.localRotation.x, _button.transform.localRotation.y, -720f), 2,
            RotateMode.FastBeyond360))
            .Join(DOTween.To(() => _rectTransform.anchoredPosition, x => _rectTransform.anchoredPosition = x, new Vector2(_rectTransform.anchoredPosition.x + 200, _rectTransform.anchoredPosition.y), 2))
            .AppendCallback(() =>
            {
                _rectTransform.anchoredPosition = position;
            });
    }
}