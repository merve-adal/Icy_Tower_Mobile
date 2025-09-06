using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterHareket : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float hareketHizi = 5f;
    public float ziplamaGucu = 11f;

    private float yatayHareket;
    private bool yerdeMi;

    Rigidbody rb;
    Animator animator;

    [Header("Duvar Sekme Ayarları")]
    public float duvarSekmeTemel = 3f;
    public float duvarSekmeYan = 0.5f;
    public float comboSuresi = 3f;
    public float katlanmaCarpani = 0.1f;

    private float sonDuvarZiplamaZamani = -999f;
    private int comboSayaci = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Oyun başında karakterin Y hızını sıfırla
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Başlangıçta yere temas kontrolü
        yerdeMi = true;
    }

    void Update()
    {
        // Input okuma
        yatayHareket = Input.GetAxis("Horizontal");

        // Karakter yönünü çevir
        if (yatayHareket > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (yatayHareket < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);

        // Yürüme animasyonu
        animator.SetBool("Yurume", Mathf.Abs(yatayHareket) > 0f);

        // Normal zıplama
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi)
        {
            rb.velocity = new Vector3(rb.velocity.x, ziplamaGucu, rb.velocity.z);
            animator.SetBool("Ziplama", true);
            yerdeMi = false;
        }

        // Combo süresi
        float kalanSure = comboSuresi - (Time.time - sonDuvarZiplamaZamani);
        if (kalanSure > 0)
        {
            UnityEngine.Debug.Log("Combo: " + comboSayaci + " | Kalan Süre: " + kalanSure.ToString("F2") + " sn");
        }
    }

    void FixedUpdate()
    {
        // Sadece X ekseninde hareket etsin
        rb.velocity = new Vector3(yatayHareket * hareketHizi, rb.velocity.y, 0f);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Zemin ile temas
        if (collision.gameObject.CompareTag("Zemin"))
        {
            yerdeMi = true;
            animator.SetBool("Ziplama", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Duvar ile temas
        if (collision.gameObject.CompareTag("Duvar"))
        {
            if (Time.time - sonDuvarZiplamaZamani <= comboSuresi)
                comboSayaci++;
            else
                comboSayaci = 1;

            float sekmeYukseklik = duvarSekmeTemel * Mathf.Pow(1 + katlanmaCarpani, comboSayaci - 1);

            Vector3 carpmaNormal = collision.contacts[0].normal;
            Vector3 sekmeYon = new Vector3(
                carpmaNormal.x * duvarSekmeYan,
                sekmeYukseklik,
                0f
            );

            rb.velocity = sekmeYon;
            sonDuvarZiplamaZamani = Time.time;

            UnityEngine.Debug.Log("Duvara Sekme! Combo: " + comboSayaci + " | Sekme Yüksekliği: " + sekmeYukseklik.ToString("F2"));
        }
    }
}
