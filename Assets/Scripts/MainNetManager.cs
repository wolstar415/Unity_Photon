using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class MainNetManager : MonoBehaviourPunCallbacks,IPunObservable
{
    public static MainNetManager instance;
    public GameObject IntroOb;
    public bool Intro=true;
    public Transform PlayerCreatePos;
    public List<GameObject> PlayerOb;
    public int KillerNum = 0;
    public Color[] color;

    public GameObject KillUI;
    public GameObject NormalUI;
    public GameObject Playerob;
    public PhotonView PV;

    public Button KillBtn;
    public Button iceBtn;
    public Button useBtn;

    public GameObject GameoverOb;
    private void Awake()
    {
        instance = this;
    }
    public Transform[] CreatePos;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Intro = true;

        if (PhotonNetwork.IsMasterClient)
        {
        KillerNum = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        }
        int ran = Random.Range(0, CreatePos.Length);
        GameObject Player = PhotonNetwork.Instantiate("Main_Character", CreatePos[ran].position, Quaternion.identity, 0);
        Playerob = Player;
        Invoke("StartFunc", 1.5f);
    }

    public void IceButton()
    {
        Playerob.GetComponent<MainPlayerInfo>().IceFunc();
    }
    public void UseButton()
    {

        Playerob.GetComponent<MainPlayerInfo>().UseFunc();
    }
    public void KillButton()
    {
        Playerob.GetComponent<MainPlayerInfo>().KillFunc();
    }
    void StartFunc()
    {
        IntroOb.SetActive(false);
        Intro = false;
        // int i = PhotonNetwork.
        //  GameObject Player = PhotonNetwork.Instantiate("Main_Character", PlayerCreatePos.position, Quaternion.identity, 0);
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PlayerOb.Count; i++)
            {

                if (i == KillerNum)
                {

                    PlayerOb[i].GetComponent<MainPlayerInfo>().PV.RPC("KillOn", RpcTarget.All);
                }

            }
        }
    }

    public void GameOver()
    {
        PV.RPC("GameOverFunc", RpcTarget.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(KillerNum);
        }
        else
        {

            this.KillerNum = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void GameOverFunc()
    {
        GameoverOb.SetActive(true);
        Time.timeScale = 0f;
        Invoke("GameOverFunc2", 2f);
    }

    void GameOverFunc2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("01_Loby");
    }
}
