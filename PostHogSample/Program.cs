using PostHog;
using System.Threading.Tasks;

namespace PostHogSample
{
    sealed class Program
    {
        static PostHogClient posthog = new(new PostHogOptions
        {
            ProjectApiKey = "phc_tW9wZBm0mdKidnGXWZPZiLTJo5uQvBjln7w0tO0zcMK",
            HostUrl = new Uri("https://us.i.posthog.com"),
        });


        static async Task Main(string[] args)
        //static void Main(string[] args)
        {
#pragma warning disable CA5394
#pragma warning disable CA1305
            string userid = Random.Shared.Next(1000, 2000).ToString();
            string userAlias = $"TEST - {userid}";


            posthog.Capture(userid, "user_signed_up");
            posthog.Capture(
                userid,
                "event_name",
                personPropertiesToSet: new() { ["name"] = "Max Hedgehog" },
                personPropertiesToSetOnce: new() { ["initial_url"] = "/blog" }
            );
            await posthog.AliasAsync(userid, userAlias);

            posthog.Capture(
                userid,
                "app_start",
                groups: [new Group("company", "company_id_in_your_db")]);

            await posthog.GroupIdentifyAsync(
                type: "company",
                key: "company_id_in_your_db",
                name: "EE GAMES CO.,LTD",
                properties: new()
                {
                    ["employees"] = 11
                },
                distinctId: userid
            );

            if (await posthog.GetFeatureFlagAsync(
                    "experiment-feature-flag-key",
                    userid)
                is { VariantKey: "variant-name" })
            {
                // Do something
            }

            if (await posthog.IsFeatureEnabledAsync("flag-key", userid))
            {
                // Feature is enabled
            }
            else
            {
                // Feature is disabled
            }

            var flag = await posthog.GetFeatureFlagAsync(
                "flag-key",
                userid
            );

            // replace "variant-key" with the key of your variant
            if (flag is { VariantKey: "variant-key" })
            {
                // Do something differently for this userid
                // Optional: fetch the payload
                var matchedPayload = flag.Payload;
            }

            if (await posthog.GetFeatureFlagAsync(
                    "flag-key",
                    userid)
               )
            {
                // Do something differently for this user
            }

            posthog.Capture(
                userid,
                "login",
                properties: new()
                {
                    // replace feature-flag-key with your flag key.
                    // Replace "variant-key" with the key of your variant
                    ["$feature/feature-flag-key"] = "variant-key"
                }
            );

            posthog.Capture(
                userid,
                "levelup",
                properties: null,
                groups: null,
                sendFeatureFlags: true
            );

            var flags = await posthog.GetAllFeatureFlagsAsync(
                userid
            );

            var personFlag = await posthog.GetFeatureFlagAsync(
                "flag-key",
                userid,
                personProperties: new() { ["plan"] = "premium" });

            var groupFlag = await posthog.GetFeatureFlagAsync(
                "flag-key",
                userid,
                options: new FeatureFlagOptions
                {
                    Groups =
                    [
                        new Group("your_group_type", "your_group_id")
                    {
                        ["group_property_name"] = "your group value"
                    },
                    new Group(
                        "another_group_type",
                        "another_group_id")
                    {
                        ["group_property_name"] = "another group value"
                    }
                    ]
                });
            var bothFlag = await posthog.GetFeatureFlagAsync(
                "flag-key",
                userid,
                options: new FeatureFlagOptions
                {
                    PersonProperties = new() { ["property_name"] = "value" },
                    Groups =
                    [
                        new Group("your_group_type", "your_group_id")
                    {
                        ["group_property_name"] = "your group value"
                    },
                    new Group(
                        "another_group_type",
                        "another_group_id")
                    {
                        ["group_property_name"] = "another group value"
                    }
                    ]
                });

            posthog.Capture(
                userid,
                "app_quit",
                properties: null,
                groups: null,
                sendFeatureFlags: true
            );
            Console.WriteLine("Hello, World!");
        }
    }
}
