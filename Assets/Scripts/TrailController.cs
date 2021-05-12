using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject Trail;
    public Bird TargetBird;

    private List<GameObject> _trails;
    // Start is called before the first frame update
    void Start()
    {
        _trails = new List<GameObject>();
    }

    public void SetBird(Bird bird)
    {
        TargetBird = bird;

        for(int i = 0; i < _trails.Count; i++)
        {
            Destroy(_trails[i].gameObject);
        }

        _trails.Clear();
    }

    public IEnumerator SpawnTrail()
    {
        _trails.Add(Instantiate(Trail, TargetBird.transform.position, Quaternion.identity));

        yield return new WaitForSeconds(0.1f);

        if(TargetBird != null && TargetBird.State != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }    
}


// note

/* Atribut GameObject Trail di atas akan menampung prefab berupa GameObject dengan komponen sprite yang akan kita buat nanti.
 * TargetBird merupakan burung yang akan kita berikan trails.
 * List _trails merupakan trail yang ditampilkan dalam game.
 * Pada method Start, atribut _trails diinisiasi.
 * Method SetBird berfungsi untuk menambahkan burung yang akan dijadikan target, kemudian me-reset ulang trail yang ada.
 * SpawnTrail merupakan method yang berfungsi untuk membuat game object trail setiap 100ms. 
 * Method IEnumerator dapat kita gunakan untuk memberikan delay tersebut dengan menggunakan code:
 * 
 * yield return new WaitForSeconds(0.1f);
 * 
 * Dengan code tersebut, maka ketika method dipanggil, ia akan menunggu hingga kondisi Wait tersebut terpenuhi, 
 * baru melanjutkan ke potongan code di bawahnya. 
 * Setelah menunggu 100ms, jika TargetBird belum mengenai collider lain, maka trail akan kita tambahkan.
 */