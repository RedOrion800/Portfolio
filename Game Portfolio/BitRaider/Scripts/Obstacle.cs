using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{

    public Animator transitionAnimator;

    private void Awake()
    {
        transitionAnimator = GameObject.Find("Canvas").GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Die(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Die(collision.gameObject);
        }
    }

    private void Die(GameObject player)
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraFollow>().enabled = false;
        Destroy(player);
        int livesLeft = PlayerPrefs.GetInt("LivesLeft");
        livesLeft--;
        if(livesLeft == 0)
        {
            StartCoroutine(LoadLevel(0));
        }
        else
        {
            PlayerPrefs.SetInt("LivesLeft", livesLeft);
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        }
    }

    IEnumerator LoadLevel(int index)
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }
}
