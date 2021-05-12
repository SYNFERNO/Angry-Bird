using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    public BoxCollider2D TapCollider;

    private Bird _shotBird;

    private bool _isGameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if(_isGameEnded == true)
        {
            return;
        }

        Birds.RemoveAt(0);

        if(Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
        }
    }

    public void CheckGameEnd(GameObject destroyEnemy)
    {
        for(int i = 0; i < Enemies.Count; i++)
        {
            if(Enemies[i].gameObject == destroyEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if(Enemies.Count == 0)
        {
            _isGameEnded = true;
        }
    }

    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    private void OnMouseUp()
    {
        if(_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}

// Note

/* Dari code di atas, maka setiap kali object Bird memanggil method OnBirdDestroyed, 
 * Game Controller akan memanggil method Change Bird.
 */

/*
 * Pada game object tersebut, kita akan menambahkan BoxCollider2D untuk dapat menerima event  OnMouseUp. 
 * Collider tersebut akan kita nyalakan, bila burung sudah dilontarkan, 
 * dan akan dimatikan bila ada burung baru yang ditempatkan pada ketapel dalam kondisi Idle.
 * 
 * Dalam fungsi OnMouseUp, 
 * kita akan memberitahukan burung yang saat ini dilontarkan bahwa player 
 * melakukan tap pada screen dengan memanggil fungsi OnTap dalam burung. 
 * Untuk Itu, buka kembali script Bird
 */
