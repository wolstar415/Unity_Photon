using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    public string name;
    public int idx;
    public TextMeshProUGUI Roomname;
    public TextMeshProUGUI PlayerCnt;
    
    public void ClickRoom()
    {
        PhotonManager.instance.RoomJoin(name);
    }
}
