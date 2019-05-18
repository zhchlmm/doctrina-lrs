using AutoMapper;
using Doctrina.Application.Infrastructure.AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doctrina.Application.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private MapperConfiguration _config;

        [TestInitialize]
        public void Initialize()
        {
            var profile = new AutoMapperProfile();
            _config = new MapperConfiguration(cfg => {
                cfg.AddProfile(profile);
            });
        }

        [TestMethod]
        public void TestMethod1()
        {
            _config.AssertConfigurationIsValid();
        }
    }
}
