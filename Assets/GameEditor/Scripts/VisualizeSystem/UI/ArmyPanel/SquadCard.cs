using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class SquadCard : MonoBehaviour
    {
        [SerializeField] public Text UnitsNumber;
        [SerializeField] public Text HPCellAtrtibute;
        [SerializeField] public Text APCellAtrtibute;

        public void SetHPCellAtrtibute(float unitInfluence, int unitHP)
        {
            int hpCellAttribute = (int)((unitHP*unitInfluence)-unitHP);
            
            if (hpCellAttribute > 0)
            {
                HPCellAtrtibute.text = $"+ {hpCellAttribute}";
            }
            
            if (hpCellAttribute == 0)
            {
                HPCellAtrtibute.text = "0";
            }

            if (hpCellAttribute < 0)
            {
                HPCellAtrtibute.text = $"- {hpCellAttribute}";
            }
        }
        
        public void SetAPCellAtrtibute(float unitInfluence, int unitAP)
        {
            int apCellAttribute = (int)((unitAP*unitInfluence)-unitAP);
            
            if (apCellAttribute > 0)
            {
                HPCellAtrtibute.text = $"+ {apCellAttribute}";
            }
            
            if (apCellAttribute == 0)
            {
                HPCellAtrtibute.text = "0";
            }

            if (apCellAttribute < 0)
            {
                HPCellAtrtibute.text = $"- {apCellAttribute}";
            }
        }
    }
}