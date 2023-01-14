using System;
using CardSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberCardView : CardView
{
    [SerializeField] private GameObject Substate1;
    [SerializeField] private GameObject Substate2;
    [SerializeField] private GameObject Substate3;

    [SerializeField] private Sprite _statWarrior;
    [SerializeField] private Sprite _statSteamer;
    [SerializeField] private Sprite _statMage;
    
    [SerializeField] private Image Sub1Sta1img;
    [SerializeField] private Image Sub2Sta1img;
    [SerializeField] private Image Sub2Sta2img;
    [SerializeField] private Image Sub3Sta1img;
    [SerializeField] private Image Sub3Sta2img;
    [SerializeField] private Image Sub3Sta3img;
    
    [SerializeField] private TextMeshProUGUI Sub1Sta1txt;
    [SerializeField] private TextMeshProUGUI Sub2Sta1txt;
    [SerializeField] private TextMeshProUGUI Sub2Sta2txt;
    [SerializeField] private TextMeshProUGUI Sub3Sta1txt;
    [SerializeField] private TextMeshProUGUI Sub3Sta2txt;
    [SerializeField] private TextMeshProUGUI Sub3Sta3txt;
    private NumberCard _numberCard;
    private int[] _squadValues = new int[3];
    private Sprite[] _statSprites = new Sprite[3];

    public override void OnValidate()
    {
        base.OnValidate();

        _numberCard = _cardData as NumberCard;

        if (_numberCard == null)
        {
            Debug.LogWarning($"CardData is not NumberCard");
            return;
        }

        _squadValues[0] = _numberCard.Warriors;
        _squadValues[1] = _numberCard.Assasin;
        _squadValues[2] = _numberCard.Mage;

        _statSprites[0] = _statWarrior;
        _statSprites[1] = _statSteamer;
        _statSprites[2] = _statMage;

        Substate1.SetActive(false);
        Substate2.SetActive(false);
        Substate3.SetActive(false);
        
        switch (CheckCountOfSquads())
        {
            case 1:
                SetSubstance1();
                break;
            case 2:
                SetSubstance2();
                break;
            case 3:
                SetSubstance3();
                break;
        }
    }
    
    private int CheckCountOfSquads()
    {
         int a = _numberCard.Warriors == 0 ? 0 : 1;
         int b = _numberCard.Assasin == 0 ? 0 : 1;
         int c = _numberCard.Mage == 0 ? 0 : 1;

         return a + b + c;
    }

    private void SetSubstance1()
    {
        Substate1.SetActive(true);

        foreach (var value in _squadValues)
        {
            if (value != 0)
            {
                Sub1Sta1txt.text = $"{value}";
                Sub1Sta1img.sprite = _statSprites[Array.IndexOf(_squadValues, value)];
                return;
            }
        }
    }

    private void SetSubstance2()
    {
        int k = 1;
        Substate2.SetActive(true);

        foreach (var value in _squadValues)
        {
            if (value == 0) continue;

            if (k == 1)
            {
                k++;
                Sub2Sta1txt.text = $"{value}";
                Sub2Sta1img.sprite = _statSprites[Array.IndexOf(_squadValues, value)];
                continue;
            }

            if (k == 2)
            {
                Sub2Sta2txt.text = $"{value}";
                Sub2Sta2img.sprite = _statSprites[Array.IndexOf(_squadValues, value)];
            }
        }
    }

    private void SetSubstance3()
    {
        Substate3.SetActive(true);
        Sub3Sta1txt.text = $"{_squadValues[1]}";
        Sub3Sta2txt.text = $"{_squadValues[2]}";
        Sub3Sta3txt.text = $"{_squadValues[0]}";
        Sub3Sta1img.sprite = _statSprites[1];
        Sub3Sta2img.sprite = _statSprites[2];
        Sub3Sta3img.sprite = _statSprites[0];
    }
}