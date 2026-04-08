using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.Android.Gradle.Manifest;
using System.Runtime.InteropServices.WindowsRuntime;



public class MapPieceBuild : MonoBehaviour
{
    public class Cell
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
    }

    public class RoomShape
    {
        public List<Vector2Int> cells;
    }


    private int m_gridWidth = 10;
    private int m_gridHeight = 10;

    Cell[,] m_grid;//二次元配列はx行y列の配列を用意　その中に型を入れれる
    bool[,] m_isValid;

    private void ReceiveMapData(int width, int height)
    {
        m_gridWidth = width;
        m_gridHeight = height;
    }


    private void Start()
    {
        GridMap(m_gridWidth, m_gridHeight);
        GridValidMap(m_gridWidth, m_gridHeight);
    }

    private void GridMap(int width, int height)//引数で受け取る
    {
        m_grid = new Cell[width, height];
    }

    private void GridValidMap(int width, int height)//床が有効かどうか
    {
        m_isValid = new bool[width, height];
    }

    private void CreatePiece()
    {
        var shap = new RoomShape();

        shap.cells = new List<Vector2Int>
        {
            new Vector2Int(0,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
        };

    }

    private void CreateGurids()
    {
        for(int x = 0; x < m_gridWidth; x++)
        {
            for (int z = 0; z < m_gridHeight; z++)
            {
                //m_grid.Add();
            }
        }
    }

    private bool CanPlace(RoomShape shape, Vector2Int origin)
    {
        foreach(var cell in shape.cells)
        {
            Vector2Int pos = origin + cell;

            if(pos.x < 0 || pos.x >= m_gridWidth) return false;
            if(pos.y < 0  || pos.y >= m_gridHeight) return false;

            //gridがIsValidがfalse かどうか

            if (m_grid[pos.x, pos.y] != null) return false;
        }

        return true;
    }
    
    
}
