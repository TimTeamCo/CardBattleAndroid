using UnityEngine;

public class AnimatorButtonView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public void AnimateOnClick()
    {
        _animator.SetTrigger("Pressed");
    }
}