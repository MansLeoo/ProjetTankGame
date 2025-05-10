using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    // Singleton Instance
    public static UiManager Instance { get; private set; }

    private List<GameObject> pannels = new();
    private int selection = 0;
    [SerializeField] private GameObject messageErreur;
    void Awake()
    {
        // Implémentation Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Supprime l'objet en cas de doublon
            return;
        }
        Instance = this;
    }
    public void OnPanelClicked(GameObject panel,int index)
    {
        if (!pannels.Contains(panel)) pannels.Add(panel);
        foreach (GameObject pan in pannels)
        {
            if(panel == pan)
            {
                pan.GetComponent<Image>().color = new Color32(255, 73, 73, 223); // Change la couleur en rouge
                selection = index; // Recupere l'index de la texture
                
            }
            else
            {
                pan.GetComponent<Image>().color = new Color32(255, 255, 255, 100);// Change la couleur en blanc
            }
            
        }
    }
    public void StartGame()
    {
        if(selection == 0)
        {
            messageErreur.SetActive(true); // Affiche le message d'erreur si pas de tank selectionner
        }
        else
        {
            GameManager.Instance.TextureTankIndex = --selection;
            SceneManager.LoadScene("Game");

        }
    }
    public void Update()
    {
        if (!(selection == 0)) messageErreur.SetActive(false); // Si selection, désactive le message d'erreur
    }
}
