using UnityEngine;
using UnityEngine.UI;

public class AnimatorButtonView : MonoBehaviour
{
    [SerializeField] protected Button _button;
    [SerializeField] private Animator _animator;
    
    public void AnimateOnClick()
    {
        _animator.SetTrigger("Pressed");
    }
}