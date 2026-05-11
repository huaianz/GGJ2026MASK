using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Tester : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (!IsSceneLoaded("PersistentScene"))
        {
            yield return SceneManager.LoadSceneAsync("PersistentScene", LoadSceneMode.Additive);
        }
    
        if (!IsSceneLoaded("Level2Scene"))
        {
            yield return SceneManager.LoadSceneAsync("Level2Scene", LoadSceneMode.Additive);
        }
        
        Level2Loader loader = FindObjectOfType<Level2Loader>();
        if (loader != null)
        {
            loader.InitLevel2();
        }
    }

    private bool IsSceneLoaded(string sceneName)
    {
        var s = SceneManager.GetSceneByName(sceneName);
        return s.IsValid() && s.isLoaded;
    }
}
