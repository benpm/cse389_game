using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperties : MonoBehaviour
{
    public enum LevelType
    {
        Game, Shop
    };

    public Rect viewBox;
    public ScenePicker nextLevel { get; private set; }
    public LevelType levelType = LevelType.Game;

    private void Start()
    {
        nextLevel = GetComponent<ScenePicker>();
        Debug.Assert(nextLevel);
    }
}
