using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer Trajectory;

    private Vector2 _startPos;

    [SerializeField] private float _radius = 0.75f;
    [SerializeField] private float _throwSpeed = 30f;

    private Bird _bird;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
    }

    private void OnMouseUp()
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        // Kembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }

    private void OnMouseDrag()
    {
        // Mengubah posisi mouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Hitung supaya 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = p - _startPos;

        if (dir.sqrMagnitude > _radius)
            dir = dir.normalized * _radius;
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if (!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    void DisplayTrajectory(float distance)
    {
        if(_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        // Posisi awal trajectory merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        // velocity awal
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for(int i = 1; i < segmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for(int i = 0; i < segmentCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// Note

/* Atribut _startPos digunakan untuk menyimpan titik awal sebelum karet ketapel ditarik. 
 * Atribut _radius merupakan radius/panjang maksimal dari tali ditarik. 
 * Atribut _throwSpeed adalah kecepatan awal yang diberikan ketapel pada saat melontarkan burung nantinya.
 * 
 * Pada method OnMouseDrag, kita akan mengubah posisi dari game object ShooterArea agar mengikuti gerakan mouse, 
 * tetapi gerakan tersebut akan terbatas pada radius yang kita tetapkan.
 * 
 * Pada method OnMouseUp, burung akan kita lemparkan dengan arah (velocity) dan panjangan tarikan ketapel beserta dengan kecepatan awal. 
 * Lalu kembalikan posisi tali pelontar ke posisi awal. 
 * Dengan mematikan collider, maka method OnMouseDrag dan OnMouseUp tidak akan dipanggil ketika mouse di klik pada area collider.
 */

/* Trajectory akan terus berubah sesuai dengan gerakan mouse player pada saat menarik ketapel. 
 * Oleh karena itu, pada method OnMouseDrag kita akan terus menggambarkan ulang trajectory dari burung yang akan dilempar.
 * 
 * Method DisplayTrajectory merupakan method yang berfungsi untuk memprediksikan posisi burung dengan rumus di atas, 
 * kemudian menggambarkannya dengan menggunakan LineRenderer. Parameter masukannya merupakan panjang ketapel yang ditarik. 
 * Panjang ketapel yang ditarik kita dapatkan dengan menghitung jarak dari titik awal ketapel dan posisi mouse pada saat ini.
 * 
 * Atribut segmentCount merupakan total point/titik yang akan kita gambarkan ke dalam trajectory kita. 
 * Pada saat ini kita akan membuat 5 titik untuk trajectory kita.
 * 
 * Untuk setiap titik, kita akan menghitung posisi dari burung setelah dilemparkan pada waktu tertentu dengan menggunakan rumus di atas. 
 * segments[0] merupakan posisi awal (p0). segVelocity merupakan velocity awal (v0). Daya gravitasi kita dapatkan dari setting Physics2D.
 * 
 * Setelah mendapatkan posisi burung dari semua point tersebut, 
 * kita tinggal mengambarkannya pada LineRenderer  yang kita punya dengan menggunakan method SetPosition.
 * 
 */
