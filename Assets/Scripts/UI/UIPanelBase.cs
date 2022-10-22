using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TTUI
{
    // Basic UI element that can be shown or hidden.
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanelBase : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> m_onVisibilityChange;
        
        // Otherwise, when this Shows/Hides, the children won't know to update their own visibility.
        private List<UIPanelBase> m_uiPanelsInChildren = new List<UIPanelBase>();
        private bool showing;
        private CanvasGroup m_canvasGroup;
        protected CanvasGroup MyCanvasGroup
        {
            get
            {
                if (m_canvasGroup != null)
                {
                    return m_canvasGroup;
                }
                return m_canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public virtual void Start()
        {
            // Note that this won't detect children in GameObjects added during gameplay, if there were any.
            var children = GetComponentsInChildren<UIPanelBase>(true); 
            foreach (var child in children)
            {
                if (child != this)
                {
                    m_uiPanelsInChildren.Add(child);
                }
            }
        }
        
        public void Toggle()
        {
            if (showing)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        
        public void Show()
        {
            Show(true);
        }
        
        public void Show(bool propagateToChildren)
        {
            MyCanvasGroup.alpha = 1;
            MyCanvasGroup.interactable = true;
            MyCanvasGroup.blocksRaycasts = true;
            showing = true;
            m_onVisibilityChange?.Invoke(true);
            if (propagateToChildren == false)
            {
                return;
            }

            foreach (UIPanelBase child in m_uiPanelsInChildren)
            {
                child.m_onVisibilityChange?.Invoke(true);
            }
        }
        
        // Called by some serialized events, so we can't just have targetAlpha as an optional parameter.
        public void Hide()
        {
            Hide(0);
        }

        public void Hide(float targetAlpha)
        {
            MyCanvasGroup.alpha = targetAlpha;
            MyCanvasGroup.interactable = false;
            MyCanvasGroup.blocksRaycasts = false;
            showing = false;
            m_onVisibilityChange?.Invoke(false);
            foreach (UIPanelBase child in m_uiPanelsInChildren)
            {
                child.m_onVisibilityChange?.Invoke(false);
            }
        }
    }
}