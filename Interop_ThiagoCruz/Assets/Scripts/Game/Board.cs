using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{

    public const int BOARD_SIZE = 8;

    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece [,] grid;
    private Piece selectedPiece;
    private GameController gameController;

    private void Awake()
    {
        CreateGrid();
    }

    public void SetDependencies(GameController gameController)
    {
        this.gameController = gameController;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).y / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y); 
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && gameController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
            else if (selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMove(coords, selectedPiece);
        }
        else
        {
            if (piece != null && gameController.IsTeamTurnActive(piece.team))
                SelectPiece(piece); 
        }
    }

    private void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
    }

    private void OnSelectedPieceMove(Vector2Int coords, Piece piece)
    {
        UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
        selectedPiece.MovePiece(coords);
        DeselectPiece();
        EndTurn();
    }

    private void EndTurn()
    {
        gameController.EndTurn();
    }

    private void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    private Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordsOnBoard(coords))
            return grid[coords.x, coords.y];
        return null; 
    }

    private bool CheckIfCoordsOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    public bool HasPiece(Piece piece)
    {
        for(int i = 0; i < BOARD_SIZE; i++)
        {
            for(int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }
        return false;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if(CheckIfCoordsOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }

}