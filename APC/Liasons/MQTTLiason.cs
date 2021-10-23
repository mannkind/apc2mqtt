using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using APC.Models.Options;
using APC.Models.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwoMQTT;
using TwoMQTT.Interfaces;
using TwoMQTT.Liasons;
using TwoMQTT.Models;
using TwoMQTT.Utils;

namespace APC.Liasons
{
    /// <summary>
    /// An class representing a managed way to interact with MQTT.
    /// </summary>
    public class MQTTLiason : MQTTLiasonBase<Resource, object, SlugMapping, SharedOpts>, IMQTTLiason<Resource, object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="generator"></param>
        /// <param name="sharedOpts"></param>
        public MQTTLiason(ILogger<MQTTLiason> logger, IMQTTGenerator generator, IOptions<SharedOpts> sharedOpts) :
            base(logger, generator, sharedOpts)
        {
        }

        /// <inheritdoc />
        public IEnumerable<(string topic, string payload)> MapData(Resource input)
        {
            var results = new List<(string, string)>();
            var slug = this.Questions
                .Where(x => x.SerialNo == input.SerialNo)
                .Select(x => x.Slug)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrEmpty(slug))
            {
                this.Logger.LogDebug("Unable to find slug for {serialNo}", input.SerialNo);
                return results;
            }

            this.Logger.LogDebug("Found slug {slug} for incoming data for {serialNo}", slug, input.SerialNo);
            results.AddRange(new[]
                {
                    (this.Generator.StateTopic(slug, nameof(Resource.BCharge)), input.BCharge.ToString("0")),
                    (this.Generator.StateTopic(slug, nameof(Resource.BattV)), input.BattV.ToString("N2")),
                    (this.Generator.StateTopic(slug, nameof(Resource.LastXfer)), input.LastXfer),
                    (this.Generator.StateTopic(slug, nameof(Resource.LoadPct)), input.LoadPct.ToString("0")),
                    (this.Generator.StateTopic(slug, nameof(Resource.TimeLeft)), input.TimeLeft.ToString("N1")),
                    (this.Generator.StateTopic(slug, nameof(Resource.Status)), input.Status),
                    (this.Generator.StateTopic(slug, nameof(Resource.NumXfers)), input.NumXfers.ToString("0")),
                }
            );

            return results;
        }

        /// <inheritdoc />
        public IEnumerable<(string slug, string sensor, string type, MQTTDiscovery discovery)> Discoveries()
        {
            var discoveries = new List<(string, string, string, MQTTDiscovery)>();
            var assembly = Assembly.GetAssembly(typeof(Program))?.GetName() ?? new AssemblyName();
            var mapping = new[]
            {
                new { Sensor = nameof(Resource.BCharge), Type = Const.SENSOR, UOM = "%", Icon = "mdi:gauge" },
                new { Sensor = nameof(Resource.BattV), Type = Const.SENSOR, UOM = "V", Icon = "mdi:flash" },
                new { Sensor = nameof(Resource.LastXfer), Type = Const.SENSOR, UOM = "", Icon = "mdi:information-outline" },
                new { Sensor = nameof(Resource.LoadPct), Type = Const.SENSOR, UOM = "%", Icon = "mdi:gauge" },
                new { Sensor = nameof(Resource.TimeLeft), Type = Const.SENSOR, UOM = "Mins", Icon = "mdi:clock-alert" },
                new { Sensor = nameof(Resource.Status), Type = Const.SENSOR, UOM = "", Icon = "mdi:information-outline" },
                new { Sensor = nameof(Resource.NumXfers), Type = Const.SENSOR, UOM = "", Icon = "mdi:information-outline" },
            };

            foreach (var input in this.Questions)
            {
                foreach (var map in mapping)
                {
                    var discovery = this.Generator.BuildDiscovery(input.Slug, map.Sensor, assembly, false);

                    if (!string.IsNullOrEmpty(map.UOM))
                    {
                        discovery = discovery with
                        {
                            UnitOfMeasurement = map.UOM,
                        };
                    }

                    if (!string.IsNullOrEmpty(map.Icon))
                    {
                        discovery = discovery with
                        {
                            Icon = map.Icon,
                        };
                    }

                    discoveries.Add((input.Slug, map.Sensor, map.Type, discovery));
                }
            }

            return discoveries;
        }
    }
}