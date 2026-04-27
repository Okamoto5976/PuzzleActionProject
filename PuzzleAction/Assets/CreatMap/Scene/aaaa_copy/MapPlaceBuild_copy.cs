using UnityEditor.SceneManagement;
using UnityEngine;

public class MapPlaceBuild_copy : MonoBehaviour
{
    [SerializeField] private GameObject m_cube;

    [SerializeField] private Transform m_parent;

    public void Generate(Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
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

    public void GenerateRoomObject(Room room)
    {
        var parent = Instantiate(m_pieceParent);
        float worldPosX = UnityEngine.Random.Range(3f, 14f);
        float worldPosZ = UnityEngine.Random.Range(7f, -7f);
        parent.transform.position = new Vector3(worldPosX, 1, worldPosZ);


        for(int y = 0; y < room.Size.y; y++)
        {
            for(int x = 0;x < room.Size.x; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                int roomIndex = x + y * room.Size.x;
                if (room.Floors[roomIndex].State == Floor.FloorState.empty) continue;
                var floor = Instantiate(m_piece, parent.transform);

                floor.transform.localPosition = new Vector3(
                    (x + 0.5f) * 1.5f,
                    parent.transform.position.y,
                    (y + 0.5f) * 1.5f
                );
            }
        }

        var obj = parent.GetComponent<RoomObj>();
        obj.SetRoom(room);
        obj.Init();
    }


}
