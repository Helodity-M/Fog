public class NoFailCheckbox : UICheckbox
{
    protected override bool GetStartEnabled()
    {
        return UserOptions.NoFail;
    }

    protected override void UpdateSave()
    {
        UserOptions.NoFail = CheckboxEnabled;
    }
}
