namespace GUPS.EasyParallelScene.Demo
{
    public class ScoreManager : GUPS.EasyParallelScene.Singleton.PersistentSingleton<ScoreManager>
    {
        public static int Score { get; set; }
    }
}