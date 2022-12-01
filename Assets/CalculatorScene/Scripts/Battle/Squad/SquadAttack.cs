﻿using System.Collections;
using UnityEngine;
using TTBattle.UI;

namespace TTBattle
{
    public class SquadAttack : MonoBehaviour
    {
        public void Attack(ArmyPanel army1, ArmyPanel army2, TurnsNumerator turnsNumerator)
        {
            Player playerAttacker = army1.Player;
            Player playerDefender = army2.Player;
            Squad attacker = playerAttacker.PlayerHand.GetUnitChoice(army1.UnitDropdown.value);
            Squad defender = playerDefender.PlayerHand.GetUnitChoice(army2.UnitDropdown.value);

            var defUnitType = ChooseDefender(defender);
            switch (attacker._unit.UnitType)
            {
                case UnitType.Warrior:
                    WarriorLogicBattle(turnsNumerator, defUnitType, defender, playerDefender, attacker,
                        playerAttacker, army2);
                    break;
                case UnitType.Assasin:
                    AssasinLogicBattle(defUnitType, defender, playerDefender, attacker,
                        playerAttacker);
                    break;
                case UnitType.Mage:
                    MageLogicBattle(defUnitType, turnsNumerator, defender, playerDefender, attacker,
                        playerAttacker, army2);
                    break;
            }
        }

        private void WarriorLogicBattle(TurnsNumerator turnsNumerator, UnitType defUnitType, Squad defender,
            Player playerDefender, Squad attacker, Player playerAttacker, ArmyPanel armyDeffender)
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
                    StartCoroutine(MageAttack(defUnitType, attacker, defender, playerAttacker.UnitsInfluence[2],
                        playerDefender, playerAttacker, turnsNumerator, armyDeffender));
                    break;
            }
        }

        private void AssasinLogicBattle(UnitType defUnitType, Squad defender,
            Player playerDefender, Squad attacker, Player playerAttacker)
        {
            switch (defUnitType)
            {
                case UnitType.Warrior:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 0, 1,
                        attacker._unit.WeakCoeficient);
                    break;
                case UnitType.Assasin:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 1, 1);
                    break;
                case UnitType.Mage:
                    CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 2, 1,
                        attacker._unit.StrongCoefitient);
                    break;
            }
        }


        private void MageLogicBattle(UnitType defUnitType, TurnsNumerator turnsNumerator, Squad defender,
            Player playerDefender, Squad attacker, Player playerAttacker, ArmyPanel armyDeffender)
        {
            StartCoroutine(MageAttack(defUnitType, attacker, defender, playerAttacker.UnitsInfluence[2],
                playerDefender, playerAttacker, turnsNumerator, armyDeffender));
        }

        private void CalculateDefenderCount(Squad defender, Player playerDefender, Squad attacker,
            Player playerAttacker, int unitsInfluenceDefender, int unitsInfluenceAttacker, float coefficient = 1)
        {
            defender.Count = (int) ((defender._unit.Health * playerDefender.UnitsInfluence[unitsInfluenceDefender] *
                                     defender.Count -
                                     attacker._unit.Attack * playerAttacker.UnitsInfluence[unitsInfluenceAttacker] *
                                     attacker.Count) * coefficient /
                                    (defender._unit.Health * playerDefender.UnitsInfluence[unitsInfluenceDefender]));
        }

        private UnitType ChooseDefender(Squad defender)
        {
            return defender._unit.UnitType;
        }

        private IEnumerator MageAttack(UnitType defUnitType, Squad attacker, Squad defender, float mageInfluence,
            Player playerDefender, Player playerAttacker, TurnsNumerator turnsNumerator, ArmyPanel armyDeffender)
        {
            int numerator;
            var tempCell = playerDefender.PlayerMapCell;
            numerator = turnsNumerator.MoveCount;
            while (turnsNumerator.MoveCount != numerator + 1) yield return null;
            if (playerDefender.PlayerMapCell == tempCell)
            {
                switch (defUnitType)
                {
                    case UnitType.Warrior:
                        CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 0, 2,
                            attacker._unit.StrongCoefitient);
                        yield return defender.Count;
                        break;
                    case UnitType.Assasin:
                        CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 1, 2,
                            attacker._unit.WeakCoeficient);
                        yield return defender.Count;
                        break;
                    case UnitType.Mage:
                        CalculateDefenderCount(defender, playerDefender, attacker, playerAttacker, 2, 2);
                        yield return defender.Count;
                        break;
                }

                armyDeffender.SetTextOfUnitsAmount();
            }
        }
    }
}