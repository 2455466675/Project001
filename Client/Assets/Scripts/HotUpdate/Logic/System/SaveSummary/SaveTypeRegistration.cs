using GameFramework.Core;

namespace GameFramework.Logic
{
    public static class SaveTypeRegistration
    {
        public static void Register()
        {
            MDebug.Log("注册存档类型");
            SaveTypeRegistry.Register<SampleData>(1);
        }
    }
}
