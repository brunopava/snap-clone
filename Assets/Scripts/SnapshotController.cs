using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnapshotController : MonoBehaviour
{
    private int photosTaken = 0;

    public Camera targetCamera;
    public Transform contentPanel;
    public GameObject picPrefab;

    private FirstPersonController playerFPC;
    private List<Texture2D> _snaps;

    public Slider timeSlider;

    private bool canSnap = true;
    private bool isGameOn = true;

    public float sliderStep = 0.5f;

    public GameObject canvas;
    public GameObject panelPhoto;
    public GameObject chrosshair;
    public GameObject panelMenu;

    private void Start()
    {
        _snaps = new List<Texture2D>();
        canvas.gameObject.SetActive(false);
        playerFPC = GetComponent<FirstPersonController>();
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(panelMenu.activeSelf)
            {
                HideMenu();
            }else{
                ShowMenu();
            }
        }

        if(!isGameOn)
            return;

        if(Input.GetMouseButtonUp(0) && canSnap)
        {
            photosTaken++;
            Texture2D snapshot = ScreenShot.GetSnapshot(targetCamera);
            _snaps.Add(snapshot);
            CreatePicture(snapshot);

            //TODO: Index picture with some data such as:
            //Creatures on screen, wich animation they are currently playing
            //Figure out how centered they are in the photo
            //Use this to score the photo
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            if(canvas.activeSelf)
            {
                HideAlbum();
            }else{
                ShowAlbum();
            }
        }
        
        //This timescale is just development friendly for debugging purposes
        //The real game doest not have this feature 
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0) 
        { 
            timeSlider.value += sliderStep;
            Time.timeScale = timeSlider.value;
        } 
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0) 
        { 
            timeSlider.value -= sliderStep;
            Time.timeScale = timeSlider.value; 
        }
    }

    public void ShowMenu()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        panelMenu.SetActive(true);

        //I know this is bad and not optimal.
        Button restart = panelMenu.transform.GetChild(0).GetComponent<Button>();
        Button exit = panelMenu.transform.GetChild(1).GetComponent<Button>();

        restart.onClick.RemoveAllListeners();
        exit.onClick.RemoveAllListeners();

        restart.onClick.AddListener(
            ()=>{
                // Debug.Log("Restart");
                Time.timeScale = 1f;
                SceneManager.LoadScene("SnapClone");
            }
        );

        exit.onClick.AddListener(
            ()=>{
                // Debug.Log("QUIT");
                Application.Quit();
            }
        );
    }

    public void HideMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        panelMenu.SetActive(false);
    }

    public void ShowAlbum()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        playerFPC.cameraCanMove = false;
        canSnap = false;
        chrosshair.SetActive(false);
    }

    public void HideAlbum()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        playerFPC.cameraCanMove = true;
        canSnap = true;
        chrosshair.SetActive(true);
    }

    public void CreatePicture(Texture2D snapshot)
    {
        GameObject temp = GameObject.Instantiate(picPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        temp.transform.SetParent(contentPanel);
        RawImage picture = temp.GetComponent<RawImage>();
        picture.texture = snapshot;

        temp.GetComponent<Button>().onClick.AddListener(
            ()=>{
                panelPhoto.SetActive(true);

                //This is also bad.
                //In general we should avoid GetChild and GetComponent as much as possible.
                GameObject photo = panelPhoto.transform.GetChild(0).gameObject;
                Button button = photo.GetComponent<Button>();
                photo.GetComponent<RawImage>().texture = snapshot;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(
                    ()=>{
                        panelPhoto.SetActive(false);
                    }
                );
            }
        );
    }

    public void EndGame()
    {
        ShowAlbum();
    }
}
