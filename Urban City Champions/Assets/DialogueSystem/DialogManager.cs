using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    //SceneUI
    public GameObject DialogScreen;
    
    public GameObject StartButton;
    public GameObject NextButton;

    //DialogBox
    public Text convoContent;
    
    //SentenceString
    private Queue<string> sentences;
    //private Queue<AudioClip> myAudioClips;
    /*
    //Textures
    public Image profileImage;
    public Sprite JamesTired;
    public Sprite KyleReg;
    
    //AnimationYetToBe
    public GameObject fadeToBlack;
    public GameObject blackFader;
    */

    //AudioSourcesYetToBe
    void Start()
    {
        sentences = new Queue<string>();
        //myAudioClips = new Queue<AudioClip>();
        //NextButton.gameObject.SetActive(false);
    }
    public void StartDialog(Dialog dialog)
    {  //method called at start of a conversation
        Debug.Log("talk to " + dialog.name); // check to make sure this works
        // first need to clear out any previous conversation that might linger in sentences array
        sentences.Clear();
        // then loop through the array and line up each sentence currently in it to prepare 
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence); // put each in the queue
        }
        /*foreach (AudioClip myClip in dialog.audioClips)
        {
            myAudioClips.Enqueue(myClip);
        }*/
        // then call a method to actually display it
        DisplayNextSentence();
    }
    /*public void PlayDialogAudio()
    {
        AudioClip myClip = myAudioClips.Dequeue();
        AudioSource myAudio = GetComponent<AudioSource>();
        myAudio.clip = myClip;
        myAudio.Play();
    }*/
    
    public void DisplayNextSentence()
    {
        // first check to see if we are at the end of convo and if so call end method
        if (sentences.Count == 0)
        { // if array is empty
            EndConvo(); // call end method
            return;     // leave the function
        }
        
        string sentence = sentences.Dequeue();  // otherwise pull sentence out of the queue
        Debug.Log(sentences.Count);
        convoContent.text = sentence;
        /*for (int i = 6; i > 0; i--)
            {
            if (sentences.Count == 6)
            {
                NextButton.gameObject.SetActive(true);
                Debug.Log("number is even");
                //StartButton.gameObject.SetActive(false);

            }
            else if (sentences.Count == 5)
            {
                Debug.Log("number is odd");
                //convoContent.color = Color.red;
                profileImage.sprite = KyleReg; 
            }

            else if (sentences.Count == 4)
            {
                Debug.Log("number is even");
                //convoContent.color = Color.yellow;
            }

            else if (sentences.Count == 3)
            {
                Debug.Log("number is odd");
                //convoContent.color = Color.red;
            }

            else if (sentences.Count == 2)
            {
                Debug.Log("number is even");
                //convoContent.color = Color.yellow;
            }

            else if (sentences.Count == 1)
            {
                Debug.Log("number is odd");
                convoContent.alignment = TextAnchor.MiddleCenter;
            }

            else if (sentences.Count == 0)
            {
                Debug.Log("number is even");
                //convoContent.color = Color.red;
            }

            else
            {
                Debug.Log("number is odd");
                //convoContent.color = Color.yellow;
            }
                //Debug.Log ("line is" + sentence); // display it
                
            }
        PlayDialogAudio();*/
    }

    public void EndConvo()
    {
        Debug.Log("reached end of convo");
        //StartCoroutine(SceneTransition(1f));
       
    }
    /*IEnumerator SceneTransition(float time)
    {
        //fadeToBlack.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene("InDreamDialogueScene");

        yield return new WaitForSeconds(2f);

        yield return new WaitForSeconds(5f);

    }*/
}
