using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialoguePanel : MyMono
{

    public static DialoguePanel current;

    public GameObject FaceplateHidePosition;
    public GameObject Faceplate;
    public GameObject FaceplateShowPosition;

    public GameObject NameplateHidePosition;
    public GameObject Nameplate;
    public GameObject NameplateShowPosition;

    public GameObject Textplate;

    public AudioSource talkSoundEffect;
    public ScrollingText textScroller;
    public Image faceplateImage;
    public Text nameplateText;

    private bool changeFaceplate;
    private bool fpmove;
    private bool changeNameplate;
    private bool npmove;
    private bool moveTextplate;

    private Sprite nextPortrait;
    private string nextName;

    private bool CanAdvance;
    private bool hasText;

    private void Start()
    {
        current = this;
    }

    public void CharacterTransition(Sprite portrait, string name, string text)
    {
        CanAdvance = false;
        nextName = name; nextPortrait = portrait; textScroller.LoadText(text);
        changeNameplate = true;
        changeFaceplate = true;
        fpmove = true;
        npmove = true;
    }

    public void FacialTransition(Sprite portrait, string text)
    {
        CanAdvance = false;
        nextPortrait = portrait; textScroller.LoadText(text);
        changeFaceplate = true;
        fpmove = true;
    }

    public void TextTransition(string text)
    {
        CanAdvance = false;
        textScroller.LoadText(text);
        hasText = true;
    }


    public override void PausingUpdate()
    {
        // transition movements

        // faceplate transition
        // move the faceplate down off screen and back up.
        if (changeFaceplate)
        {
            if (fpmove)
            {
                if (Util.MoveToTarget(Faceplate, FaceplateHidePosition, Screen.height * 3))
                {
                    fpmove = false;
                    faceplateImage.sprite = nextPortrait;
                }
            }
            else {
                if (Util.MoveToTarget(Faceplate, FaceplateShowPosition, Screen.height * 3))
                {
                    changeFaceplate = false;
                    if (!changeNameplate)
                    {
                        textScroller.Display();
                    }
                }
            }
        }


        if (changeNameplate)
        {
            // nameplate transition
            // rotate 180 and reset rotation
            if (npmove)
            {
                if (Util.MoveToTarget(Nameplate, NameplateHidePosition, Screen.width * 5))
                {
                    npmove = false;
                    nameplateText.text = nextName;
                }
            }
            else
            {
                if (Util.MoveToTarget(Nameplate, NameplateShowPosition, Screen.width * 5))
                {
                    changeNameplate = false;
                    if (!changeFaceplate)
                    {
                        textScroller.Display();
                    }
                }
            }
        }

        if (textScroller.isDone && Input.GetMouseButtonDown(0))
        {
            CanAdvance = true;
        }
        else if (Input.GetMouseButtonDown(0) && !changeFaceplate && !changeNameplate && !hasText)
        {
            textScroller.fastforward();
        }

        if (hasText)
        {
            textScroller.Display();
            hasText = false;
        }

    }

    public bool isDone()
    {
        return CanAdvance;
    }
}
