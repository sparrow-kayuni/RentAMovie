namespace RentAMovie.Services.LoginService
{
    public interface ILoginService
    {
        public bool verifyLoginCredentials(string userName, string password);
        // public LoginSession createLoginSession();
    }
}