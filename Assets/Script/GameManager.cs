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
        Debug.Log("La sc�ne " + scene.name + " a �t� charg�e avec le mode " + mode);
        if (scene.name == "Game")
        {
            // PERMET DE CHANGER LA TEXTURE DU TANK 
            Material nMat = tankMaterials[textureTankIndex];
            // Trouver tous les objets avec un Renderer dans la sc�ne
            Renderer[] renderers = FindObjectsOfType<Renderer>();
            foreach (Renderer rend in renderers)
            {
                // V�rifier si le mat�riau de l'objet est celui � remplacer
                if (rend.sharedMaterial == tankInitMaterial)
                {
                    rend.sharedMaterial = nMat;
                }
            }
        }
    }
}
