using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTask_SO", menuName = "PlayerTask/PlayerTask_SO")]
public class PlayerTask_SO : ScriptableObject
{
    public List<PlayerTask> Task;
}
