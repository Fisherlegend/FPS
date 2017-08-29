﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 游戏是否结束
    /// </summary>
    private bool gameOver = false;
    /// <summary>
    /// 开火点
    /// </summary>
    public Transform firePoint;
    /// <summary>
    /// 子弹预设
    /// </summary>
    public GameObject buttle;
    /// <summary>
    /// 开火特效
    /// </summary>
    public GameObject fireEffect;
    /// <summary>
    /// 开火声音
    /// </summary>
    public AudioClip fireClip;
    /// <summary>
    /// 音频组件
    /// </summary>
    private AudioSource aduioScource;
    /// <summary>
    /// 射击CD
    /// </summary>
    private float shootCD = 0.5f;
    private float curShootTime;

	private void Start ()
    {
        aduioScource = this.GetComponent<AudioSource>();
    }
	
	
	private void Update ()
    {
        if (!gameOver)
        {
            Shoot();
        }
	}

    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot()
    {
        curShootTime += Time.deltaTime;
        if (curShootTime > shootCD)
        {
            if (Input.GetMouseButtonDown(0))
            {
                curShootTime = 0;
                //生成子弹和开火特效
                GameObject b = Instantiate(buttle) as GameObject;
                //b.transform.SetParent(firePoint);
                b.transform.position = firePoint.position;
                b.transform.eulerAngles = firePoint.eulerAngles;
                //b.AddComponent<ButtleController>();
                //b.transform.localScale = Vector3.one;
                GameObject e = Instantiate(fireEffect) as GameObject;
                e.transform.SetParent(firePoint);
                e.transform.localPosition = Vector3.zero;
                e.transform.localEulerAngles = Vector3.zero;
                e.transform.localScale = Vector3.one;
                Destroy(e, 0.3f);
                aduioScource.clip = fireClip;
                aduioScource.Play();
            }
        }
        
    }
}
