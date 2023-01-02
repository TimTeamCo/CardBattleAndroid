using DG.Tweening;
using UnityEngine;

public class StartButtonView : MonoBehaviour
{
    [SerializeField] private GameObject _bottomCircle;
    [SerializeField] private GameObject _middleCircle;
    [SerializeField] private GameObject _upCircle;
    
    public void AnimateOnClick()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(_upCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_middleCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_bottomCircle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f))
            .Append(_upCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_middleCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .Join(_bottomCircle.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f))
            .AppendCallback(() =>
            {
                Debug.Log("StartAnimate Search or not");
            });
    }
}
