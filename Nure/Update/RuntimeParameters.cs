using System.IO;

namespace Nure.Update
{
    public class RuntimeParameters
    {
        public string DirectoryPath { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Help { get; set; }

        public bool Validate()
        {
            bool isValid = Directory.Exists(DirectoryPath) && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

            return isValid;
        }
    }
}
