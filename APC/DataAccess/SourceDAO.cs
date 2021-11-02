using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using APC.Models.Shared;
using APC.Models.Source;
using ApcupsdLib;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwoMQTT.Interfaces;

namespace APC.DataAccess;

public interface ISourceDAO : IPollingSourceDAO<SlugMapping, Response, object, object>
{
}

/// <summary>
/// An class representing a managed way to interact with a source.
/// </summary>
public class SourceDAO : ISourceDAO
{
    /// <summary>
    /// Initializes a new instance of the SourceDAO class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientFactory"></param>
    /// <returns></returns>
    public SourceDAO(ILogger<SourceDAO> logger, IDictionary<string, ApcupsdClient> clients)
    {
        this.Logger = logger;
        this.Clients = clients;
    }

    /// <summary>
    /// Fetch one response from the source.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Response?> FetchOneAsync(SlugMapping key,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await Task.Run(() =>
                this.FetchAsync(key.SerialNo, cancellationToken)
            );
        }
        catch (Exception e)
        {
            var msg = e switch
            {
                HttpRequestException => "Unable to fetch from the APC UPS server",
                KeyNotFoundException => "Unable to deserialize response from the APC UPS server",
                _ => "Unable to send to the APC UPS server"
            };
            this.Logger.LogError(msg + " - {serial}; {exception}", key.SerialNo, e);
            return null;
        }
    }

    /// <summary>
    /// Fetch one response from the source.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected Response? FetchAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        this.Logger.LogDebug("Connecting to APC UPS for {serialNo}", serialNo);

        if (!this.Clients.TryGetValue(serialNo, out var client))
        {
            return null;
        }

        var status = client.GetStatus();
        return status == null ?
            null :
            new Response
            {
                LoTrans = status.LoTrans ?? 0,
                HiTrans = status.HiTrans ?? 0,
                RetPct = status.RetPct ?? 0,
                ITemp = status.ITemp ?? 0,
                AlarmDel = status.AlarmDel ?? 0,
                BattV = status.BattV ?? 0,
                LineFreq = status.LineFreq ?? 0,
                LastXfer = status.LastXfer,
                NumXfers = status.NumXfers ?? 0,
                TOnBatt = status.TOnBatt ?? 0,
                CumOnBatt = status.CumOnBatt ?? 0,
                XOffBatt = status.XOffBatt ?? DateTime.Now,
                SelfTest = status.SelfTest ?? false,
                StatFlag = status.StatFlag,
                SerialNo = status.SerialNo,
                BattDate = status.BattDate ?? DateTime.Now,
                NomInV = status.NomInV ?? 0,
                NomBattV = status.NomBattV ?? 0,
                OutputV = status.OutputV ?? 0,
                NomPower = status.NomPower ?? 0,
                MaxLineV = status.MaxLineV ?? 0,
                MinTimeL = status.MinTimeL ?? 0,
                Apc = status.Apc,
                Date = status.Date,
                Hostname = status.Hostname,
                Version = status.Version,
                UpsName = status.UpsName,
                Cable = status.Cable,
                Driver = status.Driver,
                Model = status.Model,
                MaxTime = status.MaxTime ?? 0,
                UpsMode = status.UpsMode,
                MasterUpd = status.MasterUpd ?? DateTime.Now,
                EndApc = status.EndApc,
                LineV = status.LineV ?? 0,
                LoadPct = status.LoadPct ?? 0,
                BCharge = status.BCharge ?? 0,
                TimeLeft = status.TimeLeft ?? 0,
                MBattChg = status.MBattChg ?? 0,
                StartTime = status.StartTime,
                Firmware = status.Firmware,
                Status = status.Status.ToString(),
            };
    }

    /// <summary>
    /// The logger used internally.
    /// </summary>
    private readonly ILogger<SourceDAO> Logger;

    /// <summary>
    /// The apcupsd client.
    /// </summary>
    private readonly IDictionary<string, ApcupsdClient> Clients;
}
