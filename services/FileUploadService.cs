using Amazon.S3;
using Amazon.S3.Transfer;

namespace CsvFileUpload
{
    public class FileUploadService
    {
        private readonly string awsAccessKeyId;
        private readonly string awsSecretAccessKey;
        private readonly string awsRegion;
        private readonly string s3BucketName;

        public FileUploadService(string awsAccessKeyId, string awsSecretAccessKey, string awsRegion, string s3BucketName)
        {
            this.awsAccessKeyId = awsAccessKeyId;
            this.awsSecretAccessKey = awsSecretAccessKey;
            this.awsRegion = awsRegion;
            this.s3BucketName = s3BucketName;
        }

        public async Task<List<FileUploadResult>> UploadFilesAsync(List<IFormFile> files)
        {
            var results = new List<FileUploadResult>();

            if (files == null || files.Count == 0)
            {
                results.Add(new FileUploadResult { FileName = "No files", IsSuccess = false, ErrorMessage = "No files uploaded" });
                return results;
            }

            try
            {
                using var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, Amazon.RegionEndpoint.GetBySystemName(awsRegion));
                var transferUtility = new TransferUtility(client);

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        results.Add(new FileUploadResult { FileName = "Empty file", IsSuccess = false, ErrorMessage = "Empty file uploaded" });
                        continue;
                    }

                    await transferUtility.UploadAsync(file.OpenReadStream(), s3BucketName, file.FileName);
                    results.Add(new FileUploadResult { FileName = file.FileName, IsSuccess = true });
                }
            }
            catch (AmazonS3Exception e)
            {
                var errorMessage = $"AWS S3 error: {e.Message}";
                foreach (var file in files)
                {
                    results.Add(new FileUploadResult { FileName = file.FileName, IsSuccess = false, ErrorMessage = errorMessage });
                }
            }
            catch (Exception e)
            {
                var errorMessage = $"Unexpected error: {e.Message}";
                foreach (var file in files)
                {
                    results.Add(new FileUploadResult { FileName = file.FileName, IsSuccess = false, ErrorMessage = errorMessage });
                }
            }

            return results;
        }
    }

    public class FileUploadResult
    {
        public string? FileName { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
