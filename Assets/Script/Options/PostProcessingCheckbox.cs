public class PostProcessingCheckbox : UICheckbox
{
    protected override bool GetStartEnabled()
    {
        return UserOptions.EnablePostProcessing;
    }

    protected override void UpdateSave()
    {
        UserOptions.EnablePostProcessing = CheckboxEnabled;
    }
}
