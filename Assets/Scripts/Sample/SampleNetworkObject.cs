using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Sample
{
    /// This holds the logic and data for an individual symbol, which can be "clicked" if the server detects the collision with a player who sends a click input.
    // public class SampleNetworkObject : NetworkBehaviour
    // {
    //     [SerializeField]  private SampleData sampleData;
    //     [SerializeField]  private SpriteRenderer spriteRenderer;
    //     [SerializeField]  private Animator animator;
    //     
    //     public bool Clicked { get; private set; }
    //     [HideInInspector] public NetworkVariable<int> sampleIndex; // The index into SampleData, not the index of this object.
    //     
    //     public override void OnNetworkSpawn()
    //     {
    //         sampleIndex.OnValueChanged += OnSampleIndexSet;
    //     }
    //
    //     /// Because of the need to distinguish host vs. client calls, we use the sampleIndex NetworkVariable to learn what icon to display.
    //     private void OnSampleIndexSet(int prevValue, int newValue)
    //     {
    //         spriteRenderer.sprite = sampleData.GetIconForIndex(sampleIndex.Value);
    //         sampleIndex.OnValueChanged -= OnSampleIndexSet;
    //     }
    //     
    //     public void SetPosition_Server(Vector3 newPosition)
    //     {
    //         SetPosition_ClientRpc(newPosition);
    //     }
    //     
    //     [ClientRpc]
    //     void SetPosition_ClientRpc(Vector3 newPosition)
    //     {
    //         transform.localPosition = newPosition;
    //     }
    //     
    //     
    //     [ServerRpc]
    //     public void ClickedSequence_ServerRpc(ulong clickerPlayerId)
    //     {
    //         Clicked = true;
    //         Clicked_ClientRpc(clickerPlayerId);
    //         StartCoroutine(HideIconAnimDelay());
    //     }
    //     
    //     [ClientRpc]
    //     public void Clicked_ClientRpc(ulong clickerPlayerId)
    //     {
    //         if (NetworkManager.LocalClientId == clickerPlayerId)
    //         {
    //             animator.SetTrigger("iClicked");
    //         }
    //         else
    //         {
    //             animator.SetTrigger("theyClicked");
    //         }
    //     }
    //     
    //     [ServerRpc]
    //     public void HideIcon_ServerRpc()
    //     {
    //         // Actually destroying the symbol objects can cause garbage collection and other delays that might lead to desyncs.
    //         // Disabling the networked object can also cause issues, so instead, just move the object, and it will be cleaned up once the NetworkManager is destroyed.
    //         // (If we used object pooling, this is where we would instead return it to the pool.)
    //         //The animation calls RemoveIcon(only for server
    //         transform.localPosition += Vector3.forward * 500;
    //     }
    //     
    //     //It's easier to have the post-animation icon "deletion" happen entirely in server world rather than depend on client-side animation triggers.
    //     IEnumerator HideIconAnimDelay()
    //     {
    //         yield return new WaitForSeconds(0.3f);
    //         HideIcon_ServerRpc();
    //     }
    // }
}