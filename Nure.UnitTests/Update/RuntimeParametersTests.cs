using System.IO;
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
            RuntimeParameters missingPassword = new RuntimeParameters {
                DirectoryPath = "/bin/bash",
                Username = "user"
            };
            RuntimeParameters missingPath = new RuntimeParameters {
                Username = "user",
                Password = "password"
            };
            RuntimeParameters missingUser = new RuntimeParameters {
                DirectoryPath = "/bin/bash",
                Password = "password"
            };

            Assert.False(missingPassword.Validate());
            Assert.False(missingPath.Validate());
            Assert.False(missingUser.Validate());
        }

        [Fact]
        public void MaliciousRuntimeParametersAreDenied()
        {
            RuntimeParameters maliciousConfig = new RuntimeParameters {
                DirectoryPath = "rm -rf",
                Username = "user",
                Password = "password"
            };

            Assert.False(maliciousConfig.Validate());
        }

        private static RuntimeParameters GivenValidRuntimeParameters() =>
            new RuntimeParameters {
                DirectoryPath = Directory.GetCurrentDirectory(),
                Help = false,
                Username = "admin",
                Password = "12345"
            };
    }
}
