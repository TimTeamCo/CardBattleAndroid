using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        public MapCellScrip startMapCell;
        public MapCellScrip playerMapCell;

        private void Start()
        { 
            playerMapCell = startMapCell;
        }
    }
}