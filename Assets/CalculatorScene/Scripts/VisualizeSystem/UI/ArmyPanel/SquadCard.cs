using PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class SquadCard : MonoBehaviour
    {
        [SerializeField] private Image _fireImage;
        [SerializeField] private Text _fireDamageText;
        [SerializeField] private Color _green;
        [SerializeField] private Color _red;
        [SerializeField] public Text UnitsNumber;
        [SerializeField] public Text HPCellAtrtibute;
        [SerializeField] public Text APCellAtrtibute;
        [SerializeField] public Text HPSquadAtrtibute;
        [SerializeField] public Text APSquadAtrtibute;

        private void OnEnable()
        {
            _green = HPCellAtrtibute.color;
            _red = APCellAtrtibute.color;
            _fireImage.enabled = false;
            _fireDamageText.enabled = false;
        }

        public void SetUnitStats(float unitInfluence, int health, int damage)
        {
            
            int hpCellAttribute = (int) (health * unitInfluence) - health;
            int apCellAttribute = (int) (damage * unitInfluence) - damage;

            if (hpCellAttribute == 0)
            {
                HPCellAtrtibute.text = "__";
                HPCellAtrtibute.color = Color.white;
                APCellAtrtibute.text = "__";
                APCellAtrtibute.color = Color.white;
                return;
            }

            HPCellAtrtibute.text =
                hpCellAttribute > 0
                    ? $"+ {hpCellAttribute}"
                    : $"- {hpCellAttribute * -1}"; //hpCellAttribute * -1 == -hpCellAttribute
            HPCellAtrtibute.color = hpCellAttribute > 0 ? _green : _red;
            APCellAtrtibute.text = apCellAttribute > 0 ? $"+ {apCellAttribute}" : $"- {apCellAttribute * -1}";
            APCellAtrtibute.color = apCellAttribute > 0 ? _green : _red;
        }

        public void SetHPCellAtrtibute(float unitInfluence, int unitHP)
        {
            int hpCellAttribute = (int) ((unitHP * unitInfluence) - unitHP);

            if (hpCellAttribute > 0)
            {
                HPCellAtrtibute.text = $"+ {hpCellAttribute}";
                HPCellAtrtibute.color = _green;
            }

            if (hpCellAttribute == 0)
            {
                HPCellAtrtibute.text = "__";
                HPCellAtrtibute.color = Color.white;
            }

            if (hpCellAttribute < 0)
            {
                HPCellAtrtibute.text = $"- {hpCellAttribute * -1}";
                HPCellAtrtibute.color = _red;
            }
        }

        public void SetAPCellAtrtibute(float unitInfluence, int unitAP)
        {
            int apCellAttribute = (int) ((unitAP * unitInfluence) - unitAP);

            if (apCellAttribute > 0)
            {
                APCellAtrtibute.text = $"+ {apCellAttribute}";
                APCellAtrtibute.color = _green;
            }

            if (apCellAttribute == 0)
            {
                APCellAtrtibute.text = "__";
                APCellAtrtibute.color = Color.white;
            }

            if (apCellAttribute < 0)
            {
                APCellAtrtibute.text = $"- {apCellAttribute * -1}";
                APCellAtrtibute.color = _red;
            }
        }

        public void SetHPSquadAtrtibute(int unitCount)
        {
            HPSquadAtrtibute.text = $"{unitCount}";
            HPSquadAtrtibute.color = _green;
        }

        public void SetAPSquadAtrtibute(int unitCount)
        {
            HPSquadAtrtibute.text = $"{unitCount}";
            HPSquadAtrtibute.color = _red;
        }

        public void SetBurningDamageText(PlayerDataCalculator playerDataCalculator)
        {
            var burnFactor = playerDataCalculator.MapZone.burnFactor;
            if (burnFactor == 0)
            {
                _fireImage.enabled = false;
                _fireDamageText.enabled = false;
                return;
            }

            _fireImage.enabled = true;
            _fireDamageText.enabled = true;

            _fireImage.sprite = burnFactor switch
            {
                3 => playerDataCalculator.PlayerMapCell._map.FireStage1,
                6 => playerDataCalculator.PlayerMapCell._map.FireStage2,
                9 => playerDataCalculator.PlayerMapCell._map.FireStage3,
                _ => _fireImage.sprite
            };

            _fireDamageText.text = $"- {burnFactor}";
            _fireDamageText.color = _red;
        }
    }
}