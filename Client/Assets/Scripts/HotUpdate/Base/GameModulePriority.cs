namespace GameFramework 
{
    public enum GameModulePriority
    {
        AssetsManager = -100,
        ConfigManager = -95,

        SceneManager = -80,
        InputManager = -70,

        StateManager = -20,

        EntityManager = -10,

        SaveManager = 0,

        InputController = 90,

        UIManager = 100,
    }
}