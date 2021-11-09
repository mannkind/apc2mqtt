using System.Collections.Generic;
using APC.DataAccess;
using APC.Liasons;
using APC.Models.Options;
using APC.Models.Shared;
using ApcupsdLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TwoMQTT;
using TwoMQTT.Extensions;
using TwoMQTT.Interfaces;
using TwoMQTT.Managers;

await ConsoleProgram<Resource, object, SourceLiason, MQTTLiason>.
    ExecuteAsync(args,
        envs: new Dictionary<string, string>()
        {
            {
                $"{MQTTOpts.Section}:{nameof(MQTTOpts.TopicPrefix)}",
                MQTTOpts.TopicPrefixDefault
            },
            {
                $"{MQTTOpts.Section}:{nameof(MQTTOpts.DiscoveryName)}",
                MQTTOpts.DiscoveryNameDefault
            },
        },
        configureServices: (HostBuilderContext context, IServiceCollection services) =>
        {
            services
                .AddOptions<SharedOpts>(SharedOpts.Section, context.Configuration)
                .AddOptions<SourceOpts>(SourceOpts.Section, context.Configuration)
                .AddOptions<TwoMQTT.Models.MQTTManagerOptions>(MQTTOpts.Section, context.Configuration)
                .AddHttpClient()
                .AddSingleton<IThrottleManager, ThrottleManager>(x =>
                {
                    var opts = x.GetRequiredService<IOptions<SourceOpts>>();
                    return new ThrottleManager(opts.Value.PollingInterval);
                })
                .AddSingleton<ISourceDAO, SourceDAO>()
                .AddSingleton<IDictionary<string, ApcupsdClient>>(x =>
                {
                    var opts = x.GetRequiredService<IOptions<SharedOpts>>();
                    var dict = new Dictionary<string, ApcupsdClient>();
                    foreach (var resource in opts.Value.Resources)
                    {
                        dict[resource.SerialNo] = new ApcupsdClient(resource.Host, resource.Port);
                    }

                    return dict;
                });
        });