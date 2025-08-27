using System.Collections.Generic;
using UnityEngine;

public class SonsuzDuvar : MonoBehaviour
{
    [Header("Duvar Ayarlarý")]
    public GameObject duvarPrefab;              // Üretilecek duvar prefab'i
    public int baslangicSegmentSayisi = 10;    // Baþlangýçta kaç segment oluþturulacak
    public float uretilmeAraligi = 20f;         // Kaç saniyede bir yeni segment üretilecek

    [Header("Referanslar")]
    public Transform[] baslangicDuvarlar;      // Sol ve sað duvarlarýn referansý

    private Dictionary<Transform, GameObject> sonDuvarlar = new Dictionary<Transform, GameObject>();
    private float duvarHeight;
    private float zamanSayaci = 2f;

    void Start()
    {
        if (baslangicDuvarlar == null || baslangicDuvarlar.Length == 0 || duvarPrefab == null)
        {
            UnityEngine.Debug.LogError("BaslangicDuvarlar veya duvarPrefab atanmamis!");
            enabled = false;
            return;
        }

        duvarHeight = duvarPrefab.GetComponent<Renderer>().bounds.size.y;

        // Baþlangýç segmentlerini oluþtur
        foreach (var duvar in baslangicDuvarlar)
        {
            GameObject son = duvar.gameObject;
            sonDuvarlar[duvar] = son;

            for (int i = 1; i <= baslangicSegmentSayisi; i++)
            {
                son = EkleYeniDuvar(son.transform);
                sonDuvarlar[duvar] = son;
            }
        }
    }

    void Update()
    {
        zamanSayaci += Time.deltaTime;

        if (zamanSayaci >= uretilmeAraligi)
        {
            foreach (var duvar in baslangicDuvarlar)
            {
                GameObject son = sonDuvarlar[duvar];
                GameObject yeni = EkleYeniDuvar(son.transform);
                sonDuvarlar[duvar] = yeni;
            }
            zamanSayaci = 0f;
        }
    }

    // Verilen duvarýn üstüne yeni bir segment ekle
    GameObject EkleYeniDuvar(Transform sonDuvar)
    {
        Vector3 pozisyon = new Vector3(
            sonDuvar.position.x,
            sonDuvar.position.y + duvarHeight,
            sonDuvar.position.z
        );

        GameObject yeniDuvar = Instantiate(duvarPrefab, pozisyon, sonDuvar.rotation);
        return yeniDuvar;
    }
}
