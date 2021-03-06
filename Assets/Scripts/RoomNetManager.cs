using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomNetManager : MonoBehaviourPunCallbacks
{
    public Transform[] CreatePos;
    public PhotonView pv;
    public GameObject StartOb;
    public TextMeshProUGUI playerCnt;
    public GameObject playerob;
    public TMP_InputField ChatInput;
    public GameObject ChatPrefab;
    public Transform parent;
    public Scrollbar scrollbar;
    public GameObject startOb;

    public List<GameObject> Playerobs;

    public Color[] colors;
    public static RoomNetManager instance;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.CurrentRoom.IsOpen = true;
        int ran = Random.Range(0, CreatePos.Length);
        GameObject Player = PhotonNetwork.Instantiate("LobyCharacter", CreatePos[ran].position, Quaternion.identity, 0);
        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " +
                         PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        playerob = Player;
        if (PhotonNetwork.IsMasterClient)
        {
            StartOb.SetActive(true);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startOb.GetComponent<Button>().interactable = true;
        }
        else
        {
            startOb.GetComponent<Button>().interactable = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startOb.GetComponent<Button>().interactable = true;
        }
        else
        {
            startOb.GetComponent<Button>().interactable = false;
        }

        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " +
                         PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startOb.GetComponent<Button>().interactable = true;
        }
        else
        {
            startOb.GetComponent<Button>().interactable = false;
        }

        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " +
                         PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    public void StartFunc()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        pv.RPC("GamePlayer", RpcTarget.All);
    }

    public void GameExit()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("01_Loby");
    }

    #region ????

    public void Send()
    {
        if (ChatInput.text == "")
        {
            return;
        }

        ChatInput.Select();
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text, PhotonNetwork.NickName);


        ChatInput.text = "";
    }


    [PunRPC] // RPC?? ?????????? ???????? ?? ???? ???????? ????????
    void ChatRPC(string msg, string s)
    {
        GameObject ob = Instantiate(ChatPrefab, parent);
        ob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = msg;
        if (s == PhotonNetwork.LocalPlayer.NickName)
        {
            ob.transform.GetChild(0).gameObject.SetActive(true);
            ob.transform.GetChild(1).gameObject.SetActive(false);
            ob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.MidlineRight;
        }
        else
        {
            ob.transform.GetChild(0).gameObject.SetActive(false);
            ob.transform.GetChild(1).gameObject.SetActive(true);
            ob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.MidlineLeft;
        }

        scrollbar.value = 0;
    }

    [PunRPC]
    void GamePlayer()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("03_Main");
    }

    //[PunRPC]
    //void AllColorSet()
    //{
    //    for (int i = 0; i < Playerobs.Count; i++)
    //    {
    //        int num = Playerobs[i].GetComponent<LobyPlayerInfo>().ColorNum;
    //        Playerobs[i].GetComponent<LobyPlayerInfo>().spriteRenderer.material.SetColor("_PlayerColor", ColorSystem.instance.colors[num]);
    //    }
    //}

    #endregion
}