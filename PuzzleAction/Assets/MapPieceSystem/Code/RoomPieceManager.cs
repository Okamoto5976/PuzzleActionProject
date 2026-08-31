using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPieceManager : MonoBehaviour
{
    [SerializeField] private GameObject m_MapPieceUI;

    [SerializeField] private GameObject m_roomPieceParent;
    [SerializeField] private GameObject m_roomPiece;

    private Queue<RoomPieceParent> m_pieces = new();

    [SerializeField] private int m_pieceAmount = 20;
    //[SerializeField] private int m_poolPieceAmount = 100;

    //[SerializeField] private int m_normalRoomGenerate = 40;
    //[SerializeField] private int m_enemyRoomGenerate = 20;
    //[SerializeField] private int m_shopRoomGenerate = 20;
    //[SerializeField] private int m_trapRoomGenerate = 20;

    //[SerializeField] private IntRunTime m_level;

    public void Start()
    {
        for(int i = 0; i < m_pieceAmount; i++)
        {
            Room room = CreateRoom();
            RoomPieceParent piece= GenerateRoomObject(room);
            //m_pieces.Enqueue(piece);
        }

        if(GameManager.Instance.Level % 5 == 0)
        {
            Room bossroom = CreateBossRoom();
            RoomPieceParent bossPiece = GenerateBossRoomObject(bossroom);
        }
       
        
    }

    #region ƒ‹[ƒ€ì¬
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

    private Room CreateBossRoom()
    {
        Room room = new(new(), new(0, 0));


        room = new(
            new()
            {
                    Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,
            }, new(2, 2)
            );


        return room;
    }

    #endregion


    //map piece generate
    public RoomPieceParent GenerateRoomObject(Room room)
    {
        GameObject parentObj = Instantiate(m_roomPieceParent, m_MapPieceUI.transform);

        float rectX = UnityEngine.Random.Range(-300f, 300f);
        float rectY = UnityEngine.Random.Range(-500f, 500f);

        RectTransform rect = parentObj.gameObject.GetComponent<RectTransform>();

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

        switch (type)
        {
            case AreaType.None:
                break;
            case AreaType.Summon:
                roomPieceParent.SetColor(Color.red);
                break;
            case AreaType.Shop:
                roomPieceParent.SetColor(Color.green);
                break;
            case AreaType.Damage:
                roomPieceParent.SetColor(Color.cyan);

                break;
        }

        return roomPieceParent;
    }

    public RoomPieceParent GenerateBossRoomObject(Room room)
    {
        GameObject parentObj = Instantiate(m_roomPieceParent, m_MapPieceUI.transform);

        float rectX = UnityEngine.Random.Range(-300f, 300f);
        float rectY = UnityEngine.Random.Range(-500f, 500f);

        RectTransform rect = parentObj.gameObject.GetComponent<RectTransform>();

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
        AreaType type = AreaType.Boss;

        roomPieceParent.SetAreaType(type);
        roomPieceParent.Init(this);

        roomPieceParent.SetColor(Color.magenta);

        return roomPieceParent;
    }

    public void ResetRoomPiece()
    {
        //all delete
        //UIPanel right slide
        //show 20piece
    }

    private void ShowRoomObject()
    {
        //get from queue
        RoomPieceParent piece = m_pieces.Dequeue();

        //random
        float rectX = UnityEngine.Random.Range(-300f, 300f);
        float rectY = UnityEngine.Random.Range(-500f, 500f);

        RectTransform rect = piece.gameObject.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(rectX, rectY);
    }

    public void OnResetTransform(RoomPieceParent piece, Vector2 pos)
    {
        //float rectX = UnityEngine.Random.Range(-300f, 300f);
        //float rectY = UnityEngine.Random.Range(-500f, 500f);

        RectTransform rect = piece.GetComponent<RectTransform>();
        //Vector2 target = new Vector2(rectX, rectY);
        Vector2 target = pos;

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
