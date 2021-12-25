namespace BusinessLogic
{
    public class PostgreUrlParser
    {
        public ConnectionProperties Parse(string url)
        {
            url = url.Substring("postgres://".Length);
            
            var splitByDog = url.Split("@");

            var usernamePassword = splitByDog[0];
            var hostDatabase = splitByDog[1];

            return new ConnectionProperties
                {
                    UserName = usernamePassword.Split(":")[0],
                    Password = usernamePassword.Split(":")[1],
                    HostAndPort = hostDatabase.Split('/')[0],
                    DatabaseName = hostDatabase.Split('/')[1],
                };
        }
    }
}