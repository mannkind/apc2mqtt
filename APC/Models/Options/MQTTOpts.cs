using TwoMQTT.Models;

namespace APC.Models.Options;

/// <summary>
/// The sink options
/// </summary>
public record MQTTOpts : MQTTManagerOptions
{
    public const string Section = "APC:MQTT";
    public const string TopicPrefixDefault = "home/apc";
    public const string DiscoveryNameDefault = "apc";
}
