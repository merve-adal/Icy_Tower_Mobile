using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DuvarZiplama : MonoBehaviour
{
    [Header("Sekme Ayarlarý")]
    public float temelSekmeYukseklik = 8f;   // Ýlk zýplamanýn yukarýya gücü
    public float yataySekme = 2f;            // Yanlara itme
    public float comboSuresi = 3f;           // Combo süresi
    public float katlanmaCarpani = 0.2f;     // Her art arda sekmede artýþ

    private Rigidbody rb;
    private int comboSayaci = 0;
    private float sonZiplamaZamani = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Duvar"))
        {
            float zamanFarki = Time.time - sonZiplamaZamani;
            if (zamanFarki <= comboSuresi)
                comboSayaci++;
            else
                comboSayaci = 1;

            float yukseklik = temelSekmeYukseklik * Mathf.Pow(1 + katlanmaCarpani, comboSayaci - 1);

            Vector3 carpmaNormal = collision.contacts[0].normal;
            float xYon = -Mathf.Sign(carpmaNormal.x) * yataySekme;

            rb.velocity = new Vector3(xYon, yukseklik, 0f);
            sonZiplamaZamani = Time.time;

            Debug.Log($"Duvara Sekme! Combo: {comboSayaci}, Yukseklik: {yukseklik:F2}");
        }
    }

    public void ZemineDegdi()
    {
        comboSayaci = 0;
    }
}
