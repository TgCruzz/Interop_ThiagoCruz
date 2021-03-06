using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Board))]

public class BoardInputHandler : MonoBehaviour, InputHandler
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action onClick)
    {
        board.OnSquareSelected(inputPosition);
    }
}
