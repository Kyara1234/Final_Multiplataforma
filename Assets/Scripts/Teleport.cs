using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string sceneToLoad = "NextScene"; 
    public string targetObjectName = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == targetObjectName)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}