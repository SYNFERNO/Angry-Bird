using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField]
    public float _boostForce = 100;
    public bool _hasBoost = false;

    public void Boost()
    {
        if(State == BirdState.Thrown && !_hasBoost)
        {
            Rbody.AddForce(Rbody.velocity * _boostForce);
            _hasBoost = true;
        }
    }

    public override void OnTap()
    {
        Boost();
    }
}


// note

/*
 * Fungsi Boost adalah untuk memberikan efek boost  ketika burung sedang terbang, dan efek ini hanya dapat digunakan 1 kali saja.
 * 
 * Dengan kelas ini kita sudah memiliki fungsi khusus untuk burung kuning saja. 
 * Tetapi, kita belum menempatkan di mana fungsi ini harus dipanggil. 
 * Pada game, fungsi ini baru dapat dipanggil ketika player melakukan tap pada screen. 
 * Untuk itu, mari kita ubah code pada class GameController agar dapat menerima input tap dari player
 */

/*
 * Dengan begini, ketika burung kuning dilemparkan, kemudian player melakukan tap, maka fungsi Boost akan terpanggil.
 */