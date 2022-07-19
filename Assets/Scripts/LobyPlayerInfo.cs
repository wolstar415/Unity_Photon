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
    private static readonly int PlayerColor = Shader.PropertyToID("_PlayerColor");

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
        }
        else
        {

            Nick.text = PV.Owner.NickName;
        }
        spriteRenderer.material.SetColor(PlayerColor, ColorSystem.instance.colors[PV.Owner.ActorNumber-1]);

    }
    private void OnDestroy()
    {
        RoomNetManager.instance.Playerobs.Remove(gameObject);
    }
    public void ColorsetFunc()
    {
        spriteRenderer.material.SetColor(PlayerColor, ColorSystem.instance.colors[ColorNum]);
    }
    public void ColorSet()
    {
        ColorNum = 1000;
        foreach (var t in ColorSystem.instance.colorIdx)
        {
            CheckColor.Add(t);
        }
        foreach (var t in RoomNetManager.instance.Playerobs)
        {
            int num = t.GetComponent<LobyPlayerInfo>().ColorNum;
            if (CheckColor.Contains(num))
            {
                CheckColor.Remove(num);

            }
        }
        ColorNum=CheckColor[0];
        PV.RPC("setcolor", RpcTarget.AllBuffered, ColorSystem.instance.colors[ColorNum]);
    }



}
