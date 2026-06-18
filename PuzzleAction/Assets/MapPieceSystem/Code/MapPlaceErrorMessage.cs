using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private GameObject m_panel;
    [SerializeField] private TextMeshProUGUI m_errorText;

    public void ShowErrorMessage(MapPlaceErrorMessageType type)
    {

        foreach(var error in m_errorList)
        {
            if(error.Type == type)
            {
                m_errorText.text = error.Message;
            }
        }

        OnShowErrorMessage();

    }

    private void OnShowErrorMessage()
    {
        m_panel.SetActive(true);
        Invoke(nameof(OnHideErrorMessage), 2f);
    }

    private void OnHideErrorMessage()
    {
        m_panel.SetActive(false);
    }
}
