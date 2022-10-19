using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCellScrip : MonoBehaviour
{
    [SerializeField] public int id;
    [SerializeField] public List<MapCellScrip> NextCell;
}
