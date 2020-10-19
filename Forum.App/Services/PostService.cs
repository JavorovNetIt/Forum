namespace Forum.App.Services
{
    using Contracts;
    using Forum.Data;
    using System;
    using System.Collections.Generic;

    public class PostService : IPostService
    {
        private ForumData forumData;
        private ISession session;

        public PostService(ForumData forumData, ISession session)
        {
            this.forumData = forumData;
            this.session = session;
        }
        public int AddPost(int userId, string postTitle, string postCategory, string postContent)
        {
            throw new NotImplementedException();
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public string GetCategoryName(int categoryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            throw new NotImplementedException();
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
