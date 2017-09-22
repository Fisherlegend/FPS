using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region 字段
    /// <summary>
    /// 单例
    /// </summary>
    public static EnemyController instance;
    /// <summary>
    /// 敌人的状态
    /// </summary>
    private EnemyState curState = EnemyState.Patrol;
    /// <summary>
    /// 玩家
    /// </summary>
    public GameObject Player;
    /// <summary>
    /// 目标对象
    /// </summary>
    private GameObject target;
    /// <summary>
    /// 最大最小生命值
    /// </summary>
    public int maxHP,minHP;
    /// <summary>
    /// 当前的生命值
    /// </summary>
    private int curHP;
    /// <summary>
    /// 最大移动速度
    /// </summary>
    public float maxSpeed = 2f;
    /// <summary>
    /// 巡逻半径
    /// </summary>
    public float radius = 10f;
    /// <summary>
    /// 初始位置
    /// </summary>
    private Vector3 initPos;
    /// <summary>
    /// 下一个巡逻位置
    /// </summary>
    private Vector3 nextPatrolPos;
    /// <summary>
    /// 上次搜寻敌人时间
    /// </summary>
    private float lastSearchTargetTime = 0;
    /// <summary>
    /// 搜寻间隔
    /// </summary>
    private float searchTargetInterval = 2;
    /// <summary>
    /// 视野范围 在此范围内追随敌人
    /// </summary>
    public float sightDistance = 50;
    /// <summary>
    /// 开始攻击敌人距离
    /// </summary>
    public float attackDistance = 30;
    /// <summary>
    /// 动画组件
    /// </summary>
    private Animation anim;
    /// <summary>
    /// 是否搜寻目标
    /// </summary>
    private bool IsSearchTarget = true;
    /// <summary>
    /// 是否正在攻击中
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool isDead = false;
    #endregion


    private void Start()
    {
        instance = this;
        initPos = this.transform.position;
        GetNxetPatrolPos();
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponent<Animation>();
        curHP = UnityEngine.Random.Range(minHP, maxHP);
   }   

    private void Update()
    {
        if (isDead)
            return;
        TargetUpdate();
        DoCurrentState();
    }

    /// <summary>
    /// 获得巡逻的下一个位置
    /// </summary>
    public void GetNxetPatrolPos()
    {
        Vector2 v = UnityEngine.Random.insideUnitCircle * 10;
        nextPatrolPos = initPos + new Vector3(v.x, 0, v.y);     
    }

    /// <summary>
    /// 搜寻目标
    /// </summary>
    private void TargetUpdate()
    {
        if (!IsSearchTarget)
            return;
        lastSearchTargetTime += Time.deltaTime;
        if (searchTargetInterval > lastSearchTargetTime)
            return;
        lastSearchTargetTime = 0;
        if (Vector3.Distance(this.transform.position, Player.transform.position) < sightDistance)
            HasTarget();
        else
            NoTarget();
    }

    /// <summary>
    /// 发现目标
    /// </summary>
    private void HasTarget()
    {
        if(Vector3.Distance(this.transform.position, Player.transform.position) < attackDistance)
        {
            curState = EnemyState.Attack;
            maxSpeed = 0;
            //Debug.Log("Attact");
        }          
        else
        {
            curState = EnemyState.Follow;
            maxSpeed = 4;
            //Debug.Log("Follow");
        }         
    }
    /// <summary>
    /// 没发现目标
    /// </summary>
    private void NoTarget()
    {
        curState = EnemyState.Patrol;
        maxSpeed = 2;
    }

    /// <summary>
    /// 执行当前的状态
    /// </summary>
    private void DoCurrentState()
    {
        switch (curState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Escape:
                Escape();
                break;
            case EnemyState.Follow:
                Follow();
                break;
        }
    }

    /// <summary>
    /// 巡逻
    /// </summary>
    private void Patrol()
    {
        if (!anim.IsPlaying("Walk"))
            anim.Play("Walk");
        this.transform.LookAt(nextPatrolPos);
        this.transform.position = Vector3.MoveTowards(this.transform.position, nextPatrolPos, Time.deltaTime * maxSpeed);
        float dis = Vector3.Distance(this.transform.position, nextPatrolPos);

        if (dis < 0.6f)
            GetNxetPatrolPos();
    }

    /// <summary>
    ///攻击
    /// </summary>
    private void Attack()
    {
        this.transform.LookAt(Player.transform);
        if (isAttack)
            return;
        
        isAttack = true;
        anim.Play("Reloading");      
        StartCoroutine(WaitShootFinish(anim.GetClip("Reloading").length));
    }

    /// <summary>
    /// 等待设计完成
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitShootFinish(float t)
    {
        yield return new WaitForSeconds(t + 0.5f);

        this.GetComponent<AudioSource>().Play();
        yield return  anim.Play("Shoot");

        yield return new WaitForSeconds(2f);
        float range = UnityEngine.Random.Range(0.0f, 1.0f);
        
        if (range < 0.3f)
        {
            PlayerController.instance.BeHit(10);
            Debug.Log("****被敌人击中");
        }
          
        isAttack = false;
    }

    /// <summary>
    /// 跟随敌人
    /// </summary>
    private void Follow()
    {
        if(!anim.IsPlaying("Run"))
            anim.Play("Run");
        this.transform.LookAt(Player.transform);
        Vector3 pos = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, pos, Time.deltaTime * maxSpeed);
    }

    /// <summary>
    /// 逃跑
    /// </summary>
    private void Escape()
    {

    }

    public void ReduceHP()
    {
        if (isDead)
            return;
        curHP--;
        if (curHP <= 0)
        {
            isDead = true;
            StopAllCoroutines();
            anim.Play("Fall");
            StartCoroutine(WaitDead(anim.GetClip("Fall").length));            
        }
    }
    /// <summary>
    /// 等待死亡
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitDead(float t)
    {
        yield return new WaitForSeconds(t);
        this.transform.position = new Vector3(this.transform.position.x, -38.5f,this.transform.position.z);
        Destroy(this.gameObject,1);
    }
}

/// <summary>
/// 敌人状态
/// </summary>
public enum EnemyState
{
    /// <summary>
    /// 巡逻
    /// </summary>
    Patrol,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 追随敌人
    /// </summary>
    Follow,
    /// <summary>
    /// 逃跑
    /// </summary>
    Escape,
}