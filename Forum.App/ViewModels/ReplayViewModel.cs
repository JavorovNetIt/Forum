namespace Forum.App.ViewModels
{
    using Contracts;
    public class ReplayViewModel : ContentViewModel, IReplyViewModel
    {
        public ReplayViewModel(string author, string content) : base(content)
        {
            this.Author = author;
        }
        public string Author { get; }
    }
}
