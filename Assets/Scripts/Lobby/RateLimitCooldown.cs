using System;
using System.Collections.Generic;

namespace TTLobbyLogic
{
    public class RateLimitCooldown : Observed<RateLimitCooldown>
    {
        private float m_timeSinceLastCall = float.MaxValue;
        private readonly float m_cooldownTime;
        private Queue<Action> m_pendingOperations = new Queue<Action>();
        private bool m_isInCooldown = false;
        
        public bool IsInCooldown
        {
            get => m_isInCooldown;
            private set
            {
                if (m_isInCooldown != value)
                {
                    m_isInCooldown = value;
                    OnChanged(this);
                }
            }
        }
        
        public bool CanCall()
        {
            if (m_timeSinceLastCall < m_cooldownTime)
                return false;
            else
            {
                Locator.Get.UpdateSlow.Subscribe(OnUpdate, m_cooldownTime);
                m_timeSinceLastCall = 0;
                IsInCooldown = true;
                return true;
            }
        }
        
        public RateLimitCooldown(float cooldownTime)
        {
            m_cooldownTime = cooldownTime;
        }

        public void EnqueuePendingOperation(Action action)
        {
            m_pendingOperations.Enqueue(action);
        }
        
        private void OnUpdate(float dt)
        {
            m_timeSinceLastCall += dt;
            if (m_timeSinceLastCall >= m_cooldownTime)
            {
                IsInCooldown = false;
                if (!m_isInCooldown) // It's possible that by setting IsInCooldown, something called CanCall immediately, in which case we want to stay on UpdateSlow.
                {
                    Locator.Get.UpdateSlow.Unsubscribe(OnUpdate); // Note that this is after IsInCooldown is set, to prevent an Observer from kicking off CanCall again immediately.
                    int numPending = m_pendingOperations.Count; // It's possible a pending operation will re-enqueue itself or new operations, which should wait until the next loop.
                    for (; numPending > 0; numPending--)
                    {
                        m_pendingOperations.Dequeue()?.Invoke(); // Note: If this ends up enqueuing many operations, we might need to batch them and/or ensure they don't all execute at once.
                    }
                }
            }
        }
        
        public override void CopyObserved(RateLimitCooldown oldObserved)
        {
            /* This behavior isn't needed; we're just here for the OnChanged event management. */
        }
    }
}