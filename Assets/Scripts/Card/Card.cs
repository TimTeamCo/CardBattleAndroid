using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardSpace
{
    public class Card : ScriptableObject
    {
        [BoxGroup("Basic Info")] 
        public int id;
        [BoxGroup("Basic Info")]
        public string cardName;
        
        [PreviewField(50, ObjectFieldAlignment.Left)]
        [BoxGroup("CardView")]
        [HorizontalGroup("CardView/Sprite", 150)]
        [LabelWidth(50)]
        public Sprite crystal;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [BoxGroup("CardView")]
        [HorizontalGroup("CardView/Sprite", 200)]
        [LabelWidth(50)]
        public Sprite frame;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [BoxGroup("CardView")]
        [HorizontalGroup("CardView/Sprite", 200)]
        [LabelWidth(50)]
        public Sprite art;

        [PropertySpace(50f)]
        [Button("UPDATE VIEW", ButtonHeight = 100, Icon = SdfIconType.CardImage, IconAlignment = IconAlignment.LeftEdge, Stretch = false)]
        private void UpdateView()
        {
            if (SceneManager.GetActiveScene().name != "Card")
            {
                return;
            }
            
            var cardScript = GameObject.Find("Canvas/Card").GetComponent<CardView>();
            if (cardScript == null) {return;}

            cardScript._cardData = this;
            cardScript.OnValidate();
        } 
    }
}
