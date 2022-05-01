using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainPlayerInfo : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI Nick;
    public PhotonView PV;
    public bool IsKiller=false;
    public bool IsIce=false;
    public Transform Pos;
    public List<GameObject> Enemy;
    public List<GameObject> friendly;
    public Button KillBtn;
    public Button iceBtn;
    public Button useBtn;
    public GameObject IceOb;
    void Awake()
    {
            MainNetManager.instance.PlayerOb.Add(gameObject);
        if (PV.IsMine)
        {

            Nick.text = PhotonNetwork.NickName;
            KillBtn = MainNetManager.instance.KillBtn;
            iceBtn = MainNetManager.instance.iceBtn;
            useBtn = MainNetManager.instance.useBtn;
        }
        else
        {

            Nick.text = PV.Owner.NickName;
        }
        
    }

    public void Killer_Colli()
    {
        KillBtn.interactable = false;
        for (int i = 0; i < Enemy.Count; i++)
        {
            if (!Enemy[i].GetComponent<MainPlayerInfo>().IsIce)
            {
                KillBtn.interactable = true;
            }
        }
    }
    public void Normal_Colli()
    {
        useBtn.interactable = false;
        for (int i = 0; i < friendly.Count; i++)
        {
            if (friendly[i].GetComponent<MainPlayerInfo>().IsIce)
            {
                useBtn.interactable = true;
            }
        }
    }
    public void UseFunc()
    {
        for (int i = 0; i < friendly.Count; i++)
        {
            if (friendly[i].GetComponent<MainPlayerInfo>().IsIce)
            {
                friendly[i].GetComponent<MainPlayerInfo>().PV.RPC("IceBye", RpcTarget.All);
            }
        }
        useBtn.interactable = false;
    }

    public void IceFunc()
    {
        if (!IsIce)
        {
            iceBtn.interactable = false;
            PV.RPC("IceGo", RpcTarget.All);
            for (int i = 0; i < friendly.Count; i++)
            {
                if (friendly[i].GetComponent<MainPlayerInfo>().IsKiller)
                {
                    friendly[i].GetComponent<MainPlayerInfo>().Killer_Colli();
                }
            }
           
        }

    }

    public void KillFunc()
    {
        for (int i = 0; i < Enemy.Count; i++)
        {
            if (!Enemy[i].GetComponent<MainPlayerInfo>().IsIce)
            {
                MainNetManager.instance.GameOver();
                return;
            }
        }
    }
    public void iceCheck()
    {
        bool b = true;
        for (int i = 0; i < MainNetManager.instance.PlayerOb.Count; i++)
        {
            if (MainNetManager.instance.PlayerOb[i].GetComponent<MainPlayerInfo>().IsIce==false&& MainNetManager.instance.PlayerOb[i].GetComponent<MainPlayerInfo>().IsKiller==false)
            {
                b = false;
            }
            
        }

        if (b)
        {
            MainNetManager.instance.GameOver();
        }
        
    }
    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (IsKiller)
                {
                    KillFunc();
                }
                else
                {
                IceFunc();

                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (!IsKiller)
                {
                   UseFunc();
                }
            }
        }
    }

    [PunRPC]
    void KillOn()
    {
        Nick.color = MainNetManager.instance.color[0];
        if (PV.IsMine)
        {

        IsKiller = true;
            MainNetManager.instance.NormalUI.SetActive(false);
            MainNetManager.instance.KillUI.SetActive(true);
        }
    }
    [PunRPC]
    void IceGo()
    {
        this.IceOb.SetActive(true);
        this.IsIce = true;
        iceCheck();

    }
    [PunRPC]
    void IceBye()
    {
        this.IceOb.SetActive(false);
        this.IsIce = false;
        iceBtn.interactable = true;
    }
    private void OnDestroy()
    {
        MainNetManager.instance.PlayerOb.Remove(gameObject);
    }

}
