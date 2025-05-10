using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 360, 0); // Vitesse de rotation (par axe)
    [SerializeField] private bool isClockwise = true; // D�finir la direction de rotation

    void Update()
    {
        // D�termine la direction de rotation (sens horaire ou anti-horaire)
        float direction = isClockwise ? 1f : -1f;

        // Applique une rotation en fonction du temps �coul�
        transform.Rotate(rotationSpeed * direction * Time.deltaTime);
    }
}
