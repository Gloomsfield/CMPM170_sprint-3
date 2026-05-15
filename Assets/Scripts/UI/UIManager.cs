using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displaytext;
    [SerializeField] TextMeshProUGUI pressSpace;
    [SerializeField] Image textBackground;
    string pressSpaceText = "Press Space To Continue...";
    [SerializeField] float typingSpeed = 0.5f; // Smaller = Faster
    [SerializeField] DialogueInputHandler uiInput; // Link to inputhandler for space
    private int letterGap = 4;  //this is to delay for the therapist talking. It skips that many letters before saying a noise again.

    string currentFullText;
    bool isTyping = false;
    Coroutine typingCoroutine;

	public static UIManager Instance;

	void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

    void OnEnable()
    {
		EventManager.onContinueTriggered += OnContinue;
    }

    void OnDisable()
    {
		EventManager.onContinueTriggered -= OnContinue;
    }

    // Sets the text for what the Therapist will say
    // CALL THIS BEFORE DisplayTherapyTest
    public void setText(string newText)
    {
        currentFullText = newText;
    }

    // Starts the dialogue with typewritter effect. 
    public void DisplayTherapyText()
    {
		Debug.Log("display");

		displaytext.gameObject.SetActive(true);
		pressSpace.gameObject.SetActive(true);
		
		displaytext.gameObject.SetActive(true);
		pressSpace.gameObject.SetActive(true);
		textBackground.gameObject.SetActive(true);
		
		if (typingCoroutine != null)
		{
		    StopCoroutine(typingCoroutine);
		}
		
		typingCoroutine = StartCoroutine(TypeText(currentFullText));
    }

    IEnumerator TypeText(string textToType)
    {

        isTyping = true;
        displaytext.text = "";
        pressSpace.text = "";

        foreach (char letter in textToType.ToCharArray())
        {
            displaytext.text += letter;
            if (displaytext.text.Length % letterGap == 0)
            {
                // AudioManager.Instance.PlayTherapistLetter(Random.Range(0.9f, 1.1f));
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        foreach (char letter in pressSpaceText.ToCharArray())
        {
            pressSpace.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

    }

    // Clears text and hides the UI. Also changes the gamestate back
    void EndConversation()
    {
        setText("");
        displaytext.gameObject.SetActive(false);
        pressSpace.gameObject.SetActive(false);

		EventManager.InvokeTherapyEnded();

        textBackground.gameObject.SetActive(false);
    }

    void Start()
    {
        // TOREMOVE: this function will be called with an event
        // This is for testing purposes
        setText("THERAPY HAS STARTED! BURN EVERYTHING THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES");

        //displaytext.text = "";
        //displaytext.gameObject.SetActive(false);
    }

    void OnContinue()
    {
		if (isTyping)
		{
           // If the therapist is still talking and space is pressed, instantly show the full text   
           StopCoroutine(typingCoroutine);
           displaytext.text = currentFullText;
           isTyping = false;
       }
       else
       {
           EndConversation();
       }
	}
}
