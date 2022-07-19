using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove_Main : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Animator ani;
    public SpriteRenderer sr;
    public float speed = 2;
    public GameObject _camera;
    public MainPlayerInfo playerInfo;
    public Rigidbody2D rd;
    private static readonly int IsMove = Animator.StringToHash("isMove");

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        rd.velocity = Vector3.zero;
        if (PV.IsMine&&MainNetManager.instance.Intro==false)
        {
            if (playerInfo.IsIce)
            {
                ani.SetBool(IsMove, false);
                return;
            }
            Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
            if (dir.x < 0f)
            {
                PV.RPC(nameof(RPC_FlipSet), RpcTarget.AllBuffered, true);
            }
            else if(dir.x > 0f)
            {

                PV.RPC(nameof(RPC_FlipSet), RpcTarget.AllBuffered, false);
            }
            transform.position += dir * (speed * Time.deltaTime);
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") ==0)
            {
                ani.SetBool(IsMove, false);
            }
            else
            {
                ani.SetBool(IsMove, true);

            }
            
            Vector3 TargetPos = new Vector3(transform.position.x, transform.position.y, -11);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, TargetPos, Time.deltaTime * 2f);
        }
        
    }

    [PunRPC]
    void RPC_FlipSet(bool b)
    {
        sr.flipX = b;
    }
}
