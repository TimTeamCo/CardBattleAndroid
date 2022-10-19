using System;
using System.Collections;
using System.Collections.Generic;
using TTBattle.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle
{
    public class SquadAttack: MonoBehaviour
    {
        public void Attack(Squad attacker, Squad deffender, TurnNumeratorButton _turnNumeratorButton)
        {
            if (attacker._unit is Warrior)
            {
                if (deffender._unit is Warrior)
                {
                    deffender.Count = ((deffender._unit.Health * deffender.Count - (attacker._unit.Attack *  attacker.Count)) /
                                        deffender._unit.Health);
                    attacker.Count = ((attacker._unit.Health *  attacker.Count - (deffender._unit.Attack * deffender.Count)) / attacker._unit.Health);
                }

                if (deffender._unit is Assasin)
                {
                    deffender.Count = ((deffender._unit.Health * deffender.Count - (attacker._unit.Attack *  attacker.Count)) /
                                       deffender._unit.Health);
                }
                if (deffender._unit is Mage)
                {
                    deffender.Count = ((deffender._unit.Health * deffender.Count - (attacker._unit.Attack *  attacker.Count)) /
                                       deffender._unit.Health);
                    StartCoroutine(MageAttack(attacker, deffender,  attacker.Count));
                }
            }

            if (attacker._unit is Assasin)
            {
                if (deffender._unit is Warrior)
                {
                    deffender.Count =
                        (int) Math.Floor(
                            ((deffender._unit.Health * deffender.Count -
                              (attacker._unit.Attack *  attacker.Count) * attacker._unit.WeakCoeficient) / deffender._unit.Health));
                }

                if (deffender._unit is Assasin)
                {
                    deffender.Count = ((deffender._unit.Health * deffender.Count - (attacker._unit.Attack *  attacker.Count)) /
                                       deffender._unit.Health);
                }

                if (deffender._unit is Mage)
                {
                    deffender.Count =
                        (int) Math.Floor(
                            ((deffender._unit.Health * deffender.Count -
                              (attacker._unit.Attack *  attacker.Count) * attacker._unit.StrongCoefitient) / deffender._unit.Health));
                }
            }

            if (attacker._unit is Mage)
            {
                StartCoroutine(MageAttack(attacker, deffender,  attacker.Count));
            }
            
            IEnumerator MageAttack(Squad _attacker, Squad deffender, int _attackCount)
            {
                Squad attacker = _attacker;
                int _numerator;
                _numerator = _turnNumeratorButton.attackNumerator;
                while (_turnNumeratorButton.attackNumerator!=_numerator+1 )
                {
                    yield return null;
                }
                if (deffender._unit is Warrior)
                {
                    yield return deffender.Count =
                        (int) Math.Floor(
                            ((deffender._unit.Health * deffender.Count -
                              (attacker._unit.Attack * _attackCount) * attacker._unit.StrongCoefitient) / deffender._unit.Health));
                    yield break;
                }

                if (deffender._unit is Assasin)
                {
                    yield return deffender.Count =
                        (int) Math.Floor(
                            ((deffender._unit.Health * deffender.Count -
                              (attacker._unit.Attack * _attackCount) * attacker._unit.WeakCoeficient) / deffender._unit.Health));
                    yield break;
                }

                if (deffender._unit is Mage)
                {
                    yield return deffender.Count = (((deffender._unit.Health * deffender.Count - (attacker._unit.Attack * _attackCount)) /
                                                     deffender._unit.Health));
                    yield break;
                }
            }
        }
        
    }
}