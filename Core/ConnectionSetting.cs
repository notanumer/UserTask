using Microsoft.Extensions.Configuration;

namespace Core
{
    public class ConnectionSetting
    {
        public string ConnectionString { get; }

        public ConnectionSetting(IConfiguration configuration)
        {
            var t = configuration.GetSection("Dal");
            ConnectionString = configuration.GetSection("Dal")["ConnectionString"];
        }
    }
}
