using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething}
    public GameObject Parent;
    public Rigidbody2D Rbody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public BirdState State { get { return _state; } }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        Rbody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    private void FixedUpdate()
    {
        if(_state == BirdState.Idle && Rbody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if((_state == BirdState.Thrown || _state == BirdState.HitSomething) && Rbody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            // Hancurkan gameobject setelah 2 detik
            // jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        Rbody.bodyType = RigidbodyType2D.Dynamic;
        Rbody.velocity = velocity * speed * distance;

        OnBirdShot(this);
    }

    void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething)
            OnBirdDestroyed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
    }

    public virtual void OnTap()
    {
        //DO Nothing
    }
}

// Note

/* Pada method Start, kita akan mematikan fungsi physics dan collider dari burung.
 * 
 * Method FixedUpdate merupakan method bawaan dari Monobehaviour, di mana method tersebut akan terus dipanggil pada setiap fixed frame. 
 * Dalam method tersebut, kita akan mengubah state dari burung menjadi Thrown, 
 * jika kondisi saat ini adalah Idle dan kecepatannya berubah menjadi lebih dari 0.5. 
 * Jika kondisi burung pada saat ini adalah Thrown, dan kecepatan burung telah berada di bawah batas minimum (0.5), 
 * maka kita akan menghancurkan game object  burung tersebut setelah 2 detik.
 * 
 * Method MoveTo berfungsi untuk menginisiasi posisi dan mengubah parent dari game object burung.
 * Method Shoot berfungsi untuk melemparkan burung dengan arah, jarak tali yang ditarik, dan kecepatan awal. 
 */

/* OnBirdDestroyed merupakan sebuah event delegate dari UnityAction. 
 * Event tersebut kemudian dipanggil pada method OnDestroy(), 
 * sehingga class yang meng-subscribe event tersebut, 
 * dapat mengetahui ketika object Bird di-destroy dan dapat melakukan aksi yang sesuai setelahnya. 
 * Perlu diingat, sebaiknya setiap event delegate diinisiasi dengan sebuah method kosong (dalam contoh di atas menggunakan delegate { }; ), 
 * agar ketika tidak ada class lain yang meng-subscribe event  tersebut, tidak terjadi error null ketika ia dipanggil.
 */

/* Fungsi ini tidak melakukan apa pun, 
 * karena burung merah yang sudah kita buat sebelumnya tidak akan melakukan apa-apa ketika di-tap saat terbang. 
 * Fungsi ini baru akan berfungsi pada burung berwarna kuning. 
 * Kata kunci virtual kita tambahkan agar class turunan dari Bird dapat melakukan override terhadap fungsi ini.
 */
