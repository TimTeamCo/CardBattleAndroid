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
            PlayerDataSO.PlayerDataSO playerAttacker = army1.playerDataSo;
            PlayerDataSO.PlayerDataSO playerDefender = army2.playerDataSo;
            PlayerSquad attacker = playerAttacker.playerArmy.Squads[army1.UnitDropdown.Value];
            PlayerSquad defender = playerDefender.playerArmy.Squads[army2.UnitDropdown.Value];

            var defUnitType = ChooseDefender(defender);
            switch (attacker.SquadUnit.UnitType)
            {
                case UnitType.Warrior:
                    WarriorLogicBattle(turnsNumerator, defUnitType, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
                case UnitType.Steamer:
                    SteamerLogicBattle(defUnitType, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
                case UnitType.Mage:
                    MageLogicBattle(defUnitType, turnsNumerator, defender, playerDefender, attacker,
                        playerAttacker, army2, army1);
                    break;
            }
        }

        private void WarriorLogicBattle(TurnsNumerator turnsNumerator, UnitType defUnitType, PlayerSquad defender,
            PlayerDataSO.PlayerDataSO playerDefender, PlayerSquad attacker, PlayerDataSO.PlayerDataSO playerAttacker)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, attacker,  
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior));
                    CalculateDefenderCount(attacker, defender,  
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior),
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior));
                    break;
                case UnitType.Steamer:
                    CalculateDefenderCount(defender, attacker,  
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Steamer), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior));
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, attacker,  
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Mage), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Warrior));
                    break;
            }
        }

        private void SteamerLogicBattle(UnitType defUnitType, PlayerSquad defender,
            PlayerDataSO.PlayerDataSO playerDefender, PlayerSquad attacker, PlayerDataSO.PlayerDataSO playerAttacker)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, attacker,  
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer),
                        attacker.SquadUnit.WeakCoeficient);
                    CalculateDefenderCount(attacker, defender,  
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer),
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior));
                    break;
                case UnitType.Steamer:
                    CalculateDefenderCount(defender, attacker,  
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Steamer), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer));
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, attacker, 
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Mage), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Steamer),
                        attacker.SquadUnit.StrongCoefitient);
                    break;
            }
        }


        private void MageLogicBattle(UnitType defUnitType, TurnsNumerator turnsNumerator, PlayerSquad defender,
            PlayerDataSO.PlayerDataSO playerDefender, PlayerSquad attacker, PlayerDataSO.PlayerDataSO playerAttacker, ArmyPanel armyDefender, ArmyPanel armyAttacker)
        {
            StartCoroutine(MageAttack(defUnitType, attacker, defender,
                playerDefender, playerAttacker, turnsNumerator, armyDefender, armyAttacker));
        }

        private void CalculateDefenderCount(PlayerSquad defender, PlayerSquad attacker, float unitsInfluenceDefender, float unitsInfluenceAttacker, float coefficient = 1)
        {
            unitsInfluenceDefender = (unitsInfluenceDefender / 100) + 1;
            unitsInfluenceAttacker = (unitsInfluenceAttacker / 100) + 1;
            var one = defender.SquadUnit.Health * unitsInfluenceDefender * defender.Count;
            var two = attacker.SquadUnit.Attack * unitsInfluenceAttacker * attacker.Count;
            var three = one - two * coefficient;
            
            defender.Count = (int) (three / defender.SquadUnit.Health * unitsInfluenceDefender);
        }

        private UnitType ChooseDefender(PlayerSquad defender)
        {
            return defender.SquadUnit.UnitType;
        }

        private IEnumerator MageAttack(UnitType defUnitType, PlayerSquad attacker, PlayerSquad defender,
            PlayerDataSO.PlayerDataSO playerDefender, PlayerDataSO.PlayerDataSO playerAttacker, TurnsNumerator turnsNumerator, ArmyPanel armyDefender, ArmyPanel armyAttacker)
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
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage),
                        attacker.SquadUnit.StrongCoefitient);
                    CalculateDefenderCount( attacker, defender,
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage),
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Warrior));
                    yield return (defender.Count, attacker.Count);
                    armyAttacker.SetTextOfUnitsAmount();
                    break;
                case UnitType.Steamer:
                    CalculateDefenderCount(defender, attacker, 
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Steamer), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage),
                        attacker.SquadUnit.WeakCoeficient);
                    yield return defender.Count;
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, attacker, 
                        playerDefender.MapZone.GetUnitInfluence(UnitType.Mage), 
                        playerAttacker.MapZone.GetUnitInfluence(UnitType.Mage));
                    yield return defender.Count;
                    break;
            }
            
            armyDefender.SetTextOfUnitsAmount();
        }
    }
}