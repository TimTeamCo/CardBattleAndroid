using System;
using TTBattle.UI;
using UnityEngine;

public class PlayerMenagerScript : MonoBehaviour
{
    [SerializeField] public ArmyPanel PlayerSelector; 
    [SerializeField] public ArmyPanel PlayerInferior;

    public void ChangePlayersRoles()
        {
            (PlayerSelector, PlayerInferior) = (PlayerInferior, PlayerSelector);
        }
}