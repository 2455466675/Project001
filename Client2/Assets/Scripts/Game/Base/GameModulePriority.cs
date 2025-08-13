namespace GameFramework 
{
    public enum GameModulePriority
    {
        ResourceManager = -100,
        ConfigManager = -95,
        AssemblyManager = -90,
        EventManager = -80,
        InputManager = -70,

        EntityManager = -10,

        UIManager = 100,
    }
}