using TeckShop.Core.Options;

namespace TeckShop.Infrastructure.Health
{
    /// <summary>
    /// The health options.
    /// </summary>
    public class HealthOptions : IOptionsRoot
    {
        /// <summary>
        /// Gets or sets  a value indicating whether to postgres.
        /// </summary>
        public bool Postgres { get; set; }

        /// <summary>
        /// Gets or sets  a value indicating whether to redis.
        /// </summary>
        public bool Redis { get; set; }

        /// <summary>
        /// Gets or sets  a value indicating whether to rabbit mq.
        /// </summary>
        public bool RabbitMq { get; set; }

        /// <summary>
        /// Gets or sets  a value indicating whether to hangfire.
        /// </summary>
        public bool Hangfire { get; set; }

        /// <summary>
        /// Gets or sets  a value indicating whether to send grid.
        /// </summary>
        public bool SendGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether signal R.
        /// </summary>
        public bool SignalR { get; set; }

        /// <summary>
        /// Gets or sets  a value indicating whether to kubernetes.
        /// </summary>
        public bool Kubernetes { get; set; }

        /// <summary>
        /// Gets or sets  a value indicating whether to application status.
        /// </summary>
        public bool ApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether open id connect server.
        /// </summary>
        public bool OpenIdConnectServer { get; set; }
    }
}
