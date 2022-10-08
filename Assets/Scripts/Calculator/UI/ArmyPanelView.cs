using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTCalculator.UI
{
    public class ArmyPanelView : MonoBehaviour
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

        public void ChangeText(PlayerHand a)
        {
            //SetNumberUnits(a);
        }
        private void SetTitlePanel()
        {
            mText.text = gameObject.name;
        }

        static public void SetNumberUnits(PlayerHand a)
        {
           //  b = Convert.ToString(a.Count);
            
        }
        
    }
}