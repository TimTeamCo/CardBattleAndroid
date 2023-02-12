using System;
using Logic.Infrastructure;

namespace Logic.Game
{
    [Serializable]
    public class LocalPlayer
    {
        public CallbackValue<string> DisplayName = new CallbackValue<string>("");
    }
}