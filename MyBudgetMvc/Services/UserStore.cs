using System.Collections.Concurrent;
using MyBudgetMvc.Models;

namespace MyBudgetMvc.Services
{
    public class UserStore
    {
        private readonly ConcurrentDictionary<string, RegisterViewModel> _users = new();

        public bool IsUserNameTaken(string userName)
        {
            return _users.ContainsKey(userName);
        }

        public void SaveUser(RegisterViewModel model)
        {
            _users[model.UserName] = model;
        }

        public RegisterViewModel GetUser(string userName)
        {
            _users.TryGetValue(userName, out var user);
            return user;
        }
    }
}
