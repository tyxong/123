using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Helpers
{
    public interface IHelperForStringFriends
    {
        string AddNewFriendToString(int newFriendId, string friendsString);
        string RemoveFriendToString(int friendId, string friendsString);
        bool IsContain(int newFriendId, string friendsString);
        List<int> GetAllFriendsIdFromString(string stringOfFriends);
    }
}
