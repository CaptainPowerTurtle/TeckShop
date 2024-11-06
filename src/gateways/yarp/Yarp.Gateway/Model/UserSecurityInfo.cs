namespace Yarp.Gateway.Model
{
    public class UserSecurityInfo
    {
        public string UserId { get; set; } = string.Empty;
        public SortedList<string, string> ProjectsAndRoles { get; set; } = new SortedList<string, string>();
    }
}
