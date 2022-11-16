using TTBattle.UI;
using UnityEngine;
using UnityEngine.UI;

public class NextCellInformer : MonoBehaviour
{
    [SerializeField] private Text _warriorInfluenceText;
    [SerializeField] private Text _assasinInfluenceText;
    [SerializeField] private Text _mageInfluenceText;
    [SerializeField] private Text _nextCellCard;
    [SerializeField] private Color _green;
    [SerializeField] private Color _red;
    [SerializeField] private Color _yellow;
    private bool _isSelected;
    private float _warriorInfluenceValue;
    private float _assasinInfluenceValue;
    private float _mageInfluenceValue;

    private void OnEnable()
    {
        _warriorInfluenceText.color = Color.white;
        _assasinInfluenceText.color = Color.white;
        _mageInfluenceText.color = Color.white;
        _warriorInfluenceText.text = "--";
        _assasinInfluenceText.text = "--";
        _mageInfluenceText.text = "--";
    }

    public void SetUnitsIfluenceText(MapCell mapCell, bool cellIsSelected)
    {
        float warriorInfluence = (mapCell.uintsInfluence[0] * 100) - 100;
        float assasinInfluence = (mapCell.uintsInfluence[1] * 100) - 100;
        float mageInfluence = (mapCell.uintsInfluence[2] * 100) - 100;
        
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
        
        
        if (assasinInfluence > 0)
        {
            _assasinInfluenceText.color = _green;
            _assasinInfluenceText.text = $"+{assasinInfluence}%";
        }
        if (assasinInfluence == 0)
        {
            _assasinInfluenceText.color = Color.white;
            _assasinInfluenceText.text = "--";
        }
        if (assasinInfluence < 0)
        {
            _assasinInfluenceText.color = _red;
            _assasinInfluenceText.text = $"-{assasinInfluence}%";
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
            _assasinInfluenceText.color = _yellow;
            _mageInfluenceText.color = _yellow;
            _warriorInfluenceValue = warriorInfluence;
            _assasinInfluenceValue = assasinInfluence;
            _mageInfluenceValue = mageInfluence;
        }
    }

    public void ExitCell()
    {
        if (_isSelected)
        {
            _warriorInfluenceText.color = _yellow;
            _assasinInfluenceText.color = _yellow;
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
        
        
            if (_assasinInfluenceValue > 0)
            {
                _assasinInfluenceText.text = $"+{_assasinInfluenceValue}%";
            }
            if (_assasinInfluenceValue == 0)
            {
                _assasinInfluenceText.text = "--";
            }
            if (_assasinInfluenceValue < 0)
            {
                _assasinInfluenceText.text = $"-{_assasinInfluenceValue}%";
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
