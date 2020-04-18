namespace Nure.Update
{
    public class RuntimeParameters
    {
        public bool Validate()
        {
            bool isValid = !string.IsNullOrEmpty(DirectoryPath) &&
                !string.IsNullOrEmpty(Username) &&
                !string.IsNullOrEmpty(Password);

            return isValid;
        }

        public string DirectoryPath { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Help { get; set; }
    }
}
