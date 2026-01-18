namespace SanderSaveli.GravityMaze
{
    public class GameObjectDisableButton : DisabledButton
    {
        protected override void DisableButton()
        {
            _button.gameObject.SetActive(false);
        }

        protected override void EnableButton()
        {
            _button.gameObject.SetActive(true);
        }
    }
}
