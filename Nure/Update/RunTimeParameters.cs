namespace Nure.Update
{
    public class RunTimeParameters
    {
        public bool Validate(bool p_IsGithubCompliant)
        {
            bool isValid = !string.IsNullOrEmpty(DirectoryPath) &&
                !string.IsNullOrEmpty(Username) &&
                !string.IsNullOrEmpty(Password);

            if (p_IsGithubCompliant) {
                return isValid && !string.IsNullOrEmpty(GitApiKey);
            }

            return isValid;
        }

        public string DirectoryPath { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Help { get; set; }

        public string GitApiKey { get; set; }
    }
}
