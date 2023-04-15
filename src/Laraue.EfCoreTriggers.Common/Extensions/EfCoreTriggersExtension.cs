using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    /// <inheritdoc />
    public class EfCoreTriggersExtension : IDbContextOptionsExtension
    {
        private readonly Action<IServiceCollection> _modifyServices;
    
        /// <summary>
        /// Initializes a new instance of <see cref="EfCoreTriggersExtension"/>.
        /// </summary>
        /// <param name="addDefaultServices"></param>
        /// <param name="modifyServices"></param>
        public EfCoreTriggersExtension(Action<IServiceCollection> addDefaultServices, Action<IServiceCollection>? modifyServices)
        {
            _modifyServices = addDefaultServices;

            if (modifyServices is not null)
            {
                _modifyServices += modifyServices;
            }
        
            Info = new EfCoreTriggersExtensionInfo(this);
        }
    
        /// <inheritdoc />
        public void ApplyServices(IServiceCollection services)
        {
            _modifyServices(services);
        }
    
        /// <inheritdoc />
        public void Validate(IDbContextOptions options)
        {
        }

        /// <inheritdoc />
        public DbContextOptionsExtensionInfo Info { get; }
    }
}