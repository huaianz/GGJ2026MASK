using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Transition
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] string sceneToGo;
        [SerializeField] Vector3 positionToGo;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && GameManager.Instance.Task_SO.Task[0].Wardrobe == 1)
            {
                EventHandler.CallTransitionEvent(sceneToGo, positionToGo);
                Level2Loader loader = FindObjectOfType<Level2Loader>();
                if (loader != null)
                {
                    loader.InitLevel2();
                }
            }
        }
    }
}