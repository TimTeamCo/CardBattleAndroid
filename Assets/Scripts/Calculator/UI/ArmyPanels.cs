using System;
using UnityEngine;
using UnityEngine.UI;

namespace Calculator.UI
{
    public class ArmyPanels : MonoBehaviour
    {
        public Text mText;
        static string b;
       
        public void GetText()
        {

             mText.text = b;
        }
        void Start()
        {
            SetTitlePanel();
        }

        public void ChangeText(Calculator.Units.Units.IUnit a)
        {
            SetNumberUnits(a);
        }
        private void SetTitlePanel()
        {
            mText.text = gameObject.name;
        }

        static public void SetNumberUnits(Calculator.Units.Units.IUnit a)
        {
           // b = Convert.ToString(a.Count);
            
        }
        
    }
}