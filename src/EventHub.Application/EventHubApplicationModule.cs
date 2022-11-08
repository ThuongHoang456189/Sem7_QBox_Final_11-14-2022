using Payment;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Aws;
using Volo.Abp.Modularity;
using Volo.CmsKit.Public;

namespace EventHub
{
    [DependsOn(
        typeof(EventHubDomainModule),
        typeof(EventHubApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(CmsKitPublicApplicationModule),
        typeof(PaymentApplicationModule)
    )]
    public class EventHubApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<EventHubApplicationModule>();
            });
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseAws(Aws =>
                    {
                        Aws.AccessKeyId = "AKIA5K6JNDI2YK67EEU2";
                        Aws.SecretAccessKey = "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";
                        Aws.Region = "ap-southeast-1";
                        Aws.ContainerName = "qbox-cloud-storage-bucket";
                        //Aws.UseCredentials = true;
                        //Aws.UseTemporaryCredentials = false;
                        //Aws.UseTemporaryFederatedCredentials = false;
                        //Aws.ProfileName = "qbox";
                        //Aws.ProfilesLocation = @"C:\Users\HoangThuong\.aws\credentials.csv";
                        //Aws.Name = "Administrator";
                        //Aws.Policy = "Policy1665825511620";
                        //Aws.DurationSeconds = 1000;
                        //Aws.CreateContainerIfNotExists = true;
                    });
                });
            });
        }
    }
}
