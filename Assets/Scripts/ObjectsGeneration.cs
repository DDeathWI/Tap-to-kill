using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsGeneration : MonoBehaviour {

    [SerializeField] private List<PrimitiveType> PrimitiveTypes;

    [SerializeField] private List<GameObject> currentGameObjects;

    [Space(10)]

    [SerializeField] private Material RedObject;
    [SerializeField] private Material WhiteObject;

    [Space(10)]
    [SerializeField] private float Scaling = 1;

    private string[] state_string = new string[2] { "false", "true" };

    private float maxXposition = 0;
    private float maxYposition = 0;

    private Coroutine generationCoroutine;

    private void OnEnable()
    {
        currentGameObjects = new List<GameObject>();

        // Calculate end imposible object position on the Screen
        float sideCoef = Mathf.Min(Screen.width, Screen.height)/ Camera.main.orthographicSize;

        // 0.5f - half 1 
        // Object will fully creating on screen
        maxXposition = Screen.width / sideCoef - 0.5f * Scaling;
        maxYposition = Screen.height / sideCoef - 0.5f * Scaling;

        Events.GameStart += StartGeneration;
        Events.GameOver += GameOver;
    }

    private void OnDisable()
    {
        Events.GameStart -= StartGeneration;
        Events.GameOver -= GameOver;

    }

    private void StartGeneration()
    {
        if (generationCoroutine != null)
            StopCoroutine(generationCoroutine);

        generationCoroutine = StartCoroutine(IE_Generating());
    }

    private IEnumerator IE_Generating()
    {
        int counter = 1;

        while (true)
        {
            if (!GameController.Pause && GameController.CanCreate)
            {
                GameController.CanCreate = false;

                int randomType = Random.Range(0, PrimitiveTypes.Count);

                float randomXposition = Random.Range(-maxXposition, maxXposition);
                float randomYposition = Random.Range(-maxYposition, maxYposition);

                GameObject go = GameObject.CreatePrimitive(PrimitiveTypes[randomType]);
                ObjectController objectController = go.AddComponent<ObjectController>();

                go.name = counter.ToString();
                go.transform.parent = transform;
                go.transform.position = new Vector3(randomXposition, randomYposition);
                go.transform.localScale *= Scaling;

                int objectState = Random.Range(0, state_string.Length);
                bool state = bool.Parse(state_string[objectState]);

                objectController.SetMaterial(state ? WhiteObject : RedObject, state);

                currentGameObjects.Add(go);

                counter++;
            }

            yield return null;

        }
    }

    private void GameOver()
    {
        if (generationCoroutine != null)
            StopCoroutine(generationCoroutine);

        for (int i = 0; i < currentGameObjects.Count; i++)
        {
            if (currentGameObjects[i] != null)
            {
                Destroy(currentGameObjects[i]);
            }
        }
    }

}
