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

    [Header("Zemin Kontrolü")]
    public Transform ayakNoktasi;
    public float kontrolYaricapi = 0.2f;
    public LayerMask zeminKatmani;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        yatayHareket = Input.GetAxis("Horizontal");

        // Karakter yönü
        if (yatayHareket > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (yatayHareket < 0) transform.rotation = Quaternion.Euler(0, -90, 0);

        animator.SetBool("Yurume", Mathf.Abs(yatayHareket) > 0f);

        // Zemin kontrolü
        yerdeMi = Physics.CheckSphere(ayakNoktasi.position, kontrolYaricapi, zeminKatmani);

        // Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi)
        {
            rb.velocity = new Vector3(rb.velocity.x, ziplamaGucu, rb.velocity.z);
            animator.SetBool("Ziplama", true);
        }
        else if (yerdeMi)
        {
            animator.SetBool("Ziplama", false);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(yatayHareket * hareketHizi, rb.velocity.y, 0f);
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }

    void OnDrawGizmosSelected()
    {
        if (ayakNoktasi == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ayakNoktasi.position, kontrolYaricapi);
    }
}
