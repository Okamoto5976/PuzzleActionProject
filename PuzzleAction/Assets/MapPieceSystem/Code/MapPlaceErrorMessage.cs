using System.Collections.Generic;
using UnityEngine;

public enum MapPlaceErrorMessageType
{
    CountOver, //shop enemy trap etc...   can not place piece when canplaceCount over
    BossArea, //if have BossAreaPiece you have place BossArea
    NotPieceConnected, //if all piece to place not connected
    NotRouteConnected, //if not connected from start to end
}

[System.Serializable]
public class MapPlaceErrorMessageData
{
    public MapPlaceErrorMessageType Type;
    public string Message;
}

public class MapPlaceErrorMessage : MonoBehaviour
{
    [SerializeField] private List<MapPlaceErrorMessageData> m_errorList;

    public void ShowErrorMessage(MapPlaceErrorMessageType type)
    {
        switch (type)
        {
            case MapPlaceErrorMessageType.CountOver:
                

                break;
        }
    }

}
