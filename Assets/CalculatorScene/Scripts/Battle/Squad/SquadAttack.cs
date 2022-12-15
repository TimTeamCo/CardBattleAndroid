using System.Collections;
using Army;
using UnityEngine;
using TTBattle.UI;

namespace TTBattle
{
    public class SquadAttack : MonoBehaviour
    {
        public void Attack(ArmyPanel army1, ArmyPanel army2, TurnsNumerator turnsNumerator)
        {
            PlayerData.PlayerData playerAttacker = army1.playerData;
            PlayerData.PlayerData playerDefender = army2.playerData;
            PlayerSquad attacker = playerAttacker.playerArmy.Squads[army1.UnitDropdown.Value];
            PlayerSquad defender = playerDefender.playerArmy.Squads[army2.UnitDropdown.Value];

            var defUnitType = ChooseDefender(defender);
            switch (attacker.SquadUnit.UnitType)
            {
                case UnitType.Warrior:
                    WarriorLogicBattle(turnsNumerator, defUnitType, defender, playerDefender, attacker,
                        playerAttacker, army2);
                    break;
                case UnitType.Steamer:
                    SteamerLogicBattle(defUnitType, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
                case UnitType.Mage:
                    MageLogicBattle(defUnitType, turnsNumerator, defender, playerDefender, attacker,
                        playerAttacker, army2);
                    break;
            }
        }

        private void WarriorLogicBattle(TurnsNumerator turnsNumerator, UnitType defUnitType, PlayerSquad defender,
            PlayerData.PlayerData playerDefender, PlayerSquad attacker, PlayerData.PlayerData playerAttacker, ArmyPanel armyDefender)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, attacker,  
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior));
                    CalculateDefenderCount(attacker, defender,  
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior),
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior));
                    break;
                case UnitType.Steamer:
                    CalculateDefenderCount(defender, attacker,  
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Steamer), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior));
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, attacker,  
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Mage), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior));
                    StartCoroutine(MageAttack(defUnitType, attacker, defender,
                        playerDefender, playerAttacker, turnsNumerator, armyDefender));
                    break;
            }
        }

        private void SteamerLogicBattle(UnitType defUnitType, PlayerSquad defender,
            PlayerData.PlayerData playerDefender, PlayerSquad attacker, PlayerData.PlayerData playerAttacker)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, attacker,  
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer),
                        attacker.SquadUnit.WeakCoeficient);
                    break;
                case UnitType.Steamer:
                    CalculateDefenderCount(defender, attacker,  
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Steamer), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer));
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, attacker, 
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Mage), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer),
                        attacker.SquadUnit.StrongCoefitient);
                    break;
            }
        }


        private void MageLogicBattle(UnitType defUnitType, TurnsNumerator turnsNumerator, PlayerSquad defender,
            PlayerData.PlayerData playerDefender, PlayerSquad attacker, PlayerData.PlayerData playerAttacker, ArmyPanel armyDefender)
        {
            StartCoroutine(MageAttack(defUnitType, attacker, defender,
                playerDefender, playerAttacker, turnsNumerator, armyDefender));
        }

        private void CalculateDefenderCount(PlayerSquad defender, PlayerSquad attacker, int unitsInfluenceDefender, int unitsInfluenceAttacker, float coefficient = 1)
        {
            unitsInfluenceDefender = (unitsInfluenceDefender / 100) + 1;
            unitsInfluenceAttacker = (unitsInfluenceAttacker / 100) + 1;
            defender.Count = (int) ((defender.SquadUnit.Health * unitsInfluenceDefender *
                                     defender.Count -
                                     attacker.SquadUnit.Attack * unitsInfluenceAttacker *
                                     attacker.Count) * coefficient /
                                    (defender.SquadUnit.Health * unitsInfluenceDefender));
        }

        private UnitType ChooseDefender(PlayerSquad defender)
        {
            return defender.SquadUnit.UnitType;
        }

        private IEnumerator MageAttack(UnitType defUnitType, PlayerSquad attacker, PlayerSquad defender,
            PlayerData.PlayerData playerDefender, PlayerData.PlayerData playerAttacker, TurnsNumerator turnsNumerator, ArmyPanel armyDefender)
        {
            int numerator;
            var mapZone = playerDefender.MapZone;
            numerator = turnsNumerator.MoveCount;
            while (turnsNumerator.MoveCount != numerator + 1) yield return null;
            if (playerDefender.MapZone != mapZone) yield break;
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, attacker, 
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage),
                        attacker.SquadUnit.StrongCoefitient);
                    yield return defender.Count;
                    break;
                case UnitType.Steamer:
                    CalculateDefenderCount(defender, attacker, 
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Steamer), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage),
                        attacker.SquadUnit.WeakCoeficient);
                    yield return defender.Count;
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, attacker, 
                        (int) playerDefender.MapZone.GetUnitInfluence(UnitType.Mage), 
                        (int) playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage));
                    yield return defender.Count;
                    break;
            }
            
            armyDefender.SetTextOfUnitsAmount();
        }
    }
}