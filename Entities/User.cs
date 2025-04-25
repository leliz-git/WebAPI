namespace Entities
{
    public class User
    {

        public int userId { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public User() { }
        public User(String userName, String password, String firstName, String lastName)
        {
            this.userName = userName;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
        }

    }
}
