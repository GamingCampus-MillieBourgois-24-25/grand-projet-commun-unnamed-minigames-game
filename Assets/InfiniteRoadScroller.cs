using UnityEngine;

public class InfiniteRoadGenerator : MonoBehaviour
{
    public GameObject roadPrefab;  // Le prefab de la route
    public int numberOfRoads = 5;  // Nombre de morceaux de route visibles � un moment donn�
    public float roadLength = 50f;  // Longueur d'un morceau de route
    public float scrollSpeed = 10f; // Vitesse du d�filement

    // Position exacte de d�part de la route. Peut �tre modifi�e via l'Inspector.
    public Vector3 startPosition = new Vector3(0, 0, 0);  // Position de d�part de la route

    private GameObject[] roads;  // Tableau pour stocker les morceaux de route

    void Start()
    {
        roads = new GameObject[numberOfRoads];

        // Cr�er les morceaux de route initialement
        for (int i = 0; i < numberOfRoads; i++)
        {
            // Initialiser la position de chaque morceau de route avec la position exacte
            roads[i] = Instantiate(roadPrefab, startPosition + new Vector3(0, 0, i * roadLength), Quaternion.identity);
        }
    }

    void Update()
    {
        // Faire d�filer les morceaux de route
        for (int i = 0; i < numberOfRoads; i++)
        {
            // D�placer la route sur l'axe Z
            roads[i].transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);

            // R�initialiser la position du morceau de route si il est pass� derri�re le joueur
            if (roads[i].transform.position.z <= -roadLength)
            {
                // D�placer ce morceau de route devant
                int nextIndex = (i + 1) % numberOfRoads;  // Obtenir l'index du prochain morceau
                roads[i].transform.position = new Vector3(0, 0, roads[nextIndex].transform.position.z + roadLength);
            }
        }
    }
}
