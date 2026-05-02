using UnityEngine;
public class GameManager {
    private static GameManager theInstance;

    private GameManager() {}

    public static GameManager Instance { get
        {
            if (!theInstance) {
                theInstance = new GameManager();
            }
            return theInstance;
        }
    }

}
