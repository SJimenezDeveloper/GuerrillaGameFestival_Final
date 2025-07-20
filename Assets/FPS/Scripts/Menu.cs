using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    // Nombre de la escena a cargar
    public string sceneName;

    // M�todo que se llama desde el bot�n
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena no est� definido.");
        }
    }
}
