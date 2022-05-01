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
        Player.GetComponent<LobyPlayerInfo>().Nick.text = PhotonNetwork.LocalPlayer.NickName;
        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            StartOb.SetActive(true);

        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {


        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

    }

    public void GameExit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("01_Loby");
    }

    #region 채팅
    public void Send()
    {
        if (ChatInput.text=="")
        {
            return;
        }
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
        ChatInput.Select();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send();
        }
        

    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        GameObject ob = Instantiate(ChatPrefab, parent);
        ob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = msg;
        if (pv.IsMine)
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
    #endregion
}
