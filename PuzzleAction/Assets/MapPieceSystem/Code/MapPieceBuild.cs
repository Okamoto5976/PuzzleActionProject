using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class MapPieceBuild : MonoBehaviour
{
    [SerializeField] private GameObject m_cube;

    [SerializeField] private Transform m_parent;

    public void Generate(int gridWidth, int gridHeight)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                var obj = InstantiateGridMap();

                obj.transform.localPosition = new Vector3((x + 0.5f) * 1.5f, 0, (z + 0.5f) * 1.5f);
            }
        }
    }

    private GameObject InstantiateGridMap()
    {
        var obj = Instantiate(m_cube, m_parent);

        return obj;
    }

    [SerializeField] private GameObject m_pieceParent;
    [SerializeField] private GameObject m_piece;

    public GameObject InstantiatePieceObject(Rooma room)
    {
        var parent = Instantiate(m_pieceParent);
        parent.transform.position = new Vector3(-10, 0, 0);

        foreach (var cell in room.cells)
        {
            var piece = Instantiate(m_piece, parent.transform);

            piece.transform.localPosition = new Vector3(
                (cell.x + 0.5f) * 1.5f,
                0,
                (cell.y + 0.5f) * 1.5f
                );
        }

        return parent;
    }
}
