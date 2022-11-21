using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

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

        public void SetHPCellAtrtibute(float unitInfluence, int unitHP)
        {
            int hpCellAttribute = (int)((unitHP*unitInfluence)-unitHP);
            
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
                HPCellAtrtibute.text = $"- {hpCellAttribute*-1}";
                HPCellAtrtibute.color = _red;
            }
        }
        
        public void SetAPCellAtrtibute(float unitInfluence, int unitAP)
        {
            int apCellAttribute = (int)((unitAP*unitInfluence)-unitAP);
            
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
                APCellAtrtibute.text = $"- {apCellAttribute*-1}";
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

        public void SetBurningDamageText(ArmyPanel armyPanel)
        {
            var player = armyPanel.Player;
            if (player.PlayerMapCell.BurningDamage != 0)
            {
                _fireImage.enabled = true;
                _fireDamageText.enabled = true;
                var burningDamage = player.PlayerMapCell.BurningDamage;
                if (burningDamage == 3)
                {
                    _fireImage.sprite = player.PlayerMapCell._map.FireStage1;
                }
                if (burningDamage == 6)
                {
                    _fireImage.sprite = player.PlayerMapCell._map.FireStage2;
                }
                if (burningDamage == 9)
                {
                    _fireImage.sprite = player.PlayerMapCell._map.FireStage3;
                }
                _fireDamageText.text = $"- {player.PlayerMapCell.BurningDamage}";
                _fireDamageText.color = _red;
            }
            else
            {
                _fireImage.enabled = false;
                _fireDamageText.enabled = false;
            }
        }
    }
}