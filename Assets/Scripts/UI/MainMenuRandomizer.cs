using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//using UnityEngine.Random;
public class MainMenuRandomizer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainMenuTitle;
    [SerializeField] string[] titles = { "Gone Gnomo", "Getting to Gnome You", "Gnomepinephrine", 
        "Don't Touch My Gnome Gnome Square", "Gnomicide", "The Man Who Gnomed The World",
    "Misgnomer", "The Great Gnomesby"};
    
    public void RandomizeText()
    {
        mainMenuTitle.text = titles[Random.Range(0, titles.Length + 1)];
    }

}
