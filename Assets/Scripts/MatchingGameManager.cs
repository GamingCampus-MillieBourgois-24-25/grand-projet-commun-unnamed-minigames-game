using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchingGameManager : MonoBehaviour
{
    [Header("Pattern Settings")]
    public List<Sprite> allSymbols; // Liste de tous les symboles disponibles
    public Image[] patternSlots; // Slots affichant le motif cible
    public Image[] playerSlots; // Slots où le joueur place ses choix

    [Header("UI Elements")]
    public Transform centerSymbolsPanel; // Panel contenant les symboles sélectionnables
    public GameObject symbolPrefab; // Prefab des symboles interactifs
    public Text timerText; // Texte du timer
    public Text victoryText; // Texte de victoire

    [Header("Game Settings")]
    public float gameTime = 10f; // Temps imparti pour compléter le motif
    private float timer;
    private List<Sprite> patternToMatch; // Liste du motif cible
    private bool isGameActive = false;

    void Start()
    {
        
        GeneratePattern();
        SpawnSymbols();
        StartGame();
    }

    void GeneratePattern()
    {
        patternToMatch = new List<Sprite>();
        for (int i = 0; i < patternSlots.Length; i++)
        {
            Sprite randomSymbol = allSymbols[Random.Range(0, allSymbols.Count)];
            patternToMatch.Add(randomSymbol);
            patternSlots[i].sprite = randomSymbol;
        }
    }

    void SpawnSymbols()
    {
        if (centerSymbolsPanel == null)
        {
            Debug.LogError("❌ centerSymbolsPanel est NULL ! Assigné-le dans l'Inspector.");
            return;
        }

        if (symbolPrefab == null)
        {
            Debug.LogError("❌ symbolPrefab est NULL ! Assigné-le dans l'Inspector.");
            return;
        }

        if (allSymbols == null || allSymbols.Count == 0)
        {
            Debug.LogError("❌ La liste allSymbols est vide ou NULL ! Ajoute des sprites.");
            return;
        }

        Debug.Log("✅ Toutes les références sont assignées. Génération des symboles...");

        List<Vector2> usedPositions = new List<Vector2>(); // Stocke les positions déjà prises
        RectTransform panelRect = centerSymbolsPanel.GetComponent<RectTransform>();

        for (int i = 0; i < allSymbols.Count; i++)
        {
            Vector2 randomPosition = Vector2.zero; // 🔥 Initialisation ici
            bool positionValid = false;
            int maxAttempts = 50; // Pour éviter une boucle infinie

            // Générer une position valide
            while (!positionValid && maxAttempts > 0)
            {
                float randomX = Random.Range(-panelRect.rect.width / 2, panelRect.rect.width / 2);
                float randomY = Random.Range(-panelRect.rect.height / 2, panelRect.rect.height / 2);
                randomPosition = new Vector2(randomX, randomY);

                // Vérifier si la position est trop proche d'une autre
                positionValid = true;
                foreach (Vector2 pos in usedPositions)
                {
                    if (Vector2.Distance(pos, randomPosition) < 100f) // Distance minimale entre objets
                    {
                        positionValid = false;
                        break;
                    }
                }

                maxAttempts--;
            }

            // Instancier l'objet si une position valide a été trouvée
            if (positionValid)
            {
                GameObject newSymbol = Instantiate(symbolPrefab, centerSymbolsPanel);
                RectTransform symbolRect = newSymbol.GetComponent<RectTransform>();
                symbolRect.anchoredPosition = randomPosition; // Placement aléatoire dans le panel

                // Ajouter le sprite correspondant
                Image symbolImage = newSymbol.GetComponent<Image>();
                if (symbolImage != null)
                {
                    symbolImage.sprite = allSymbols[i];
                }

                // Ajouter le bouton cliquable
                Button button = newSymbol.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnSymbolClicked(allSymbols[i], newSymbol));
                }

                usedPositions.Add(randomPosition); // Ajouter cette position à la liste des utilisées
            }
        }
    }




    void StartGame()
    {
        timer = gameTime;
        isGameActive = true;
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "Temps restant : " + Mathf.Ceil(timer).ToString();
            yield return null;
        }
        isGameActive = false;
        CheckWinCondition();
    }

    void OnSymbolClicked(Sprite selectedSymbol, GameObject symbolObject)
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (playerSlots[i].sprite == null)
            {
                playerSlots[i].sprite = selectedSymbol;
                Destroy(symbolObject);
                break;
            }
        }
    }

    void CheckWinCondition()
    {
        bool isCorrect = true;
        for (int i = 0; i < patternSlots.Length; i++)
        {
            if (playerSlots[i].sprite != patternToMatch[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            victoryText.gameObject.SetActive(true);
            Debug.Log("Victoire !");
            Invoke("LoadNextMinigame", 2f);
        }
        else
        {
            Debug.Log("Échec ! Réessaie !");
        }
    }

    void LoadNextMinigame()
    {
        FindObjectOfType<MiniGameManager>().LoadNextMinigame();
    }
}
