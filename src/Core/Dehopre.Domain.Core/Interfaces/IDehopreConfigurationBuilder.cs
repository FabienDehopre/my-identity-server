namespace Dehopre.Domain.Core.Interfaces
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IDehopreConfigurationBuilder
    {
        IServiceCollection Services { get; }
    }
}
