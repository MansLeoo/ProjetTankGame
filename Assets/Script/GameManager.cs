using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<Material> tankMaterials;
    [SerializeField] private Material tankInitMaterial;

    private int textureTankIndex;

    public int TextureTankIndex { get => textureTankIndex; set => textureTankIndex = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("La scène " + scene.name + " a été chargée avec le mode " + mode);
        if (scene.name == "Game")
        {
            // PERMET DE CHANGER LA TEXTURE DU TANK 
            Material nMat = tankMaterials[textureTankIndex];
            // Trouver tous les objets avec un Renderer dans la scène
            Renderer[] renderers = FindObjectsOfType<Renderer>();
            foreach (Renderer rend in renderers)
            {
                // Vérifier si le matériau de l'objet est celui à remplacer
                if (rend.sharedMaterial == tankInitMaterial)
                {
                    rend.sharedMaterial = nMat;
                }
            }
        }
    }
}
