namespace Dehopre.Sso.Application.CloudServices.Storage
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.Settings;

    public class AwsStorageService : IStorageService
    {
        private readonly StorageSettings privateSettings;

        public AwsStorageService(StorageSettings privateSettings) => this.privateSettings = privateSettings;

        public async Task<string> Upload(FileUploadViewModel image, CancellationToken cancellationToken = default)
        {
            var client = this.GetClient();

            var putRequest = new PutObjectRequest
            {
                BucketName = this.privateSettings.StorageName,
                Key = image.Filename,
                ContentType = image.FileType,
                InputStream = new MemoryStream(Convert.FromBase64String(image.Value)),
                CannedACL = S3CannedACL.PublicRead,
            };
            if (!image.VirtualLocation.IsNullOrEmpty())
            {
                putRequest.Metadata.Add("Location", image.VirtualLocation);
            }

            _ = await client.PutObjectAsync(putRequest, cancellationToken);

            return $"https://{this.privateSettings.StorageName}.s3.{this.privateSettings.Region}.amazonaws.com/{image.Filename}";
        }


        public async Task RemoveFile(string fileName, string virtualLocation, CancellationToken cancellationToken = default)
        {
            var client = this.GetClient();
            _ = await client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = this.privateSettings.StorageName, Key = fileName }, cancellationToken);
        }

        private IAmazonS3 GetClient() => new AmazonS3Client(new BasicAWSCredentials(this.privateSettings.Username, this.privateSettings.Password), RegionEndpoint.GetBySystemName(this.privateSettings.Region));
    }
}
