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

    //private void Awake()
    //{
    //    PhotonNetwork.ConnectUsingSettings();
    //}
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        int ran = Random.Range(0, CreatePos.Length);
        //GameObject Player = PhotonNetwork.Instantiate("LobyCharacter", CreatePos[ran].position, Quaternion.identity);
        GameObject Player = PhotonNetwork.Instantiate("LobyCharacter", CreatePos[ran].position, Quaternion.identity,0);
        //Player.GetComponent<LobyPlayerInfo>().Nick.text = PhotonNetwork.LocalPlayer.NickName;
        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        playerob = Player;
        if (PhotonNetwork.IsMasterClient)
        {
            StartOb.SetActive(true);

        }
        //PhotonNetwork.IsMessageQueueRunning = false;
        //SceneManager.LoadScene("03_Main");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount>=2)
        {
            startOb.GetComponent<Button>().interactable = true;
        }
        else
        {
            startOb.GetComponent<Button>().interactable = false;

        }

        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

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
        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

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
    #region 채팅
    public void Send()
    {
        if (ChatInput.text=="")
        {
            return;
        }
        ChatInput.Select();
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text, PhotonNetwork.NickName);



        ChatInput.text = "";
    }


    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg,string s)
    {
        GameObject ob = Instantiate(ChatPrefab, parent);
        ob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = msg;
        if (s==PhotonNetwork.LocalPlayer.NickName)
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
    #endregion
}
