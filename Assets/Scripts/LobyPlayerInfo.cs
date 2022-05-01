using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobyPlayerInfo : MonoBehaviour
{
    public TextMeshProUGUI Nick;
    public PhotonView PV;
     void Awake()
    {
        RoomNetManager.instance.Playerobs.Add(gameObject);
        if (PV.IsMine)
        {

            Nick.text = PhotonNetwork.NickName;
        }
        else
        {

            Nick.text = PV.Owner.NickName;
        }
    }
}
