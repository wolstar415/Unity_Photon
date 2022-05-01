using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobyManager : MonoBehaviour
{
    public GameObject Loby1;
    public GameObject Online;
    public GameObject CreateRoom;
    public GameObject SelectRoom;
    public GameObject Loading;
    public TMP_InputField NickName;
    public Animator NickAni;
    public GameObject[] PlayerButton;
    public TextMeshProUGUI createNick;
    public TextMeshProUGUI selectNick;
    public Color[] color1;

    public int MaxPlayer = 2;

    public bool IsCreate = false;

    public static LobyManager instance;
    void Awake() => instance = this;

    public GameObject RoomPrefab;
    public Transform parent;
    public List<GameObject> RoomList;
    public void LobyButton()
    {
        Loby1.SetActive(false);
        Online.SetActive(true);

    }
    public void LobyExitButton()
    {
        Loby1.SetActive(true);
        Online.SetActive(false);
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void LobyCreateRoom()
    {
        if (NickName.text=="")
        {
            NickAni.Play("Act");
            return;
        }
        IsCreate = true;
        PhotonManager.instance.Connect();
        Loading.SetActive(true);
        createNick.text = NickName.text;
        MaxPlayer = 2;
        CreatePlayerset();
    }
    public void PlayerIntButton(int i)
    {
        MaxPlayer = i;
        CreatePlayerset(MaxPlayer);
    }

    public void CreatePlayerset(int Idx = 2)
    {
        
        for (int i = 0; i < PlayerButton.Length; i++)
        {
            PlayerButton[i].GetComponent<Image>().color = color1[0];
        }
            PlayerButton[MaxPlayer-2].GetComponent<Image>().color = color1[1];
    }
    public void LobyEnterRoom()
    {
        if (NickName.text == "")
        {
            NickAni.Play("Act");
            return;
        }
        IsCreate = false;
        PhotonManager.instance.Connect();
        Loading.SetActive(true);
        selectNick.text = NickName.text;


    }
    public void RoomExit()
    {

        Loading.SetActive(true);
        PhotonManager.instance.Disconnect();
    }

    public void CreateRoomButton()
    {
        PhotonManager.instance.CreateRoom();
    }

    public void RoomCreateFunc()
    {
        GameObject room = Instantiate(RoomPrefab, parent);
        RoomList.Add(room);
    }
}
