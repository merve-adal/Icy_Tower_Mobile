using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterHareket : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float hareketHizi = 5f;
    public float ziplamaGucu = 5f;

    private float yatayHareket;
    private bool yerdeMi;

    Rigidbody rb;
    Animator animator;

    [Header("Duvar Sekme Ayarları")]
    public float duvarSekmeTemel = 8f;    // Temel yukarı zıplama
    public float duvarSekmeYan = 3f;      // Yanlara itme kuvveti
    public float comboSuresi = 5f;        // Combo süresi saniye
    public float katlanmaCarpani = 0.2f;  // Katlanma çarpanı (20%)

    private float sonDuvarZiplamaZamani = -999f; // sadece duvardan zıpladığında güncellenecek
    private int comboSayaci = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Karakter yatay hareket
        yatayHareket = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(yatayHareket * hareketHizi, rb.velocity.y, rb.velocity.z);

        if (yatayHareket > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (yatayHareket < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);

        animator.SetBool("Yurume", Mathf.Abs(yatayHareket) > 0f);

        // Normal zıplama
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi)
        {
            rb.velocity = new Vector3(rb.velocity.x, ziplamaGucu, rb.velocity.z);
            animator.SetBool("Ziplama", true);
            yerdeMi = false;
        }

        // Combo süresi yalnızca duvardan zıplama sonrası say
        float kalanSure = comboSuresi - (Time.time - sonDuvarZiplamaZamani);
        if (kalanSure > 0)
        {
            UnityEngine.Debug.Log("Combo: " + comboSayaci + " | Kalan Süre: " + kalanSure.ToString("F2") + " sn");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Zemin ile temas
        if (collision.gameObject.CompareTag("Zemin"))
        {
            yerdeMi = true;
            animator.SetBool("Ziplama", false);
        }

        // Duvar ile temas
        if (collision.gameObject.CompareTag("Duvar"))
        {
            // Combo süresini kontrol et
            if (Time.time - sonDuvarZiplamaZamani <= comboSuresi)
            {
                comboSayaci++;
            }
            else
            {
                comboSayaci = 1;
            }

            // Katlanarak artan sekme yüksekliği
            float sekmeYukseklik = duvarSekmeTemel * Mathf.Pow(1 + katlanmaCarpani, comboSayaci - 1);

            Vector3 carpmaNormal = collision.contacts[0].normal;
            Vector3 sekmeYon = new Vector3(
                carpmaNormal.x * duvarSekmeYan, // Yan kuvvet
                sekmeYukseklik,
                0f
            );

            rb.velocity = sekmeYon;

            // Duvardan zıplama anında süre başlat
            sonDuvarZiplamaZamani = Time.time;

            UnityEngine.Debug.Log("Duvara Sekme! Combo: " + comboSayaci + " | Sekme Yüksekliği: " + sekmeYukseklik.ToString("F2"));
        }
    }
}
