using UnityEngine;

[System.Serializable]
public class Block
{
    public bool isSquishy;

    public enum TypeOfBlock
    {
        MoveOnX,
        MoveOnZ,
        MoveOnBothAxis,
        Fixed
        
    }

}

