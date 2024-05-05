using Amazon.S3;
using Amazon.S3.Transfer;

namespace CsvFileUpload
{
    public class FileUploadService(string awsAccessKeyId, string awsSecretAccessKey, string awsRegion, string s3BucketName)
    {
        private readonly string awsAccessKeyId = awsAccessKeyId;
        private readonly string awsSecretAccessKey = awsSecretAccessKey;
        private readonly string awsRegion = awsRegion;
        private readonly string s3BucketName = s3BucketName;

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "No file uploaded";
            }

            try
            {
                using var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, Amazon.RegionEndpoint.GetBySystemName(awsRegion));
                var transferUtility = new TransferUtility(client);
                await transferUtility.UploadAsync(file.OpenReadStream(), s3BucketName, file.FileName);
                return "File uploaded successfully";
            }
            catch (AmazonS3Exception e)
            {
                return $"AWS S3 error: {e.Message}";
            }
            catch (Exception e)
            {
                return $"Unexpected error: {e.Message}";
            }
        }
    }
}