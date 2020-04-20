using Nure.Update;
using Xunit;

namespace Nure.UnitTests.Update
{
    public class RuntimeParametersTests
    {
        [Fact]
        public void ValidRuntimeParametersAreAccepted()
        {
            Assert.True(GivenValidRuntimeParameters().Validate());
        }

        [Fact]
        public void MissingRuntimeParametersAreDetected()
        {
            var missingPassword = new RuntimeParameters { DirectoryPath = "/bin/bash", Username = "user" };
            var missingPath = new RuntimeParameters { Username = "user", Password = "password" };
            var missingUser = new RuntimeParameters { DirectoryPath = "/bin/bash", Password = "password" };

            Assert.False(missingPassword.Validate());
            Assert.False(missingPath.Validate());
            Assert.False(missingUser.Validate());
        }

        private static RuntimeParameters GivenValidRuntimeParameters()
        {
            return new RuntimeParameters {
                DirectoryPath = "C:/System32",
                Help = false,
                Username = "admin",
                Password = "12345"
            };
        }
    }
}
