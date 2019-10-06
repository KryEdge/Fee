using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRetryButton : MonoBehaviour
{
    public void RepeatScene()
    {
        Destroy(GameManager.Get());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
