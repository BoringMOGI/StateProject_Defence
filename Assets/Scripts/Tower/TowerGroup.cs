using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group", menuName = "Tower/TowerGroup")]
public class TowerGroup : ScriptableObject
{
    public Tower.TYPE type;
    public Tower[] towers;
    public Tower firstTower => towers[0];
    public int MaxLevel => towers.Length;
}
