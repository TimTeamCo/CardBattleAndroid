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
        public void Attack(Squad attacker, Squad deffender, TurnNumeratorButton _turnNumeratorButton, Player _playerAttacker, Player _playerDeffender)
        {
            if (attacker._unit is Warrior)
            {
                if (deffender._unit is Warrior)
                {
                    deffender.Count =  (int) Math.Floor(((deffender._unit.Health * _playerDeffender._unitsInfluence[0] * deffender.Count - (attacker._unit.Attack * _playerAttacker._unitsInfluence[0] *  attacker.Count)) / deffender._unit.Health * _playerDeffender._unitsInfluence[0]));
                    Debug.Log($"{deffender.Count} {attacker.Count}");
                    attacker.Count =  (int) Math.Floor(((attacker._unit.Health * _playerAttacker._unitsInfluence[0] * attacker.Count - (deffender._unit.Attack * _playerDeffender._unitsInfluence[0] * deffender.Count)) / attacker._unit.Health * _playerAttacker._unitsInfluence[0]));
                }

                if (deffender._unit is Assasin)
                {
                    deffender.Count = (int) Math.Floor((deffender._unit.Health * _playerDeffender._unitsInfluence[1] * deffender.Count - (attacker._unit.Attack * _playerAttacker._unitsInfluence[0]*  attacker.Count)) /
                                       deffender._unit.Health * _playerDeffender._unitsInfluence[1]);
                }
                if (deffender._unit is Mage)
                {
                    deffender.Count = (int) Math.Floor((deffender._unit.Health *_playerDeffender._unitsInfluence[2] * deffender.Count - (attacker._unit.Attack * _playerAttacker._unitsInfluence[0] *  attacker.Count)) /
                                       deffender._unit.Health * _playerDeffender._unitsInfluence[2]);
                    StartCoroutine(MageAttack(attacker, deffender,  attacker.Count, _playerDeffender._unitsInfluence[2], _playerDeffender));
                }
            }

            if (attacker._unit is Assasin)
            {
                if (deffender._unit is Warrior)
                {
                    deffender.Count =
                        (int) Math.Floor(
                            ((deffender._unit.Health * _playerDeffender._unitsInfluence[0] * deffender.Count -
                              (attacker._unit.Attack * _playerAttacker._unitsInfluence[1] * attacker.Count) * attacker._unit.WeakCoeficient) / deffender._unit.Health * _playerDeffender._unitsInfluence[0]));
                }

                if (deffender._unit is Assasin)
                {
                    deffender.Count = (int) Math.Floor((deffender._unit.Health * _playerDeffender._unitsInfluence[1] * deffender.Count - (attacker._unit.Attack * _playerAttacker._unitsInfluence[1] *  attacker.Count)) /
                                       deffender._unit.Health * _playerDeffender._unitsInfluence[1]);
                }

                if (deffender._unit is Mage)
                {
                    deffender.Count =
                        (int) Math.Floor(
                            ((deffender._unit.Health * _playerDeffender._unitsInfluence[2] * deffender.Count -
                              (attacker._unit.Attack * _playerAttacker._unitsInfluence[2] *  attacker.Count) * attacker._unit.StrongCoefitient) / deffender._unit.Health * _playerDeffender._unitsInfluence[2]));
                }
            }

            if (attacker._unit is Mage)
            {
                StartCoroutine(MageAttack(attacker, deffender,  attacker.Count, _playerAttacker._unitsInfluence[2], _playerDeffender));
            }
            
            IEnumerator MageAttack(Squad _attacker, Squad _deffender, int _attackCount, float _mageInfluence, Player _deffenderPlayer)
            {
                int _numerator;
                _numerator = _turnNumeratorButton._attackNumerator;
                while (_turnNumeratorButton._attackNumerator!=_numerator+1 )
                {
                    yield return null;
                }
                if (_deffender._unit is Warrior)
                {
                    yield return _deffender.Count =
                        (int) Math.Floor(
                            ((_deffender._unit.Health * _deffenderPlayer._unitsInfluence[0] * _deffender.Count -
                              (_attacker._unit.Attack * _mageInfluence * _attackCount) * _attacker._unit.StrongCoefitient) / _deffender._unit.Health * _deffenderPlayer._unitsInfluence[0]));
                    yield break;
                }

                if (_deffender._unit is Assasin)
                {
                    yield return _deffender.Count =
                        (int) Math.Floor(
                            ((_deffender._unit.Health * _deffenderPlayer._unitsInfluence[1]* _deffender.Count -
                              (_attacker._unit.Attack * _mageInfluence * _attackCount) * _attacker._unit.WeakCoeficient) / _deffender._unit.Health * _deffenderPlayer._unitsInfluence[1]));
                    yield break;
                }

                if (_deffender._unit is Mage)
                {
                    yield return _deffender.Count = (int)Math.Floor(((_deffender._unit.Health * _deffenderPlayer._unitsInfluence[2] *  _deffender.Count - (_attacker._unit.Attack * _mageInfluence * _attackCount)) /
                                                     _deffender._unit.Health * _deffenderPlayer._unitsInfluence[2]));
                    yield break;
                }
            }
        }
        
    }
}