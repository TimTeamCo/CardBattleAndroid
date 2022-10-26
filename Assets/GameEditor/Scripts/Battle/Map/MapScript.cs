using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        public MapCellScrip startMapCell;
        public MapCellScrip playerMapCell; 
        public float[] uintsInfluence = new float [3];

        private void Start()
        { 
            playerMapCell = startMapCell;
        }
    }
}