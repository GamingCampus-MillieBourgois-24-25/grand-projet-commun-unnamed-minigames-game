using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    public Light redLight; // Référence au projecteur rouge
    public Light blueLight; // Référence au projecteur bleu
    public float flashSpeed = 0.5f; // Vitesse de clignotement (en secondes)

    private bool isRedActive = true; // Indique si la lumière rouge est active

    void Start()
    {
        if (redLight == null || blueLight == null)
        {
            Debug.LogError("Les lumières rouge et/ou bleue ne sont pas assignées !");
            return;
        }

        // Initialiser les lumières
        redLight.enabled = true;
        blueLight.enabled = false;

        // Lancer l'effet de clignotement
        InvokeRepeating(nameof(SwitchLights), flashSpeed, flashSpeed);
    }

    private void SwitchLights()
    {
        // Alterner entre la lumière rouge et la lumière bleue
        isRedActive = !isRedActive;
        redLight.enabled = isRedActive;
        blueLight.enabled = !isRedActive;
    }

    public void StopLights()
    {
        // Arrêter l'effet de clignotement
        CancelInvoke(nameof(SwitchLights));
        redLight.enabled = false;
        blueLight.enabled = false;
    }

    public void StartLights()
    {
        // Redémarrer l'effet de clignotement
        redLight.enabled = true;
        blueLight.enabled = false;
        isRedActive = true;
        InvokeRepeating(nameof(SwitchLights), flashSpeed, flashSpeed);
    }
}
