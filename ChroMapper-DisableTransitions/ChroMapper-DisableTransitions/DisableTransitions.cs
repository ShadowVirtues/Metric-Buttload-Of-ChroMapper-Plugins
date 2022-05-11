namespace ChroMapper_DisableTransitions
{
    [Plugin("DisableTransitions")]
    public class DisableTransitions
    {
        [Init]
        private void Init()
        {
            UnityEngine.Object.FindObjectOfType<PersistentUI>().EnableTransitions = false;
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
