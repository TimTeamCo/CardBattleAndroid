using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HelperView : MonoBehaviour
{
    [SerializeField] private Image _helperEmotion;
    [SerializeField] private HelperEmotionSprites _helperEmotions;
    [SerializeField] private float _animationDuration = 3.0f;
    private Tween _tween;

    private void Start()
    {
        _helperEmotion.sprite = _helperEmotions.GetEmotion(HelperEmotionsEnum.IDLE);
    }

    public async UniTaskVoid SetNewEmotion(HelperEmotionsEnum emotionType)
    {
        _helperEmotion.sprite = _helperEmotions.GetEmotion(emotionType);
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        _helperEmotion.sprite = _helperEmotions.GetEmotion(HelperEmotionsEnum.IDLE);
    }
}