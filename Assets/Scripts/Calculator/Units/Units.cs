using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Calculator.Units
{
    public class Units : MonoBehaviour
    {
        [SerializeField] public GameObject Army1;
        [SerializeField] public GameObject Army2;
       private List<Unit> player1 = new List<Unit>();
       private List<Unit> player2 = new List<Unit>();
       
       private void Start()
       {
           FillListOfUnits(player1);
           FillListOfUnits(player2);
       }

       private void FillListOfUnits(List<Unit> units)
       {
           Squad warriors = new Squad();
           for (int i = 0; i < warriors.Count; i++)
           {
               Warrior warrior = new Warrior();
               units.Add(warrior);
           }
       }
       
        public class Unit: IUnit
        {
            public const float strongCoefitient = 1.5f;
          public const float weakCoeficient = 0.5f;
          public int attack;
          public int health;

          public int Health
          {
              get => health;
              set
              {
                  if (value != health)
                  {
                      health = value;
                  }
              }
          }

            public int Attack { get; set; }
            public float StrongCoefitient { get; }
            public float WeakCoeficient { get; }
            public virtual void AttackAction()
            {
                
            }
        }
        
        public interface IUnit
        {
            public int Health { get; set; }
            public int Attack { get; set; }
            public float StrongCoefitient { get; }
            public float WeakCoeficient { get; }
            void AttackAction();
        }

        public class Warrior : Unit
        {
            public Warrior()
            {
                Attack = 10;
                Health = 90;
            }

            public override void AttackAction()
            {
                
            }
        }


        public class Assassin : Unit, IUnit
        {
            new public int Attack { get; set; }
            new public int Health { get; set; }
            public int Count { get; set; }
            new public float StrongCoefitient { get; set; }
            new public float WeakCoeficient { get; set; }
            public Assassin()
            {
                Attack = base.Attack;
                Health = base.Health;
                Count = 10;
                StrongCoefitient = base.StrongCoefitient;
                WeakCoeficient = base.WeakCoeficient;
            }
        }

        public class Mage : Unit, IUnit
        {
            new public int Attack { get; set; }
            new public int Health { get; set; }
            public int Count { get; set; }
            new public float StrongCoefitient { get; set; }
            new public float WeakCoeficient { get; set; }


            public Mage()
            {
                Attack = base.Attack * 3;
                Health = base.Health;
                Count = 10;
                StrongCoefitient = base.StrongCoefitient;
                WeakCoeficient = base.WeakCoeficient;
            }
        }
    }
}


