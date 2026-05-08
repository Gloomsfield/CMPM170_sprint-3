using UnityEngine;

public class GameManager {

    private static GameManager theInstance;
    private GameState state = GameState.GAME;

    // TODO remove?
    public enum GameState
    {
        PREGAME,
        GAME,
        THERAPY
    }
    public GameState gameState => state;
    
    private GameManager() {
    }

    public static GameManager Instance { get
        {
            if (theInstance == null) {
                theInstance = new GameManager();
            }
            return theInstance;
        }
    }
}
