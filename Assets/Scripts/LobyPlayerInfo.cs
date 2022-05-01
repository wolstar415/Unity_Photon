using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobyPlayerInfo : MonoBehaviourPunCallbacks,IPunObservable
{
    public TextMeshProUGUI Nick;
    public PhotonView PV;
    public int ColorNum;
    public List<int> CheckColor;
    public SpriteRenderer spriteRenderer;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ColorNum);

        }
        else
        {
            this.ColorNum = (int)stream.ReceiveNext();

        }
    }

    void Awake()
    {
        RoomNetManager.instance.Playerobs.Add(gameObject);
        if (PV.IsMine)
        {

            Nick.text = PhotonNetwork.NickName;
            //int zz = PhotonNetwork.CurrentRoom.PlayerCount - 1;
            //ColorNum = zz;
            //ColorSystem.instance.color = ColorSystem.instance.colors[ColorNum];
            
            //PV.RPC("setcolor", RpcTarget.All);
        }
        else
        {

            Nick.text = PV.Owner.NickName;
        }
        spriteRenderer.material.SetColor("_PlayerColor", ColorSystem.instance.colors[PV.Owner.ActorNumber-1]);
        //    Debug.Log(PV.Owner.ActorNumber);
        //spriteRenderer.material.SetColor("_PlayerColor", ColorSystem.instance.colors[ColorNum]);
        //Invoke("ColorsetFunc", 0.5f);
        //ColorSet();


    }
    private void OnDestroy()
    {
        RoomNetManager.instance.Playerobs.Remove(gameObject);
    }
    public void ColorsetFunc()
    {
        spriteRenderer.material.SetColor("_PlayerColor", ColorSystem.instance.colors[ColorNum]);
    }
    public void ColorSet()
    {
        ColorNum = 1000;
        for (int i = 0; i < ColorSystem.instance.colorIdx.Count; i++)
        {
            CheckColor.Add(ColorSystem.instance.colorIdx[i]);
        }
        for (int i = 0; i < RoomNetManager.instance.Playerobs.Count; i++)
        {
            int num = RoomNetManager.instance.Playerobs[i].GetComponent<LobyPlayerInfo>().ColorNum;
            if (CheckColor.Contains(num))
            {
            CheckColor.Remove(num);

            }
        }
       ColorNum=CheckColor[0];
        //Debug.Log(ColorNum);
        PV.RPC("setcolor", RpcTarget.AllBuffered, ColorSystem.instance.colors[ColorNum]);
    }



}
