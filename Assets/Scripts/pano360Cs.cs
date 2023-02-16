using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pano360Cs : MonoBehaviour {

    public GameObject container;
    public GameObject panoCamera;
    public GameObject sphere;
    public float sens;
    public bool allowScroll = true;
    public List<Material> materials;
    public List<GameObject> items;
    public GameObject PanoInfoGo;
    public Text PanoInfoText;

    private Vector2 startPos;

    private GameObject MainCamera;
    private GameObject MainCanvas;

    void Start () {
        MainCamera = GameObject.Find("Main Camera");
        MainCanvas = GameObject.Find("Canvas");
    }

    public void Init(int index)
    {
        if (index < 0 || index >= materials.Count)
        {
            return;
        }
        container.SetActive(true);
        sphere.GetComponent<MeshRenderer>().material = materials[index];
        disableItmes();
        items[index].SetActive(true);
        MainCanvas.GetComponent<CanvasGroup>().alpha = 0;
        MainCamera.SetActive(false);
    }

    public void Disactivate()
    {
        disableItmes();
        container.SetActive(false);
        MainCamera.SetActive(true);
        MainCanvas.GetComponent<CanvasGroup>().alpha = 1;
        sphere.GetComponent<MeshRenderer>().material = null;
    }

    public void SetUiInfo(string msg)
    {
        PanoInfoGo.SetActive(true);
        PanoInfoText.GetComponent<Text>().text = msg;
    }

    public void DisableUiInfo()
    {
        PanoInfoGo.SetActive(false);
    }

    private void disableItmes()
    {
        items.ForEach(x => x.SetActive(false));
    }

    void Update ()
    {
        if (!container.activeSelf)
        {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            sphere.transform.Rotate(new Vector3(0, -sens, 0));
            
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            sphere.transform.Rotate(new Vector3(0, sens, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            panoCamera.transform.Rotate(new Vector3(-sens, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            panoCamera.transform.Rotate(new Vector3(sens, 0, 0));
        }

        if(true)
        {
            float fov = panoCamera.GetComponent<Camera>().fieldOfView;
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                fov += -10 * Input.GetAxis("Mouse ScrollWheel");
                if (fov>10 && fov<65)
                {
                    panoCamera.GetComponent<Camera>().fieldOfView = fov;
                }
                
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            startPos = panoCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0))
        {
            Vector2 pos = new Vector2();
            pos.x = 200 * (panoCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition).x - startPos.x);
            pos.y = 100 * (panoCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition).y - startPos.y);
            //Debug.Log(startPos + " : " + panoCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition));
            sphere.transform.Rotate(new Vector3(0, pos.x, 0));
            panoCamera.transform.Rotate(new Vector3(pos.y, 0, 0));           
            startPos = panoCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
        }
    }

    public void resetPano()
    {
        panoCamera.GetComponent<Camera>().fieldOfView = 55f;
    }
}
