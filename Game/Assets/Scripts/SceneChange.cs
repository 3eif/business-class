using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//changes scene into other specified scene when a button is pressed (implemented in Unity)
public class SceneChange : MonoBehaviour
{
   public void customScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }
}
