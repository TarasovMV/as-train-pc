using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StateController : MonoBehaviour {

    ExperienceVrState state => JsonUtility.FromJson<ExperienceVrState>(File.ReadAllText("test.txt"));

    public enum SceneStack
    {
        Tank = 1,
        GasSeparator = 2,
        RectificationScheme = 3,
        CompressorScheme = 4,
        Pump = 5,
    }

    delegate bool Condition(GameObject item);

    public SceneStack CurrentStage;
    public List<GameObject> Scenes;
    public Transform CameraTransform;
    public Transform ControllerTransform;

    public GameObject VrContainer;
    public GameObject TimerGo;

    private GameObject MainCamera;
    private GameObject MainCanvas;

    void Start() {
        //setState(state);
        //Debug.Log(GameObject.Find("System").name);
        MainCamera = GameObject.Find("Main Camera");
        MainCanvas = GameObject.Find("Canvas");
    }

    public void Activate()
    {
        VrContainer.SetActive(true);
        MainCanvas.GetComponent<CanvasGroup>().alpha = 0;
        MainCamera.SetActive(false);
    }

    public void Disactivate()
    {
        VrContainer.SetActive(false);
        MainCamera.SetActive(true);
        MainCanvas.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void SetState(ExperienceVrState state)
    {
        if (!(VrContainer?.activeSelf ?? true))
        {
            return;
        }

        checkScene((SceneStack)state.Scene);
        CameraTransform.SetTransformByState(state.CameraTransform);
        ControllerTransform.SetTransformByState(state.ControllerTransform);
        setMainGoTransform(state.MainGoTransform);
        setCursorZone(state.CursorZonesIdx);
        setMovableItemsStates(state.CursorMovableItems, state.ChooseMovableItems, state.UseMovableItems);
        setZoneItems(state.ZoneItems);
        setTimerValue(state.AllTimeVr, state.RestTimeVr);
    }

    private void checkScene(SceneStack stateScene)
    {
        if (CurrentStage == stateScene)
        {
            return;
        }
        CurrentStage = stateScene;
        Scenes.ForEach(x => x.SetActive(false));
        Scenes[(int)stateScene - 1].SetActive(true);
    }

    private void setMainGoTransform(StateTransform stateTransform)
    {
        if (stateTransform == null)
            return;
        var mainGo = GameObject.Find("System")?.GetComponent<commonData>()?.MainRotationGo ?? null;
        if (mainGo == null)
            return;
        mainGo.transform?.SetTransformByState(stateTransform);
    }

    private void setCursorZone(int idx)
    {
        var zones = GameObject.Find("System")?.GetComponent<commonData>()?.massPoints ?? null;
        if (zones == null)
            return;
        foreach (var zone in zones)
        {
            zone?.GetComponent<IGreenField>()?.SetDefault();
        }
        if (idx == -1)
            return;
        zones[idx]?.GetComponent<IGreenField>()?.SetCursor();
    }

    // TODO set default
    private void setMovableItemsStates(List<int> cursors, List<int> chooses, List<int> uses)
    {
        List<IChooseItem> movableItems = new List<IChooseItem>();
        var movableItemsContainer = GameObject.Find("MovableItems");
        foreach (Transform item in movableItemsContainer.transform)
            movableItems.Add(item.gameObject.GetComponent<IChooseItem>());
        movableItems.ForEach(x => x.SetDefault());
        setMovableItemsState(cursors, movableItems, (obj) => obj.SetCursor());
        setMovableItemsState(chooses, movableItems, (obj) => obj.SetChoose());
        setMovableItemsState(uses, movableItems, (obj) => obj.SetUse());
    }

    private void setMovableItemsState(List<int> idxs, List<IChooseItem> movableItems, Action<IChooseItem> action)
    {
        foreach (var idx in idxs)
            action(movableItems[idx]);
    }

    private void setZoneItems(List<int> zoneItems)
    {
        var zones = GameObject.Find("System")?.GetComponent<commonData>()?.massPoints ?? null;
        if (zones == null)
            return;
        for(int i = 0; i < zones.Length; i++)
            zones[i]?.GetComponent<IGreenField>()?.SetObject(zoneItems[i]);
    }

    private void setTimerValue(int allTime, int restTime)
    {
        if (TimerGo == null)
            return;
        TimerGo?.GetComponent<TimerContoller>()?.SetTime(allTime, restTime);
    }
}

public static class TransformExtensions
{
    public static void SetTransformByState(this Transform transform, StateTransform stateTransform)
    {
        if (!((stateTransform?.position?.Count ?? 0) == 3 && (stateTransform?.rotation?.Count ?? 0) == 4))
            return;

        transform.SetPositionAndRotation(
            new Vector3(stateTransform.position[0], stateTransform.position[1], stateTransform.position[2]),
            new Quaternion(stateTransform.rotation[0], stateTransform.rotation[1], stateTransform.rotation[2], stateTransform.rotation[3])
        );
    }
}

[Serializable]
public class ExperienceVrState
{
    public int Scene;
    public StateTransform CameraTransform; // +
    public StateTransform ControllerTransform; // +
    public StateTransform MainGoTransform; // +
    public List<int> UseMovableItems; // +
    public List<int> ChooseMovableItems; // +
    public List<int> CursorMovableItems; // +
    public List<int> ZoneItems;
    public int CursorZonesIdx; // +
    public int RestTimeVr;
    public int AllTimeVr;
}

[Serializable]
public class StateTransform
{
    public StateTransform(Transform t)
    {
        position = new List<float> { t.position.x, t.position.y, t.position.z };
        rotation = new List<float> { t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w };
    }
    public List<float> position;
    public List<float> rotation;
}