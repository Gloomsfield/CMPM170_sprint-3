using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int evilAmount;
    public int EvilAmount { get => evilAmount; set => evilAmount = value; }
    public static DataManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
