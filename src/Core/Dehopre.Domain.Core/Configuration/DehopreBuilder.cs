namespace Dehopre.Domain.Core.Configuration
{
    using System;
    using Dehopre.Domain.Core.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public class DehopreBuilder : IDehopreConfigurationBuilder
    {
        public DehopreBuilder(IServiceCollection services) => this.Services = services ?? throw new ArgumentNullException(nameof(services));

        public IServiceCollection Services { get; }
    }
}
