using UnityEngine;

public class GameManager {

    private static GameManager theInstance;
    private GameState state = GameState.GAME;

    public enum GameState
    {
        PREGAME,
        GAME,
        THERAPY
    }
    public GameState gameState => state;
    
    private GameManager() {
        /* EX This is how you suscribe a function to an event. You can use -= to unsuscribe.
         * Now, LogTherapyStartTime will get called whenever the therapyStarted event is triggered*/
       
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
