using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Microsoft.VisualBasic;

public class Level2Loader : MonoBehaviour
{
    public void InitLevel2()
    {
        StartCoroutine(SetupLevel2());
    }

    private IEnumerator SetupLevel2()
    {
        // Wait one frame to ensure the scene is fully loaded
        yield return null;

        // Call Level2ResetController to initialize level reset logic
        Level2ResetController resetController = FindObjectOfType<Level2ResetController>();
        resetController.RestartLevel2WhenDeadOrInit();

        // Set up camera confiner for Level 2
        SwitchBounds switchBounds = FindObjectOfType<SwitchBounds>();
        if (switchBounds != null)
        {
            switchBounds.SwitchConfinerShape();
        }

    }
}
