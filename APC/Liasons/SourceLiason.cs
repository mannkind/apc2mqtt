using System.Threading;
using System.Threading.Tasks;
using APC.DataAccess;
using APC.Models.Options;
using APC.Models.Shared;
using APC.Models.Source;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwoMQTT.Interfaces;
using TwoMQTT.Liasons;

namespace APC.Liasons;

/// <summary>
/// An class representing a managed way to interact with a source.
/// </summary>
public class SourceLiason : PollingSourceLiasonBase<Resource, SlugMapping, ISourceDAO, SharedOpts>, ISourceLiason<Resource, object>
{
    public SourceLiason(ILogger<SourceLiason> logger, ISourceDAO sourceDAO,
        IOptions<SourceOpts> opts, IOptions<SharedOpts> sharedOpts) :
        base(logger, sourceDAO, sharedOpts)
    {
        this.Logger.LogInformation(
            "PollingInterval: {pollingInterval}\n" +
            "Resources: {@resources}\n" +
            "",
            opts.Value.PollingInterval,
            sharedOpts.Value.Resources
        );
    }

    /// <inheritdoc />
    protected override async Task<Resource?> FetchOneAsync(SlugMapping key, CancellationToken cancellationToken)
    {
        var result = await this.SourceDAO.FetchOneAsync(key, cancellationToken);
        return result switch
        {
            Response => new Resource
            {
                LoTrans = result.LoTrans,
                HiTrans = result.HiTrans,
                RetPct = result.RetPct,
                ITemp = result.ITemp,
                AlarmDel = result.AlarmDel,
                BattV = result.BattV,
                LineFreq = result.LineFreq,
                LastXfer = result.LastXfer,
                NumXfers = result.NumXfers,
                TOnBatt = result.TOnBatt,
                CumOnBatt = result.CumOnBatt,
                XOffBatt = result.XOffBatt,
                SelfTest = result.SelfTest,
                StatFlag = result.StatFlag,
                SerialNo = result.SerialNo,
                BattDate = result.BattDate,
                NomInV = result.NomInV,
                NomBattV = result.NomBattV,
                OutputV = result.OutputV,
                NomPower = result.NomPower,
                MaxLineV = result.MaxLineV,
                MinTimeL = result.MinTimeL,
                Apc = result.Apc,
                Date = result.Date,
                Hostname = result.Hostname,
                Version = result.Version,
                UpsName = result.UpsName,
                Cable = result.Cable,
                Driver = result.Driver,
                Model = result.Model,
                MaxTime = result.MaxTime,
                UpsMode = result.UpsMode,
                MasterUpd = result.MasterUpd,
                EndApc = result.EndApc,
                LineV = result.LineV,
                LoadPct = result.LoadPct,
                BCharge = result.BCharge,
                TimeLeft = result.TimeLeft,
                MBattChg = result.MBattChg,
                StartTime = result.StartTime,
                Firmware = result.Firmware,
                Status = result.Status,
            },
            _ => null,
        };
    }
}
