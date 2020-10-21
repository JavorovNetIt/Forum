namespace Forum.App.Commands
{
    using Contracts;

    public class LogOutCommand : ICommand
    {
        private IMenuFactory menuFactory;
        private ISession session;
        public LogOutCommand(ISession session, IMenuFactory menuFactory)
        {
            this.session = session;
            this.menuFactory = menuFactory;
        }
        public IMenu Execute(params string[] args)
        {
            this.session.Reset();

            return this.menuFactory.CreateMenu("MainMenu");
        }
    }
}
