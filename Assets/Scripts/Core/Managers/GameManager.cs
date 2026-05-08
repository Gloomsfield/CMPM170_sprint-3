using UnityEngine;
public class GameManager {

    public enum GameState
    {
        PREGAME,
        GAME,
        THERAPY
    }
    private static GameManager theInstance;
    
    // TO REMOVE For testing purposes
    public bool accessed = false;

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

    /*EX IMPORTANT! Notice that this suscribed function returns void and has a single float 
     * parameter. This is necessary, since it suscribes to the therapyStarted event, which declared
     * that it would trigger functions of this format */
    void LogTherapyStartTime(float time) {
        Debug.Log("Therapy event triggered at " + time);
    }

}
