using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGameManager : MonoBehaviour
{
    [Header("Pattern Settings")]
    public List<Sprite> allSymbols; // Liste de tous les symboles disponibles
    public Image[] patternSlots; // Les 3 slots qui affichent le motif cible
    public Image[] playerSlots; // Slots où le joueur place ses choix

    [Header("UI Elements")]
    public Transform centerSymbolsPanel; // Panel contenant les symboles sélectionnables
    public GameObject symbolPrefab; // Prefab d'un symbole interactif
    public TextMeshProUGUI timerText; // Texte du timer
    public TextMeshProUGUI victoryText; // Texte de victoire

    [Header("Game Settings")]
    public float gameTime = 10f; // Temps imparti pour compléter le pattern
    private float timer;
    private List<Sprite> patternToMatch; // Liste du motif à reproduire
    private bool isGameActive = false;

    void Start()
    {
        victoryText.gameObject.SetActive(false);
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
        foreach (Sprite symbol in allSymbols)
        {
            GameObject newSymbol = Instantiate(symbolPrefab, centerSymbolsPanel);
            newSymbol.GetComponent<Image>().sprite = symbol;
            newSymbol.GetComponent<Button>().onClick.AddListener(() => OnSymbolClicked(symbol, newSymbol));
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
            timerText.text = "Temps restant : " + Mathf.Ceil(timer);
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
