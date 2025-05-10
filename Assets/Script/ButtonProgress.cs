using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProgress : MonoBehaviour
{
    public Button button;        // Le bouton principal
    public Image progressBar;    // L'image utilisée comme barre de progression
    public float loadingTime = 2f; // Temps de chargement en secondes

    private bool isLoading = false;
    private float currentProgress = 0f;

    void Update()
    {
        if (isLoading)
        {
            // Incrémentation progressive
            currentProgress += Time.deltaTime / loadingTime;
            progressBar.fillAmount = currentProgress;

            if (currentProgress >= 1f)
            {
                isLoading = false;
                currentProgress = 0f;

            }
        }
    }

    // Méthode appelée lors d'un clic sur le bouton
    public void StartLoading()
    {
        isLoading = true;
    }
}