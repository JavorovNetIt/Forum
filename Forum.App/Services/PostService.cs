namespace Forum.App.Services
{
    using Contracts;
    using Forum.App.ViewModels;
    using Forum.Data;
    using Forum.DataModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostService : IPostService
    {
        private ForumData forumData;
        private ISession session;
        private IUserService userService;

        public PostService(ForumData forumData, ISession session, IUserService userService)
        {
            this.forumData = forumData;
            this.session = session;
            this.userService = userService;
        }
        public int AddPost(int userId, string postTitle, string postCategory, string postContent)
        {
            bool emptyTitle = string.IsNullOrEmpty(postTitle);
            bool emptyCategory = string.IsNullOrEmpty(postCategory);
            bool emptyContent = string.IsNullOrEmpty(postContent);

            if (emptyTitle || emptyContent || emptyCategory)
            {
                throw new ArgumentException("All fields must be filled");
            }

            Category category = this.EnsureCategory(postCategory);

            int postId = this.forumData.Posts.Any() ? this.forumData.Posts.Last().Id + 1 : 1;

            User author = this.userService.GetUserById(userId);

            Post post = new Post(postId, postTitle, postContent, category.Id, author.Id, new List<int>());

            this.forumData.Posts.Add(post);
            author.Posts.Add(postId);
            category.Posts.Add(post.Id);
            this.forumData.SaveChanges();

            return post.Id;
        }

        private Category EnsureCategory(string postCategory)
        {
            var category =  this.forumData.Categories.FirstOrDefault(c => c.Name == postCategory);

            if (category == null)
            {
                int categoryId = this.forumData.Categories.LastOrDefault()?.Id + 1 ?? 1;
                category = new Category(categoryId, postCategory, new List<int>());
                this.forumData.Categories.Add(category);
            }

            return category;
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            if (string.IsNullOrWhiteSpace(replyContents))
            {
                throw new ArgumentException("Cannot add an empty reply");
            }

            Post post = this.forumData.Posts.Find(p => p.Id == postId);
            User author = this.userService.GetUserById(userId);

            int replyId = this.forumData.Replies.LastOrDefault()?.Id + 1 ?? 1;

            Reply reply = new Reply(replyId, replyContents, userId, postId);

            this.forumData.Replies.Add(reply);

            post.Replies.Add(reply.Id);

            this.forumData.SaveChanges();

        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            IEnumerable<ICategoryInfoViewModel> categories = this.forumData
                .Categories
                .Select(c => new CategoryInfoViewModel(c.Id, c.Name, c.Posts.Count));

            return categories;
        }

        public string GetCategoryName(int categoryId)
        {
            string categoryName = this.forumData.Categories.Find(c => c.Id == categoryId)?.Name;

            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentException($"Category with id {categoryId} not found");
            }

            return categoryName;
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            var postInfoViewModels = this.forumData.Posts.Where(p => p.CategoryId == categoryId).Select(p => new PostInfoViewModel(p.Id, p.Title, p.Replies.Count));

            return postInfoViewModels;
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            var post = this.forumData.Posts.FirstOrDefault(p => p.Id == postId);

            string authorName = this.userService.GetUserName(post.AuthorId);

            IPostViewModel postViewModel = new PostViewModel(post.Title, authorName, post.Content, this.GetPostReplies(postId));

            return postViewModel;
        }

        private IEnumerable<IReplyViewModel> GetPostReplies(int postId)
        {

            var replies = this.forumData.Replies.Where(r => r.PostId == postId).Select(r => new ReplyViewModel(this.userService.GetUserName(r.AuthorId), r.Content));

            return replies;
        }
    }
}
