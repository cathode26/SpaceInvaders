using UnityEngine;
using System.Collections.Generic;
using static SpaceInvaders.PrefabTypes;

[CreateAssetMenu(fileName = "LevelData", menuName = "SpaceInvaders/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public List<SpawnableType> enemiesList; // Only the types of aliens you want in this level
    public int numberOfRows; // Number of rows of aliens
    public int numberOfColumns; // Number of columns of aliens
}
