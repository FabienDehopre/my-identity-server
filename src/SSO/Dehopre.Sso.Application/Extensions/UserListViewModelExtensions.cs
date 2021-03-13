namespace Dehopre.Sso.Application.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Dehopre.Sso.Application.ViewModels.UserViewModels;

    public static class UserListViewModelExtensions
    {
        public static UserListViewModel WithId(this IEnumerable<UserListViewModel> users, string subjectId) => users.FirstOrDefault(f => f.Id == subjectId);
    }
}
