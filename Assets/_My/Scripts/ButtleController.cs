using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleController : MonoBehaviour
{
    /// <summary>
    /// 子弹速度
    /// </summary>
    public float speed = 60;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        this.transform.Translate(this.transform.forward * speed * Time.deltaTime,Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log(other.name + " beHit");
            other.GetComponent<EnemyController>().ReduceHP();
            Destroy(this.gameObject);
        }
    }
}
