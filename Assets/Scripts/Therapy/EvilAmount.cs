using UnityEngine;

public class EvilAmount : MonoBehaviour
{
    int evilAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddEvilAmount(0);
    }

    public void AddEvilAmount(int amount)
    {
        evilAmount += amount;
        DataManager.instance.EvilAmount = evilAmount;
    }
}
