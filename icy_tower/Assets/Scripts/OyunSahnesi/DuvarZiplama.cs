using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DuvarZiplama : MonoBehaviour
{
    [Header("Sekme Ayarlarý")]
    public float temelSekmeYukseklik = 8f;   // Ýlk zýplamanýn yukarýya gücü
    public float yataySekme = 2f;            // Yanlara itme
    public float comboSuresi = 3f;           // Combo süresi
    public float katlanmaCarpani = 0.2f;     // Her art arda sekmede artýþ (%20)

    private Rigidbody rb;
    private int comboSayaci = 0;
    private float sonZiplamaZamani = -999f;
    private float oncekiYukseklik;          // Önceki sekmenin dikey yüksekliði

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        oncekiYukseklik = temelSekmeYukseklik;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Duvar"))
        {
            float zamanFarki = Time.time - sonZiplamaZamani;

            if (zamanFarki <= comboSuresi)
            {
                comboSayaci++;
                // Önceki sekmenin üzerine %20 ekle
                oncekiYukseklik = oncekiYukseklik * (1f + katlanmaCarpani);
            }
            else
            {
                comboSayaci = 1;
                oncekiYukseklik = temelSekmeYukseklik; // combo sýfýrlandý
            }

            float yukseklik = oncekiYukseklik;

            Vector3 carpmaNormal = collision.contacts[0].normal;
            float xYon = -Mathf.Sign(carpmaNormal.x) * yataySekme;

            rb.velocity = new Vector3(xYon, yukseklik, 0f);
            sonZiplamaZamani = Time.time;

            UnityEngine.Debug.Log($"Duvara Sekme! Combo: {comboSayaci}, Yukseklik: {yukseklik:F2}");
        }
    }

    public void ZemineDegdi()
    {
        comboSayaci = 0;
        oncekiYukseklik = temelSekmeYukseklik;
    }
}