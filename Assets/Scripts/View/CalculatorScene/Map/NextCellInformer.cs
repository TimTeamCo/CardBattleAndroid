using Army;
using TTBattle.UI;
using UnityEngine;
using UnityEngine.UI;

public class NextCellInformer : MonoBehaviour
{
    [SerializeField] private Text _warriorInfluenceText;
    [SerializeField] private Text _steamerInfluenceText;
    [SerializeField] private Text _mageInfluenceText;
    [SerializeField] private Text _nextCellCard;
    
    private Color _green = Color.green;
    private Color _red = Color.red;
    private Color _yellow = Color.yellow;
    private bool _isSelected;
    private float _warriorInfluenceValue;
    private float _steamerInfluenceValue;
    private float _mageInfluenceValue;

    private void Start()
    {
        _warriorInfluenceText.color = Color.white;
        _steamerInfluenceText.color = Color.white;
        _mageInfluenceText.color = Color.white;
        _warriorInfluenceText.text = "--";
        _steamerInfluenceText.text = "--";
        _mageInfluenceText.text = "--";
    }

    public void SetUnitsIfluenceText(MapCell mapCell, bool cellIsSelected)
    {
        float warriorInfluence = mapCell.MapZone.GetUnitInfluence(UnitType.Warrior);
        float steamerInfluence = mapCell.MapZone.GetUnitInfluence(UnitType.Steamer);
        float mageInfluence = mapCell.MapZone.GetUnitInfluence(UnitType.Mage);
        
        WriteInfluence(warriorInfluence, _warriorInfluenceText);
        WriteInfluence(steamerInfluence, _steamerInfluenceText);
        WriteInfluence(mageInfluence, _mageInfluenceText);

        if (cellIsSelected == false) return;
        
        _isSelected = true;
        _warriorInfluenceText.color = _yellow;
        _steamerInfluenceText.color = _yellow;
        _mageInfluenceText.color = _yellow;
        _warriorInfluenceValue = warriorInfluence;
        _steamerInfluenceValue = steamerInfluence;
        _mageInfluenceValue = mageInfluence;
    }

    private void WriteInfluence(float influence, Text influenceText)
    {
        switch (influence)
        {
            case > 0:
                influenceText.color = _green;
                influenceText.text = $"+{influence}%";
                break;
            case 0:
                influenceText.color = Color.white;
                influenceText.text = "--";
                break;
            case < 0:
                influenceText.color = _red;
                influenceText.text = $"-{influence}%";
                break;
        }
    }
    
    public void ExitCell()
    {
        if (_isSelected)
        {
            _warriorInfluenceText.color = _yellow;
            _steamerInfluenceText.color = _yellow;
            _mageInfluenceText.color = _yellow;
            if (_warriorInfluenceValue > 0)
            {
                _warriorInfluenceText.text = $"+{_warriorInfluenceValue}%";
            }
            if (_warriorInfluenceValue == 0)
            {
                _warriorInfluenceText.text = "--";
            }
            if (_warriorInfluenceValue < 0)
            {
                _warriorInfluenceText.text = $"-{_warriorInfluenceValue}%";
            }
        
        
            if (_steamerInfluenceValue > 0)
            {
                _steamerInfluenceText.text = $"+{_steamerInfluenceValue}%";
            }
            if (_steamerInfluenceValue == 0)
            {
                _steamerInfluenceText.text = "--";
            }
            if (_steamerInfluenceValue < 0)
            {
                _steamerInfluenceText.text = $"-{_steamerInfluenceValue}%";
            }
        
        
            if (_mageInfluenceValue > 0)
            {
                _mageInfluenceText.text = $"+{_mageInfluenceValue}%";
            }
            if (_mageInfluenceValue == 0)
            {
                _mageInfluenceText.text = "--";
            }
            if (_mageInfluenceValue < 0)
            {
                _mageInfluenceText.text = $"-{_mageInfluenceValue}%";
            }
        }
    }

    public void IsNotCelected()
    {
        _isSelected = false;
    }
}
