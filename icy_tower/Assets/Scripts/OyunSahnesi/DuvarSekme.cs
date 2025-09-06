using UnityEngine;

public class DuvarSekme : MonoBehaviour
{
    [Header("Sekme Ayarlarý")]
    public float temelSekmeYukseklik = 8f;  // Ýlk zýplamanýn yukarýya gücü
    public float sekmeGucu = 5f;            // Yanlara itme kuvveti
    public float comboSuresi = 5f;          // Combo için süre (5 sn)
    public float katlanmaCarpani = 0.2f;    // Her combo adýmýnda çarpanlý artýþ (20% gibi)

    private static int comboSayaci = 0;
    private static float sonZeminZamani = -999f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 carpmaNormal = collision.contacts[0].normal;

                // Combo süresini kontrol et
                if (Time.time - sonZeminZamani <= comboSuresi)
                {
                    comboSayaci++;
                }
                else
                {
                    comboSayaci = 1; // Yeni combo baþlat
                }

                // Sekme yüksekliði: katlanarak artýþ
                float sekmeYukseklik = temelSekmeYukseklik * (1f + comboSayaci * katlanmaCarpani);

                Vector3 sekmeYon = new Vector3(
                    carpmaNormal.x * sekmeGucu,
                    sekmeYukseklik,
                    0f
                );

                rb.velocity = sekmeYon;

                Debug.Log("Combo: " + comboSayaci + " | Yükseklik: " + sekmeYukseklik);
            }
        }
    }

    public static void ZemineDegdi()
    {
        sonZeminZamani = Time.time;
    }
}