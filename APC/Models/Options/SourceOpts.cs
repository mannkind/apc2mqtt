using System;

namespace APC.Models.Options
{
    /// <summary>
    /// The source options
    /// </summary>
    public record SourceOpts
    {
        public const string Section = "APC";

        /// <summary>
        //
        /// </summary>
        /// <value></value>
        public string Host { get; init; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Port { get; init; } = 3551;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan PollingInterval { get; init; } = new(0, 1, 7);
    }
}
