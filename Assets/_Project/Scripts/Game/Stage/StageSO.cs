using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Puzzle/Stage")]
public class StageSO : ScriptableObject
{
    public string DisplayName;
    public string SceneName;
    public int StageScoreValue = 250;
}
