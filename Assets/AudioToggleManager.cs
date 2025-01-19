using UnityEngine;
using UnityEngine.UI;

public class AudioToggleManager : MonoBehaviour
{
    public AudioSource bgmAudioSource; // L'Audio Source con la musica di sottofondo
    public Toggle muteToggle;         // Il toggle nella UI

    void Start()
    {
        // Assicurati che il toggle rifletta lo stato attuale dell'audio
        muteToggle.isOn = !bgmAudioSource.mute;

        // Aggiungi un listener per gestire i cambiamenti del toggle
        muteToggle.onValueChanged.AddListener(ToggleAudio);
    }

    // Metodo per mutare/smutare l'audio
    void ToggleAudio(bool isOn)
    {
        bgmAudioSource.mute = !isOn; // Mutare se il toggle è off
    }
}