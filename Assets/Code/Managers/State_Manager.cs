using System;
using System.Collections.Generic;
using UnityEngine;

public class State_Manager : MonoBehaviour
{

    private Stack<(IBlock block, Vector3 previousPosition)> globalMoves = new Stack<(IBlock, Vector3)>();

    private Dictionary<IBlock, Stack<Vector3>> blockPositions = new Dictionary<IBlock, Stack<Vector3>>();

    private void OnEnable()
    {
        GameManager.OnMoveToRegister += RegisterMove;
        GameManager.OnLevelCompleted += ClearAll;
    }
    private void OnDisable()
    {
        GameManager.OnMoveToRegister -= RegisterMove;
        GameManager.OnLevelCompleted -= ClearAll;
    }


    void RegisterMove(IBlock block, Vector3 position)
    {
        if (!blockPositions.ContainsKey(block))
        {
            blockPositions[block] = new Stack<Vector3>();
        }
        blockPositions[block].Push(position);
        globalMoves.Push((block, position));
        Debug.Log($"Mossa registrata: Blocco {block} -> Posizione {position}");
    }

    public void UndoLastMove() // button function
    {
        if (globalMoves.Count > 0)
        {
            var lastMove = globalMoves.Pop();

            lastMove.block.RestorePositionTo(lastMove.previousPosition);

            blockPositions[lastMove.block].Pop();
        }
        else Debug.Log("No Move To Undo");
    }

    void ClearAll()
    {
        globalMoves.Clear();
        blockPositions.Clear();
    }

       


    




}
