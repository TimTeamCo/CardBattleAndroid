using System;
using UnityEngine;
using UnityEngine.UI;

namespace Calculator.UI
{
    public class DropDownCaption : MonoBehaviour
    {
        [SerializeField] public Dropdown Caption;
        public int numb;

        public void Clicked()
        {
            Caption.onValueChanged.AddListener(delegate
            {if(numb==0)
              //  Calculator.UI.AttackButton.a = Calculator.Units.Units.GetUnitDeck(numb, Caption.value);
                //else
                {
                   // Calculator.UI.AttackButton.d = Calculator.Units.Units.//GetUnitDeck(numb, Caption.value);
                }
            });
        }
        
    }
}