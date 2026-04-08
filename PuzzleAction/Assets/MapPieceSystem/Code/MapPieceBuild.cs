using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class MapPieceBuild : MonoBehaviour
{
    private int m_gridWidth = 10;
    private int m_gridHeight = 10;

    private struct Grid
    {
        public int x;
        public int y;
    }

    private List<Grid> m_grid = new List<Grid>();

    private void Start()
    {
        
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

    private void BuildMapPiece()
    {

    }
}
