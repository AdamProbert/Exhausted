using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
 public class MainMenuManager : MonoBehaviour 
 {
    public void LoadDessertScene()
    {
        SceneManager.LoadScene("DesertArena");
    }
 }
