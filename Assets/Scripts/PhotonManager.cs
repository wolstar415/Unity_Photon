using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;
    public PhotonView PV;


    List<RoomInfo> myList = new List<RoomInfo>();


    public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
    {
        for (int i = 0; i < LobyManager.instance.RoomList.Count; i++)
        {
            Destroy(LobyManager.instance.RoomList[i]);
        }

        LobyManager.instance.RoomList.Clear();
        
        //방을 초기화합니다.
        
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            GameObject room = Instantiate(LobyManager.instance.RoomPrefab, LobyManager.instance.parent);
            var roomInfo = room.GetComponent<RoomInfo>();
            roomInfo.idx = i;
            roomInfo.name = roomList[i].Name;
            roomInfo.Roomname.text = roomList[i].Name;
            roomInfo.PlayerCnt.text =
                roomList[i].PlayerCount.ToString() + " / " + roomList[i].MaxPlayers;
            if (roomList[i].PlayerCount >= roomList[i].MaxPlayers || roomList[i].IsOpen == false)
            {
                room.SetActive(false);
            }

            LobyManager.instance.RoomList.Add(room);
        }
    }

    public void RoomJoin(string name)
    {
        LobyManager.instance.Loading.SetActive(true);
        PhotonNetwork.JoinRoom(name);
    }


    #region 서버연결

    void Awake() => instance = this;


    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        if (LobyManager.instance.IsCreate == false)
        {
            LobyManager.instance.Online.SetActive(false);
            LobyManager.instance.SelectRoom.SetActive(true);
            LobyManager.instance.Loading.SetActive(false);
            PhotonNetwork.LocalPlayer.NickName = LobyManager.instance.NickName.text;
        }
        else
        {
            LobyManager.instance.Online.SetActive(false);
            LobyManager.instance.CreateRoom.SetActive(true);
            LobyManager.instance.Loading.SetActive(false);
            PhotonNetwork.LocalPlayer.NickName = LobyManager.instance.NickName.text;
        }
    }

    public override void OnJoinedLobby()
    {
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobyManager.instance.Online.SetActive(true);
        LobyManager.instance.CreateRoom.SetActive(false);
        LobyManager.instance.SelectRoom.SetActive(false);
        LobyManager.instance.Loading.SetActive(false);
    }

    #endregion


    #region 방

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(LobyManager.instance.NickName.text,
            new RoomOptions { MaxPlayers = (byte)LobyManager.instance.MaxPlayer });
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.Instantiate("asd", Vector3.zero, Quaternion.identity);
        LobyManager.instance.Online.SetActive(false);
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("02_ReadyRoom");
        //return;
        //RoomPanel.SetActive(true);
        //RoomRenewal();
        //ChatInput.text = "";
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        LobyManager.instance.NickName.text += Random.Range(0, 100);
        CreateRoom();
    }

    //public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        LobyManager.instance.Loading.SetActive(false);
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //}

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //}

    #endregion
}