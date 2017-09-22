using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trigger : MonoBehaviour {

    public int sceneIndex;

    public Text text;

    private bool isGetKey = false;

	// Use this for initialization
	void Start ()
    {

        StartCoroutine(SetText());
	}

    private IEnumerator SetText()
    {
        yield return new WaitForSeconds(3);
        text.text = "";
    }

    // Update is called once per frame
    void Update () {
		
	}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Trigger"))
    //    {
    //        SceneManager.LoadScene(2);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Trigger"))
    //    {
    //        SceneManager.LoadScene(2);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("keyDoor"))
        {
            if (isGetKey)
            {
                if (other.GetComponent<BoxCollider>().enabled)
                {
                    other.transform.parent.gameObject.GetComponent<Animation>().Play();
                    other.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(WaitEnterNextLevel());
                    other.transform.FindChild("door").GetComponent<BoxCollider>().enabled = false;
                }                           
            }          
        }
        else if (other.CompareTag("door"))
        {
            if (other.GetComponent<BoxCollider>().enabled)
            {
                other.transform.parent.gameObject.GetComponent<Animation>().Play();
                other.GetComponent<BoxCollider>().enabled = false;
                other.transform.FindChild("door").GetComponent<BoxCollider>().enabled = false;
            }                                
        }
        else if (other.CompareTag("key"))
        {
            text.text = "Get the key";
            Destroy(other.gameObject);
            StartCoroutine(SetText());
            isGetKey = true;
        }
        else if (other.CompareTag("gongzhu"))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private IEnumerator WaitEnterNextLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
