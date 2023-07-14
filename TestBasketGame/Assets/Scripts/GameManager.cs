using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InGameUIController inGameUIController;
    [SerializeField] private DollyCartController dollyCartController;
    [SerializeField] private ConveyorController conveyorController;

    [SerializeField] public GameplayState gameplayState;

    [SerializeField] private Vector2Int fruitRange;

    [SerializeField] private TaskConfig taskConfig;

    [SerializeField] private CharacterController characterController;

    private int currentAmountCollected;

    private void Awake()
    {
        if (characterController == null)
            characterController = FindObjectOfType<CharacterController>();

        inGameUIController.SmoothShowScene();

        characterController.OnCollected += OnCollected;
        inGameUIController.OnChangeLevelPressed += ReloadScene;
    }

    private void OnDestroy()
    {
        characterController.OnCollected -= OnCollected;
        inGameUIController.OnChangeLevelPressed -= ReloadScene;
    }

    private void OnCollected(FruitType fruitType)
    {
        currentAmountCollected++;

        if (currentAmountCollected >= taskConfig.fruitAmount)
        {
            gameplayState = GameplayState.PreparationForFinish;
        }
    }

    private void Update()
    {
        switch (gameplayState)
        {
            case GameplayState.Prepering:
                {
                    GenerateTasks();

                    conveyorController.Activate();

                    gameplayState = GameplayState.Gameplay;
                }
                break;
            case GameplayState.Gameplay:
                break;
            case GameplayState.PreparationForFinish:
                {
                    characterController.ActiveteDance();
                    dollyCartController.Activete();
                    conveyorController.Hide();

                    inGameUIController.ShowChangeLevelView();
                    inGameUIController.Clear();

                    gameplayState = GameplayState.Finish;
                }
                break;
            case GameplayState.Finish:
                break;
            default:
                break;
        }
    }

    private void GenerateTasks()
    {
        int randomFruitAmount = Random.Range(fruitRange.x, fruitRange.y + 1);

        FruitType randomFruitType = RandomEnumValue<FruitType>();

        TaskConfig taskConfig = new TaskConfig
        {
            fruitAmount = randomFruitAmount,
            fruitType = randomFruitType
        };

        this.taskConfig = taskConfig;

        inGameUIController.SetText(taskConfig);
        characterController.SetTask(taskConfig);
    }

    public static T RandomEnumValue<T>()
    {
        var values = System.Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
