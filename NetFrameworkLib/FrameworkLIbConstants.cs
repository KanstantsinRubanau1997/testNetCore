using Newtonsoft.Json;

namespace NetFrameworkLib
{
    public static class FrameworkLIbConstants
    {
        public static string GetSmth()
        {
            return JsonConvert.SerializeObject(new { Name = "Name", Value = 12 });
        }
    }
}
