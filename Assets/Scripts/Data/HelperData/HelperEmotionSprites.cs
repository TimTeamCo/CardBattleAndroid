using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelperEmotionSprites", menuName = "ScriptableObject/Helper/HelperEmotionSprites",
    order = 0)]
public class HelperEmotionSprites : ScriptableObject
{
    [SerializeField] private List<HelperSourse> HelperEmotions = new ();

    public Sprite GetEmotion(HelperEmotionsEnum type)
    {
        foreach (var helperSourse in HelperEmotions)
        {
            if (helperSourse.HelperEmotionsEnum == type)
            {
                return helperSourse.EmotionSprite;
            }
        }
        
        Debug.LogError("EmotionSprite not found");
        return null;
    }
}

[Serializable]
public struct HelperSourse
{
    public HelperEmotionsEnum HelperEmotionsEnum;
    public Sprite EmotionSprite;
}