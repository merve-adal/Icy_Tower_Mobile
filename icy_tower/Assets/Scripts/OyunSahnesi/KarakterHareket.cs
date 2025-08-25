using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterHareket : MonoBehaviour
{
    public float hareketHizi = 5f;     // Sağ-sol hareket hızı
    public float ziplamaGucu = 5f;     // Zıplama yüksekliği
    private float yatayHareket;        // Input (-1,0,1)

    private bool yerdeMi;              // Karakter zeminde mi?

    Rigidbody rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Klavyeden X eksenindeki hareketi al
        yatayHareket = Input.GetAxis("Horizontal");

        // Rigidbody hızını ayarla
        rb.velocity = new Vector3(yatayHareket * hareketHizi, rb.velocity.y, rb.velocity.z);

        // Karakterin sağa/sola bakmasını sağla
        if (yatayHareket > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (yatayHareket < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);

        // Yürüme animasyonu için kontrol
        if (Mathf.Abs(yatayHareket) > 0f) // sağ veya sol tuşuna basılıysa
            animator.SetBool("Yurume", true);
        else
            animator.SetBool("Yurume", false);

        // Space ile zıplama
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi)
        {
            rb.velocity = new Vector3(rb.velocity.x, ziplamaGucu, rb.velocity.z);
            animator.SetBool("Ziplama", true);
            yerdeMi = false;
        }
    }

    // Zemin kontrolü
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zemin"))
        {
            yerdeMi = true;
            animator.SetBool("Ziplama", false);
        }
    }
}
