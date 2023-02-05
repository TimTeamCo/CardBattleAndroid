using System;
using System.Threading.Tasks;
using UnityEngine;

namespace NetCodeTT.Lobby
{
    //Created to mimic the way rate limits are implemented Here:  https://docs.unity.com/lobby/rate-limits.html
    public class ServiceRateLimiter
    {
        public Action<bool> onCooldownChange;
        public readonly int coolDownMS;
        public bool TaskQueued { get; private set; } = false;

        readonly int m_ServiceCallTimes;
        bool m_CoolingDown = false;
        int m_TaskCounter;

        //(If you're still getting rate limit errors, try increasing the pingBuffer)
        public ServiceRateLimiter(int callTimes, float coolDown, int pingBuffer = 100)
        {
            m_ServiceCallTimes = callTimes;
            m_TaskCounter = m_ServiceCallTimes;
            coolDownMS =
                Mathf.CeilToInt(coolDown * 1000) +
                pingBuffer;
        }

        public async Task QueueUntilCooldown()
        {
            if (!m_CoolingDown)
            {
#pragma warning disable 4014
                ParallelCooldownAsync();
#pragma warning restore 4014
            }

            m_TaskCounter--;

            if (m_TaskCounter > 0)
            {
                return;
            }

            if (!TaskQueued)
                TaskQueued = true;
            else
                return;

            while (m_CoolingDown)
            {
                await Task.Delay(10);
            }
        }

        async Task ParallelCooldownAsync()
        {
            IsCoolingDown = true;
            await Task.Delay(coolDownMS);
            IsCoolingDown = false;
            TaskQueued = false;
            m_TaskCounter = m_ServiceCallTimes;
        }

        public bool IsCoolingDown
        {
            get => m_CoolingDown;
            private set
            {
                if (m_CoolingDown != value)
                {
                    m_CoolingDown = value;
                    onCooldownChange?.Invoke(m_CoolingDown);
                }
            }
        }
    }
}