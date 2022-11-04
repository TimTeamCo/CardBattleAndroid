using System.Collections;
using UnityEngine;
using TTBattle.UI;

namespace TTBattle
{
    //Why prefab lay near?
    public class SquadAttack : MonoBehaviour
    {
        public void Attack(ArmyPanel army1, ArmyPanel army2, TurnsNumerator turnsNumerator)
        {
            Player playerAttacker = army1._player;
            Player playerDefender = army2._player;
            Squad attacker = playerAttacker._playerHand.GetUnitChoice(army1._unitDropdown.value);
            Squad defender = playerDefender._playerHand.GetUnitChoice(army2._unitDropdown.value);

            var defUnitType = ChooseDefender(defender);
            switch (attacker._unit.UnitType)
            {
                case UnitType.Warrior:
                    WarriorLogicBattle(turnsNumerator, defUnitType, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
                case UnitType.Assasin:
                    AssasinLogicBattle(defUnitType, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
                case UnitType.Mage:
                    MageLogicBattle(defUnitType,turnsNumerator, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
            }
        }

        private void WarriorLogicBattle(TurnsNumerator turnsNumerator, UnitType defUnitType, Squad defender,
            Player playerDefender, Squad attacker, Player playerAttacker)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 0, 0);
                    CalculateDefenderCount(attacker, playerAttacker, defender, playerDefender, 0, 0);
                    break;
                case UnitType.Assasin:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 1, 0);
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 2, 0);
                    StartCoroutine(MageAttack(defUnitType, attacker, defender, playerAttacker._unitsInfluence[2], 
                        playerDefender, playerAttacker, turnsNumerator));
                    break;
            }
        }

        private void AssasinLogicBattle(UnitType defUnitType, Squad defender,
            Player playerDefender, Squad attacker, Player playerAttacker)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 0, 1, attacker._unit.WeakCoeficient);
                    break;
                case UnitType.Assasin:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 1, 1);
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 2, 1, attacker._unit.StrongCoefitient);
                    break;
            }
        }

        
        private void MageLogicBattle(UnitType defUnitType, TurnsNumerator turnsNumerator, Squad defender,
            Player playerDefender, Squad attacker, Player playerAttacker)
        {
            StartCoroutine(MageAttack(defUnitType, attacker, defender, playerAttacker._unitsInfluence[2],
                playerDefender, playerAttacker, turnsNumerator));
        }
        
        private void CalculateDefenderCount(Squad defender, Player playerDefender, Squad attacker,
            Player playerAttacker, int unitsInfluenceDefender, int unitsInfluenceAttacker, float coefficient = 1)
        {
            defender.Count = (int) ((defender._unit.Health * playerDefender._unitsInfluence[unitsInfluenceDefender] *
                                      defender.Count -
                                      attacker._unit.Attack * playerAttacker._unitsInfluence[unitsInfluenceAttacker] *
                                      attacker.Count) * coefficient /
                                     (defender._unit.Health * playerDefender._unitsInfluence[unitsInfluenceDefender]));
        }

        private UnitType ChooseDefender(Squad defender)
        {
            return defender._unit.UnitType;
        }

        private IEnumerator MageAttack(UnitType defUnitType, Squad attacker, Squad defender, float mageInfluence,
            Player playerDefender, Player playerAttacker, TurnsNumerator turnsNumerator)
        {
            int numerator;
            numerator = turnsNumerator.NumeratorValue;
            while (turnsNumerator.NumeratorValue != numerator + 1) yield return null;
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 0, 2, attacker._unit.StrongCoefitient);
                    yield return defender.Count;
                    break;
                case UnitType.Assasin:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 1, 2, attacker._unit.WeakCoeficient);
                    yield return defender.Count;
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 2, 2);
                    yield return defender.Count;
                    break;
            }
        }
    }
}