using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Transition
{

    public class Teleport : MonoBehaviour
    {
        [SerializeField] string sceneToGo;
        [SerializeField] Vector3 positionToGo;
        [SerializeField] private string level2UIRootName = "Level2UIRoot";

        private GameObject level2UIRoot;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")&& GameManager.Instance.Task_SO.Task[0].Wardrobe == 1)
            {

                EventHandler.CallTransitionEvent(sceneToGo, positionToGo);
                Level2Loader loader = FindObjectOfType<Level2Loader>();
                if (loader != null)
                {
                    loader.InitLevel2();
                }
                
                // Activate Level 2 UI Root after scene is loaded
                level2UIRoot = GameObject.Find(level2UIRootName);

                if (level2UIRoot != null)
                {
                    level2UIRoot.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Level2UIRoot not found in loaded scenes.");
                }
            }
        }   
    }
}