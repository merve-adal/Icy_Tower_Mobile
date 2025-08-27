using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterHareket : MonoBehaviour
{
    public float hareketHizi = 5f;
    public float ziplamaGucu = 5f;
    private float yatayHareket;

    private bool yerdeMi;
    Rigidbody rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        yatayHareket = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(yatayHareket * hareketHizi, rb.velocity.y, rb.velocity.z);

        if (yatayHareket > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (yatayHareket < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);

        animator.SetBool("Yurume", Mathf.Abs(yatayHareket) > 0f);

        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi)
        {
            rb.velocity = new Vector3(rb.velocity.x, ziplamaGucu, rb.velocity.z);
            animator.SetBool("Ziplama", true);
            yerdeMi = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zemin"))
        {
            yerdeMi = true;
            animator.SetBool("Ziplama", false);
        }
    }
}
