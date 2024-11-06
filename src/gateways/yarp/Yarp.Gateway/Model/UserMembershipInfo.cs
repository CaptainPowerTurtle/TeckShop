namespace Yarp.Gateway.Model
{
    public class UserMembershipInfo
    {
        public string UserId { get; set; } = string.Empty;
        public SortedList<string, string> ProjectsMemberships { get; set; } = new SortedList<string, string>(); //projectId, membershipId
    }
}
