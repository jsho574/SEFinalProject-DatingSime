using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class Game1_Script : MonoBehaviour {

    DialogTree dialogTree;
    CanvasGroup buttonCanvas, canvasGroup;
    Node root;
    Node current;
    Text text;
    Text charName;
    string currentLevel;
    string nextLevel;
    bool typing;
    bool end;
    bool choiceActive;
	// Use this for initialization
	void Start () {
        currentLevel = SceneManager.GetActiveScene().name;
        print(currentLevel);
        dialogTree = new DialogTree(FindDialog(currentLevel));
        buttonCanvas = GameObject.Find("ButtonCanvas").GetComponent<CanvasGroup>();
        canvasGroup = GameObject.Find("UIGroup").GetComponent<CanvasGroup>();
        if (dialogTree.Root == null)
        {
            print("root is f***** up");
        }
        text = GameObject.Find("GameText").GetComponent<Text>();
        charName = GameObject.Find("CharacterText").GetComponent<Text>();
        root = dialogTree.Root;
        current = root;
        nextLevel = FindLevel(currentLevel);
        typing = false;
        end = false;
        choiceActive = false;
        StartCoroutine(waitRoutine());
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickText()
    {
        if (end)
        {
            if (nextLevel.Equals("END"))
            {
                StopAllCoroutines();
                GameObject.Find("MainMenuButton").GetComponent<Button>().onClick.Invoke();
                canvasGroup.interactable = false;
            } else
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
        if (choiceActive)
        {
            return;
        }
        if (current == null)
        {
            if (root == null)
            {
                print("root is also null");
            }
            print("current is null");
            return;
        }

        if (current.Choice == true)
        {
            Button choice1 =  GameObject.Find("Button1").GetComponent<Button>();
            Button choice2 = GameObject.Find("Button2").GetComponent<Button>();
            buttonCanvas.alpha = 1;
            buttonCanvas.interactable = true;
            canvasGroup.interactable = false;

            choice1.GetComponentInChildren<Text>().text = current.ChoiceList[0];
            choice2.GetComponentInChildren<Text>().text = current.ChoiceList[1];

            choice1.onClick.AddListener(delegate { DivergeTree(0); });
            choice2.onClick.AddListener(delegate { DivergeTree(1); });

            choiceActive = true;
            return;
        }

        if (typing == false)
        {
            StartCoroutine(typewriter(current));
        } else
        {
            StopAllCoroutines();
            text.text = current.Dialog;
            typing = false;
            if (current.NodeId != dialogTree.FinalNodeId)
            {
                current = current.Child[0];
            } else
            {
                end = true;
            }
        }
    }

    private void DivergeTree(int index)
    {
        current = current.Child[index].Child[0];
        buttonCanvas.interactable = false;
        buttonCanvas.alpha = 0;
        canvasGroup.interactable = true;
        choiceActive = false;
        ClickText();
    }

    private string FindDialog(string currentLevel)
    {
        if (currentLevel.Equals("Level1"))
        {
            return @"Assets\Dialog\Scene1.txt";
        } else if (currentLevel.Equals("Level2"))
        {
            return @"Assets\Dialog\Scene2.txt";
        } else if (currentLevel.Equals("Level3"))
        {
            return @"Assets\Dialog\Scene3.txt";
        }
        return null;
    }

    private string FindLevel(string currentLevel)
    {
        if (currentLevel.Equals("Level1"))
        {
            return "Level2";
        }
        else if (currentLevel.Equals("Level2"))
        {
            return "Level3";
        }
        else if (currentLevel.Equals("Level3"))
        {
            return "END";
        }
        return null;
    }

    private static IEnumerator waitRoutine()
    {
        yield return new WaitUntil(() => GameObject.Find("transition").GetComponent<Image>().color.a == 0.0f);
    }

    private IEnumerator typewriter(Node node)
    {
        if (typing == false)
        {
            typing = true;
            string name = node.Name;
            string dialog = node.Dialog;

            charName.text = name;
            for (int i = 0; i < dialog.Length; i++)
            {
                text.text = dialog.Substring(0, i);
                yield return new WaitForSeconds(0.05f);
            }
            typing = false;
            if (current.NodeId != dialogTree.FinalNodeId)
            {
                current = current.Child[0];
            } else
            {
                end = true;
            }
        } else
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
