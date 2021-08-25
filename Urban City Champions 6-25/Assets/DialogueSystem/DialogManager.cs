using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{    
    //SentenceString
    private Queue<string> sentences;
   
    void Start()
    {
        sentences = new Queue<string>();
        
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
        
        DisplayNextSentence();
    }
    
    
    public void DisplayNextSentence()
    {
        // first check to see if we are at the end of convo and if so call end method
        if (sentences.Count == 0)
        { // if array is empty
            EndConvo(); // call end method
            return;     // leave the function
        }
        
        string sentence = sentences.Dequeue();  // otherwise pull sentence out of the queue
        Debug.Log(sentence);
       
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
