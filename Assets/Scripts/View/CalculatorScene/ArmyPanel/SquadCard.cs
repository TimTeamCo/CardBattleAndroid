using PlayerDataSO;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class SquadCard : MonoBehaviour
    {
        [SerializeField] private Image _fireImage;
        [SerializeField] private Text _fireDamageText;
        [SerializeField] public Text UnitsNumber;
        [SerializeField] public Text HPCellAtrtibute;
        [SerializeField] public Text APCellAtrtibute;

        private Color _green = Color.green;
        private Color _red = Color.red;
        
        private void OnEnable()
        {
            _fireImage.enabled = false;
            _fireDamageText.enabled = false;
        }

        public void SetUnitStats(float unitInfluence, int health, int damage)
        {
            int hpCellAttribute = (int) (health * unitInfluence); 
            int apCellAttribute = (int) (damage * unitInfluence);

            if (hpCellAttribute == 0)
            {
                HPCellAtrtibute.text = "__";
                HPCellAtrtibute.color = Color.white;
                APCellAtrtibute.text = "__";
                APCellAtrtibute.color = Color.white;
                return;
            }

            HPCellAtrtibute.text = hpCellAttribute > 0 ? $"+ {hpCellAttribute}" : $"- { -hpCellAttribute}";
            HPCellAtrtibute.color = hpCellAttribute > 0 ? _green : _red;
            APCellAtrtibute.text = apCellAttribute > 0 ? $"+ {apCellAttribute}" : $"- {apCellAttribute * -1}";
            APCellAtrtibute.color = apCellAttribute > 0 ? _green : _red;
        }

        public void SetBurningDamageText(PlayerDataSoCalculator playerDataSoCalculator)
        {
            var burnFactor = playerDataSoCalculator.MapZone.burnFactor;
            if (burnFactor == 0)
            {
                _fireImage.enabled = false;
                _fireDamageText.enabled = false;
                return;
            }

            _fireImage.enabled = true;
            _fireDamageText.enabled = true;

            /*_fireImage.sprite = burnFactor switch
            {
                3 => playerDataCalculator.PlayerMapCell._map.FireStage1,
                6 => playerDataCalculator.PlayerMapCell._map.FireStage2,
                9 => playerDataCalculator.PlayerMapCell._map.FireStage3,
                _ => _fireImage.sprite
            };*/

            _fireDamageText.text = $"- {burnFactor}";
            _fireDamageText.color = _red;
        }
    }
}