using UnityEngine;
using UnityEngine.Events;

namespace TTInfrastructure
{
    /// MonoBehaviour that will automatically handle setting up to observe something.
    /// It also exposes an event so some other component can effectively observe it as well.
    public abstract class ObserverBehaviour<T> : MonoBehaviour where T : Observed<T>
    {
        public T observed { get; set; }
        public UnityEvent<T> OnObservedUpdated;

        private void Awake()
        {
            if (observed == null)
            {
                return;
            }
            BeginObserving(observed);
        }
        
        private void OnDestroy()
        {
            if (observed == null)
            {
                return;
            }
            EndObserving();
        }
        
        protected virtual void UpdateObserver(T obs)
        {
            observed = obs;
            OnObservedUpdated?.Invoke(observed);
        }
        
        public void BeginObserving(T target)
        {
            if (target == null)
            {
                Debug.LogError($"Needs a Target of type {typeof(T)} to begin observing.", gameObject);
                return;
            }
          
            UpdateObserver(target);
            observed.onChanged += UpdateObserver;
        }
        
        public void EndObserving()
        {
            if (observed == null)
                return;
            if (observed.onChanged != null)
                observed.onChanged -= UpdateObserver;
            observed = null;
        }
    }
}