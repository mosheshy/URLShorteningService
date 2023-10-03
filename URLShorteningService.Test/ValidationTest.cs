using Microsoft.Extensions.Configuration;
using URLShorteningService.Validations;

namespace URLShorteningService.Test
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void IsShortUrlValidTestMethod()
        {
            int shortUrlLength = 8;
            var inMemorySettings = new Dictionary<string, string>
            {
                {"shortUrlLength", shortUrlLength.ToString()}
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
                .Build();

            URLShorteningValidation uRLShorteningValidation = new URLShorteningValidation(configuration);
            var isValid = uRLShorteningValidation.IsShortUrlValid("EulGgQWE");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsShortUrlValidfailureTestMethod()
        {
            int shortUrlLength = 8;
            var inMemorySettings = new Dictionary<string, string> 
            {
                {"shortUrlLength", shortUrlLength.ToString()}  
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)            
                .Build();
            URLShorteningValidation uRLShorteningValidation = new URLShorteningValidation(configuration);
            var isValid = uRLShorteningValidation.IsShortUrlValid("ew12");
            Assert.IsFalse(isValid);
        }
    }
}