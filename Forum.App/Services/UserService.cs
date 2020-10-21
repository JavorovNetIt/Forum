namespace Forum.App.Services
{
    using Contracts;
    using DataModels;
    using Data;
    using System;
    using System.Linq;

    public class UserService : IUserService
    {
        private ForumData forumData;
        private ISession session;

        public UserService(ForumData forumData, ISession session)
        {
            this.forumData = forumData;
            this.session = session;
        }

        public User GetUserById(int userId)
        {
            var user = this.forumData.Users.FirstOrDefault(u => u.Id == userId);

            return user;
        }

        public string GetUserName(int userId)
        {
            var username = this.forumData.Users.FirstOrDefault(u => u.Id == userId).Username;

            return username;
        }

        public bool TryLogInUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            User user = this.forumData.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return false;
            }

            this.session.Reset();
            this.session.LogIn(user);

            return true;
        }

        public bool TrySignUpUser(string username, string password)
        {
            bool validUsername = !string.IsNullOrEmpty(username) && username.Length > 3;
            bool validPassword = !string.IsNullOrEmpty(password) && password.Length > 3;

            if (!validUsername || !validPassword )
            {
                throw new ArgumentException("Username and Password have to be more than 3 symbols long!");
            }

            bool userAlreadyExist = this.forumData.Users.Any(u => u.Username.Equals(username));

            if (userAlreadyExist)
            {
                throw new ArgumentException("Username taken!");
            }

            int userId = this.forumData.Users.LastOrDefault()?.Id + 1 ?? 1;

            User user = new User(userId, username, password);

            this.forumData.Users.Add(user);
            forumData.SaveChanges();

            this.TryLogInUser(username, password);

            return true;
        }
    }
}
