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
    [SerializeField] private Color _green;
    [SerializeField] private Color _red;
    [SerializeField] private Color _yellow;
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
        
        if (warriorInfluence > 0)
        {
            _warriorInfluenceText.color = _green;
            _warriorInfluenceText.text = $"+{warriorInfluence}%";
        }
        if (warriorInfluence == 0)
        {
            _warriorInfluenceText.color = Color.white;
            _warriorInfluenceText.text = "--";
        }
        if (warriorInfluence < 0)
        {
            _warriorInfluenceText.color = _red;
            _warriorInfluenceText.text = $"-{warriorInfluence}%";
        }
        
        
        if (steamerInfluence > 0)
        {
            _steamerInfluenceText.color = _green;
            _steamerInfluenceText.text = $"+{steamerInfluence}%";
        }
        if (steamerInfluence == 0)
        {
            _steamerInfluenceText.color = Color.white;
            _steamerInfluenceText.text = "--";
        }
        if (steamerInfluence < 0)
        {
            _steamerInfluenceText.color = _red;
            _steamerInfluenceText.text = $"-{steamerInfluence}%";
        }
        
        
        if (mageInfluence > 0)
        {
            _mageInfluenceText.color = _green;
            _mageInfluenceText.text = $"+{mageInfluence}%";
        }
        if (mageInfluence == 0)
        {
            _mageInfluenceText.color = Color.white;
            _mageInfluenceText.text = "--";
        }
        if (mageInfluence < 0)
        {
            _mageInfluenceText.color = _red;
            _mageInfluenceText.text = $"-{mageInfluence}%";
        }

        if (cellIsSelected)
        {
            _isSelected = true;
            _warriorInfluenceText.color = _yellow;
            _steamerInfluenceText.color = _yellow;
            _mageInfluenceText.color = _yellow;
            _warriorInfluenceValue = warriorInfluence;
            _steamerInfluenceValue = steamerInfluence;
            _mageInfluenceValue = mageInfluence;
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
