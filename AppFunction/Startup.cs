using AppFunction;
using DataAccess.Common;
using DataAccess.Common.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using ServiceBus.Interfaces;
using ServiceBus.ServiceBus;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AppFunction
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; private set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            AddDbContext(builder);
            AddBusinessRules(builder);
            AddDataAccess(builder);
            AddServiceBus(builder);

            builder.Services.AddHealthChecks();
        }

        public void AddDbContext(IFunctionsHostBuilder builder)
        {
            ConventionRegistry.Register("ConventionPack", new ConventionPack { new IgnoreExtraElementsConvention(true), new IgnoreIfNullConvention(true) }, _ => true);
            MongoSettings mongoSettings = new MongoSettings
            {
                ConnectionString = Environment.GetEnvironmentVariable("MongoConnectionString"),
                DatabaseName = Environment.GetEnvironmentVariable("MongoDbName")
            };

            builder.Services.AddSingleton<IMainContext>(new MainContext(mongoSettings));
            BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
        }

        public void AddServiceBus(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IServiceBusSend>(s => new ServiceBusSend(Environment.GetEnvironmentVariable("ServiceBusConnection")));
        }

        public void AddBusinessRules(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<BusinessLogic.Interfaces.IDnaMutant, BusinessLogic.BusinessRules.DnaMutant>();
        }

        public void AddDataAccess(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IDnaMutantRepository, DnaMutantRepository>();
        }

    }
}
