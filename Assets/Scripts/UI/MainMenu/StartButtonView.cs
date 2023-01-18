using System;
using DG.Tweening;
using UnityEngine;

public class StartButtonView : MonoBehaviour
{
    [SerializeField] private GameObject _bottomCircle;
    [SerializeField] private GameObject _middleCircle;
    [SerializeField] private GameObject _upCircle;
    [SerializeField] private AudioSource _startButtonAudioSource;
    [SerializeField] private AudioClip _startMatchEffect;
    [SerializeField] private AudioClip _searchingLoopEffect;
    [SerializeField] private AudioClip _foundMatchEffect;

    private bool isSelected;
    private Sequence _searchSequence;

    public void AnimateOnClick()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .PrependCallback(() =>
            {
                if (isSelected)
                {
                    StopSearchAnimation();
                }
                isSelected = !isSelected;
            })
            .Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .AppendCallback(()=> {
                _startButtonAudioSource.clip = _searchingLoopEffect;
                _startButtonAudioSource.loop = isSelected;
                _startButtonAudioSource.Play();
                StartSearchAnimation();
                });
        _startButtonAudioSource.clip = _startMatchEffect;
        _startButtonAudioSource.Play();
    }

    private void StopSearchAnimation()
    {
        if (_searchSequence == null)
        {
            return;
        }

        if (isSelected)
        {
            _searchSequence.Kill();
            ResetButton();
        }

        //Circle.Stop();
        //FoundMatch.Play();
    }
    
    private void ResetButton()
    {
        _middleCircle.transform.localEulerAngles = Vector3.zero;
        _bottomCircle.transform.localEulerAngles = Vector3.zero;
        _upCircle.transform.localEulerAngles = Vector3.zero;
    }

    private void StartSearchAnimation()
    {
        if (isSelected == false) return;
        _searchSequence = DOTween.Sequence();
        _searchSequence.Append(_middleCircle.transform.DORotate(new Vector3(0, 0,
                _middleCircle.transform.localRotation.z + 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_bottomCircle.transform.DOLocalRotate(new Vector3(0, 0, 
                _bottomCircle.transform.localRotation.z - 25f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .Append(_upCircle.transform.DOLocalRotate(new Vector3(0, 0,
                _upCircle.transform.localRotation.z - 15f), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear))
            .SetLoops(Int32.MaxValue, LoopType.Incremental);
        //Circle.Play();
    }
}