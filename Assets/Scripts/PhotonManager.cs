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
    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;



    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;



    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        //if (num == -2) --currentPage;
        //else if (num == -1) ++currentPage;
        //else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        //MyListRenewal();
    }

    void MyListRenewal()
    {
        //// 최대페이지
        //maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        //// 이전, 다음버튼
        //PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        //NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        //// 페이지에 맞는 리스트 대입
        //multiple = (currentPage - 1) * CellBtn.Length;
        //for (int i = 0; i < CellBtn.Length; i++)
        //{
        //    CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
        //    CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
        //    CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        //}
    }

    public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
    {
        for (int i = 0; i < LobyManager.instance.RoomList.Count; i++)
        {
            Destroy(LobyManager.instance.RoomList[i]);
        }
        LobyManager.instance.RoomList.Clear();
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            GameObject room = Instantiate(LobyManager.instance.RoomPrefab, LobyManager.instance.parent);
            room.GetComponent<RoomInfo>().idx = i;
            room.GetComponent<RoomInfo>().name = roomList[i].Name;
            room.GetComponent<RoomInfo>().Roomname.text = roomList[i].Name;
            room.GetComponent<RoomInfo>().PlayerCnt.text = roomList[i].PlayerCount.ToString()+" / "+roomList[i].MaxPlayers;
            if (roomList[i].PlayerCount >= roomList[i].MaxPlayers || roomList[i].IsOpen==false)
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
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    for (int i = 0; i < LobyManager.instance.RoomList.Count; i++)
    //    {
    //        Destroy(LobyManager.instance.RoomList[i]);
    //    }
    //    LobyManager.instance.RoomList.Clear();
    //    int roomCount = roomList.Count;
    //    for (int i = 0; i < roomCount; i++)
    //    {
    //        GameObject room = Instantiate(LobyManager.instance.RoomPrefab, LobyManager.instance.parent);
    //        LobyManager.instance.RoomList.Add(room);
    //    }
    //    return;
    //    //int roomCount = roomList.Count;
    //    //for (int i = 0; i < roomCount; i++)
    //    //{
    //    //    if (!roomList[i].RemovedFromList)
    //    //    {
    //    //        if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
    //    //        else myList[myList.IndexOf(roomList[i])] = roomList[i];
    //    //    }
    //    //    else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
    //    //}


    //    //MyListRenewal();
    //}
    #endregion


    #region 서버연결
    void Awake() => instance = this;

    void Update()
    {
       // StatusText.text = PhotonNetwork.NetworkClientState.ToString();
       // LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        if (LobyManager.instance.IsCreate==false)
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
        //LobbyPanel.SetActive(true);
        //RoomPanel.SetActive(false);
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        //myList.Clear();
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
        PhotonNetwork.CreateRoom(LobyManager.instance.NickName.text, new RoomOptions { MaxPlayers = (byte)LobyManager.instance.MaxPlayer });
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

    public override void OnCreateRoomFailed(short returnCode, string message) { LobyManager.instance.NickName.text += Random.Range(0,100); CreateRoom(); }

    //public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        LobyManager.instance.Loading.SetActive(false);

    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    RoomRenewal();
    //    ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    //}

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    RoomRenewal();
    //    ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    //}

    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    }
    #endregion


}
