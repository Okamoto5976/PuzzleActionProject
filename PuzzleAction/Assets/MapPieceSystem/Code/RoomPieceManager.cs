using System.Collections;
using UnityEngine;

public class RoomPieceManager : MonoBehaviour
{
    [SerializeField] private GameObject m_MapPieceUI;

    [SerializeField] private GameObject m_roomPieceParent;
    [SerializeField] private GameObject m_roomPiece;

    [SerializeField] private int m_pieceAmount;

    public void OnCallGenerate()
    {
        for(int i = 0; i < m_pieceAmount; i++)
        {
            Room room = CreateRoom();
            GenerateRoomObject(room);
        }
    }

    #region ルーム作成
    private Room CreateRoom()
    {
        int num = UnityEngine.Random.Range(0, 6);

        Room room = new(new(), new(0, 0));

        if (num == 0)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                }, new(3, 3)
                );
        }
        else if (num == 1)
        {
            room = new(
                new()
                {
                    Floor.FloorState.empty,Floor.FloorState.full,Floor.FloorState.empty,
                    Floor.FloorState.full, Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.empty,Floor.FloorState.full,Floor.FloorState.empty,
                }, new(3, 3)
                );
        }
        else if (num == 2)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full ,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full ,Floor.FloorState.full,
                }, new(3, 3)
                );
        }
        else if (num == 3)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,
                }, new(2, 2)
                );
        }
        else if (num == 4)
        {
            room = new(
               new()
               {
                    Floor.FloorState.full,Floor.FloorState.empty,
                    Floor.FloorState.full,Floor.FloorState.full,
               }, new(2, 2)
               );
        }
        else if (num == 5)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                }, new(5, 1)
                );
        }

        return room;
    }
    #endregion


    //シーン上でのマップピースを生成
    public void GenerateRoomObject(Room room)
    {
        GameObject parentObj = Instantiate(m_roomPieceParent, m_MapPieceUI.transform);
        float rectX = UnityEngine.Random.Range(-300f, 300f);
        float rectY = UnityEngine.Random.Range(-500f, 500f);

        RectTransform rect = parentObj.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(rectX, rectY);


        for (int y = 0; y < room.Size.y; y++)
        {
            for (int x = 0; x < room.Size.x; x++)
            {
                int roomIndex = x + y * room.Size.x;
                if (room.Floors[roomIndex].State == Floor.FloorState.empty) continue;
                var floor = Instantiate(m_roomPiece, parentObj.transform);
                var FloorRect = floor.GetComponent<RectTransform>();


                FloorRect.anchoredPosition = new Vector2(
                    x * 50f,
                    y * 50f
                );

                var roomPiece = floor.GetComponent<RoomPiece>();
                roomPiece.SetIndex(new Vector2Int(x, y));
            }
        }

        var roomPieceParent = parentObj.GetComponent<RoomPieceParent>();
        roomPieceParent.SetRoom(room);

        //SetAreatype
        AreaType type = (AreaType)Random.Range(0, System.Enum.GetValues(typeof(AreaType)).Length);

        roomPieceParent.SetAreaType(type);
        roomPieceParent.Init(this);

        //後　AreaTypeを仮から変更
        switch (type)
        {
            case AreaType.None:
                break;
            case AreaType.Enemy:
                roomPieceParent.SetColor(Color.red);
                break;
            case AreaType.Shop:
                roomPieceParent.SetColor(Color.green);
                break;
            case AreaType.Trap:
                roomPieceParent.SetColor(Color.cyan);

                break;
        }
    }

    public void OnResetTransform(RoomPieceParent piece)
    {
        float rectX = UnityEngine.Random.Range(-300f, 300f);
        float rectY = UnityEngine.Random.Range(-500f, 500f);

        RectTransform rect = piece.GetComponent<RectTransform>();
        Vector2 target = new Vector2(rectX, rectY);

        StartCoroutine(ReturnPiecePosition(rect, target, 0.2f));
    }

    private IEnumerator ReturnPiecePosition(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 startPos = rect.anchoredPosition;

        float time = 0f;

        while(time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            rect.anchoredPosition = Vector2.Lerp(
                startPos,
                target,
                t);

            yield return null;

        }

        rect.anchoredPosition = target;
    }
}
