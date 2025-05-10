using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 20; // D�g�ts inflig�s par la balle

    private void OnCollisionEnter(Collision collision)
    {
        // V�rifie si la balle entre en collision avec le tank
        if (collision.gameObject.CompareTag("PlayerTank"))
        {
            // R�cup�re le script de gestion du tank
            Tank tank = collision.gameObject.GetComponent<Tank>();

            tank.TakeDammage(damage); // Applique les dommages au tank 
            Destroy(gameObject);
        }
    }
}
