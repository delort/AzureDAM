namespace Avanade.AzureDAM.Commands.Authenticate
{
    public class AuthenticateAPIKey : Command<AuthenticationResult>
    {
        private string _apiKey;

        public AuthenticateAPIKey With(string apiKey)
        {
            _apiKey = apiKey;

            return this;
        }

        public AuthenticationResult Do()
        {
            return new AuthenticationResult()
            {
                KeyIsValid = false
            };
        }
    }
}