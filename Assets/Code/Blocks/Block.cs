using UnityEngine;

[System.Serializable]
public class Block
{

    public enum TypeOfBlock
    {
        MoveOnX,
        MoveOnZ,
        MoveOnBothAxis,
        Fixed,
        SlideOnX,
        SlideOnZ
        
    }

}

