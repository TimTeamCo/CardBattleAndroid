using DG.Tweening;
using UnityEngine;
using UnityEngine.SearchService;
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

    public void SetEmotion(HelperEmotionsEnum emotionsEnum)
    {
        if (_tween != null)
        {
            _tween.Kill();
        }

        _helperEmotion.sprite = _helperEmotions.GetEmotion(emotionsEnum);
        
        _tween = DOVirtual.DelayedCall(3f,
            delegate {_helperEmotion.sprite = _helperEmotions.GetEmotion(HelperEmotionsEnum.IDLE);});
    }
}