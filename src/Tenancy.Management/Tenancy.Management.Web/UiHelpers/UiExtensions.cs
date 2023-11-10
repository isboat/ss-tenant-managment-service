using Tenancy.Management.Web.Models;

namespace Tenancy.Management.Web.UiHelpers
{
    public static class UiExtensions
    {
        public static string Initialize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            var parts = input.Split(' ');
            var initials = "";
            foreach ( var part in parts )
            {
                if(string.IsNullOrEmpty(part)) continue;

                initials += part.ToUpperInvariant()[0];
            }

            return initials;
        }
        public static string ProfileIconStyle()
        {
            var list = new List<string> { "blue", "orange", "pink", "brown", "green" };
            var rnd = new Random();
            return $"data-initials-{list[rnd.Next(list.Count)]}";
        }

        public static int TenantUsersPercent(int usersLimit, int usersCount)
        {
            var x = Math.Round((double)usersCount / usersLimit, 2, MidpointRounding.AwayFromZero);
            return Convert.ToInt32(x * 100);
        }

        public static string ProgressBarColor(int percent)
        {
            if (percent >= 60) return "bg-danger";
            if (percent >= 40) return "bg-warning";
            return "bg-success";
        }
    }
}
