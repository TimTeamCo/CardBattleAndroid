using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private SoundSFX _soundSfx;
    private Sequence _optionSequence;

    private void OnEnable()
    {
        _button.onClick.AddListener(()=>
        {
            OnClickOptionsButton();
            _soundSfx.PlaySFX();
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(()=>
        {
            OnClickOptionsButton();
            _soundSfx.PlaySFX();
        });
    }

    private void OnClickOptionsButton()
    {
        if (_optionSequence != null)
        {
            DOTween.Kill(_optionSequence);
        }
        
        _optionSequence = DOTween.Sequence();
        _optionSequence.Append(_button.transform.DOLocalRotate(
            new Vector3(_button.transform.localRotation.x, _button.transform.localRotation.y, -720f), 2,
            RotateMode.FastBeyond360));
    }
}
