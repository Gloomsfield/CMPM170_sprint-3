using UnityEngine;
using TMPro;
using System.Collections;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displaytext;
    [SerializeField] float typingSpeed = 0.5f; // Smaller = Faster
    [SerializeField] DialogueInputHandler uiInput; // Link to inputhandler for space

    string currentFullText;
    bool isTyping = false;
    Coroutine typingCoroutine;

    void OnEnable()
    {
        //TODO: Subscribe to event
        //SubscribeToEvent += DisplayTherapyText
    }

    void OnDisable()
    {
        //TODO: Unsubcribe to event
        //SubscribeToEvent -= DisplayTherapyText
    }

    // Sets the text for what the Therapist will say
    // CALL THIS BEFORE DisplayTherapyTest
    public void setText(string newText)
    {
        currentFullText = newText;
    }

    // Starts the dialogue with typewritter effect. 
    void DisplayTherapyText()
    {

            displaytext.gameObject.SetActive(true);

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

        foreach (char letter in textToType.ToCharArray())
        {
            displaytext.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

    }

    // Clears text and hides the UI. Also changes the gamestate back
    void EndConversation()
    {
        setText("");
        displaytext.gameObject.SetActive(false);
    }

    void Start()
    {
        // TOREMOVE: this function will be called with an event
        // This is for testing purposes
        setText("THERAPY HAS STARTED! BURN EVERYTHING THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES THIS TEXT IS FOR TESTING PURPOSES");
        DisplayTherapyText();

        //displaytext.text = "";
        //displaytext.gameObject.SetActive(false);
    }

    void Update()
    {
        if (uiInput.ContinueTriggered)
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
}
