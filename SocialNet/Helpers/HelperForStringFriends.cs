using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SocialNet.Helpers
{
    public class HelperForStringFriends : IHelperForStringFriends
    {
        public string AddNewFriendToString(int newFriendId,string friendsString)
        {
            if (!IsContain(newFriendId, friendsString))
            {
                StringBuilder stroka = new StringBuilder(friendsString);
                stroka.Append("id" + newFriendId + ",");
                return stroka.ToString();
            }
            return friendsString;
        }

        public string RemoveFriendToString(int friendId, string friendsString)
        {
            if (IsContain(friendId, friendsString))
            {
                string idOfUser = "id" + friendId + ",";
                friendsString = friendsString.Replace(idOfUser, "");
            }
            return friendsString;
        }

        public bool IsContain(int newFriendId, string friendsString)
        {
            if (friendsString != null)
            {
                if (friendsString.Contains("id" + newFriendId.ToString() + ","))
                {
                    return true;
                }
            }
            return false;
        }

        public List<int> GetAllFriendsIdFromString(string stringOfFriends)
        {
            List<int> allFriends = new List<int>();
            if (stringOfFriends != null)
            {
                while (stringOfFriends.Length > 0)
                {
                    allFriends.Add((Convert.ToInt32(stringOfFriends.Substring(2, stringOfFriends.IndexOf(",")-2))));
                    stringOfFriends = stringOfFriends.Remove(0, stringOfFriends.IndexOf(",")+1);
                }
            }
            return allFriends;
        }
    }
}