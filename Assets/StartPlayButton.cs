using TTGame;
using UnityEngine;

public class StartPlayButton : MonoBehaviour
{
    public void ToJoinOrCreate()
    {
        Locator.Get.Messenger.OnReceiveMessage(MessageType.QuickJoin, GameState.Searching);
    }
}
