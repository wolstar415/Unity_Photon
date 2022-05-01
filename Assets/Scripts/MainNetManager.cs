using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class MainNetManager : MonoBehaviour
{
    public Transform[] CreatePos;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        int ran = Random.Range(0, CreatePos.Length);
        //GameObject Player = PhotonNetwork.Instantiate("LobyCharacter", CreatePos[ran].position, Quaternion.identity, 0);
        GameObject Player = PhotonNetwork.Instantiate("Main_Character", CreatePos[ran].position, Quaternion.identity, 0);
    }
}
