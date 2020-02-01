using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneEunumCreatorWindow : EditorWindow
{

    private const string m_sceneFileName = "Scenes";
    private Button m_createButton;
    private Label m_stateLable;
    private ListView m_sceneBuildList;
    private ListView m_sceneEnumList;
    private VisualTreeAsset m_sceneListItem;


    [MenuItem("SimpleSceneSwitch/SceneEnumCreationWindow")]
    static void Init()
    {
        SceneEunumCreatorWindow window = (SceneEunumCreatorWindow)GetWindow(typeof(SceneEunumCreatorWindow));
        window.Show();
    }


    private void OnEnable()
    {
        LoadVisualItems();
        SubscribeVisualItems();
        UpdateState();
        UpdateSceneBuildList();
        UpdateSceneEnumList();
    }


    private void LoadVisualItems()
    {
        // Reference to the root of the window.
        var root = rootVisualElement;
        root.styleSheets.Add(Resources.Load<StyleSheet>("WindowUI/WindowStyle"));

        // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("WindowUI/WindowTree");
        quickToolVisualTree.CloneTree(root);

        // Setup the ui items
        m_sceneBuildList = root.Query<ListView>(name: "sceneBuild-list").First();
        m_sceneEnumList = root.Query<ListView>(name: "sceneEnum-list").First();
        m_createButton = root.Query<Button>(name: "create-button").First();
        m_stateLable = root.Query<Label>(name: "state").First();
        m_sceneListItem = Resources.Load<VisualTreeAsset>("WindowUI/SceneListItem");
    }

    private void SubscribeVisualItems()
    {
        m_createButton.clickable.clicked += () => OnCreateAsset();
    }

    private void UpdateState()
    {

        string[] scenesInEnum = Enum.GetNames(typeof(Scenes));

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)        
            scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

        if (scenesInEnum.SequenceEqual(scenes))
        {
            m_stateLable.text = "State: Done";
            m_stateLable.style.backgroundColor = Color.green;
        }
        else
        {
            m_stateLable.text = "State: Update";
            m_stateLable.style.backgroundColor = Color.red;
        }
    }

    private void UpdateSceneBuildList()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        m_sceneBuildList.Clear();

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            VisualElement listItem = m_sceneListItem.CloneTree();
            Label sceneName = listItem.Query<Label>(name: "scene-name");
            Label sceneIndex = listItem.Query<Label>(name: "scene-number");
            m_sceneBuildList.Add(listItem);
            sceneName.text = scenes[i].ToString();
            sceneIndex.text = i.ToString();
        }
    }

    private void UpdateSceneEnumList()
    {
        string[] scenesInEnum = Enum.GetNames(typeof(Scenes));

        m_sceneEnumList.Clear();

        for (int i = 0; i < scenesInEnum.Length; i++)
        {
            VisualElement listItem = m_sceneListItem.CloneTree();
            Label sceneName = listItem.Query<Label>(name: "scene-name");
            Label sceneIndex = listItem.Query<Label>(name: "scene-number");
            m_sceneEnumList.Add(listItem);
            sceneName.text = scenesInEnum[i];
            sceneIndex.text = i.ToString();
        }
    }

    private void OnCreateAsset()
    {
        EnumWriter.CreateScenesFile();
        UpdateState();
        UpdateSceneBuildList();
        UpdateSceneEnumList();
    }


}
