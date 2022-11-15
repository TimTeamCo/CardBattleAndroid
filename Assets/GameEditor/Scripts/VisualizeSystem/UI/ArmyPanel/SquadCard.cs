using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace TTBattle.UI
{
    public class SquadCard : MonoBehaviour
    {
        [SerializeField] private Color _hpCellColor;
        [SerializeField] private Color _apCellColor;
        [SerializeField] public Text UnitsNumber;
        [SerializeField] public Text HPCellAtrtibute;
        [SerializeField] public Text APCellAtrtibute;

        private void OnEnable()
        {
            _hpCellColor = HPCellAtrtibute.color;
            _apCellColor = APCellAtrtibute.color;
        }

        public void SetHPCellAtrtibute(float unitInfluence, int unitHP)
        {
            int hpCellAttribute = (int)((unitHP*unitInfluence)-unitHP);
            
            if (hpCellAttribute > 0)
            {
                HPCellAtrtibute.text = $"+ {hpCellAttribute}";
                HPCellAtrtibute.color = _hpCellColor;
            }
            
            if (hpCellAttribute == 0)
            {
                HPCellAtrtibute.text = "__";
                HPCellAtrtibute.color = Color.white;
            }

            if (hpCellAttribute < 0)
            {
                HPCellAtrtibute.text = $"- {hpCellAttribute*-1}";
                HPCellAtrtibute.color = _hpCellColor;
            }
        }
        
        public void SetAPCellAtrtibute(float unitInfluence, int unitAP)
        {
            int apCellAttribute = (int)((unitAP*unitInfluence)-unitAP);
            
            if (apCellAttribute > 0)
            {
                APCellAtrtibute.text = $"+ {apCellAttribute}";
                APCellAtrtibute.color = _apCellColor;
            }
            
            if (apCellAttribute == 0)
            {
                APCellAtrtibute.text = "__";
                APCellAtrtibute.color = Color.white;
            }

            if (apCellAttribute < 0)
            {
                APCellAtrtibute.text = $"- {apCellAttribute*-1}";
                APCellAtrtibute.color = _apCellColor;
            }
        }
    }
}