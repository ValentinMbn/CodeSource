public class Command
{
    #region Events
    public delegate void CommandAction();
    public CommandAction OnExecute;
    #endregion

    #region Methods
    public bool Execute()
    {
        OnExecute?.Invoke();
        return true;
    }
    #endregion
}