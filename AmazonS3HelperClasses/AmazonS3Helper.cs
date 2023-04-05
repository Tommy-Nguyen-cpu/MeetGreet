using System;
using Amazon.S3;
using Amazon.S3.Model;
using MeetGreet.Data;
using MySqlConnector;

namespace MeetGreet.AmazonS3HelperClasses
{
	public class AmazonS3Helper
	{
        private static string BUCKET_NAME = "meetgreet-image-store";

        private MeetgreetContext context;
		private MySqlConnection connect;
        private IAmazonS3 s3Client;
        private int TIMEOUT_DURATION = 6;


        public AmazonS3Helper(MeetgreetContext context, MySqlConnection connect)
		{
			this.context = context;
			this.connect = connect;

            this.s3Client = instantiateS3Client(connect);

		}


        public static void uploadImageAndSaveToDatabase(string bucketName, string objectName, string filePath, byte[] imageBytes)
        {
            // TODO: Reorganize Code
        }

        public string retrieveS3BucketImageURL(string s3Key)
        {
            return GeneratePresignedURL(s3Client, BUCKET_NAME, s3Key, TIMEOUT_DURATION);
        }


        private static IAmazonS3 instantiateS3Client(MySqlConnection connect) {
            connect.Open();

            // Sends a request for API KEYS FOR AWS
            MySqlCommand command = new MySqlCommand("SELECT * FROM AWSAPIKey WHERE ID=1", connect);

            IAmazonS3 s3Client = new AmazonS3Client();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                s3Client = new AmazonS3Client(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), Amazon.RegionEndpoint.USEast1);
            }
            reader.Close();

            return s3Client;
        }

        /// <summary>
        /// Generate a presigned URL that can be used to access the file named
        /// in the objectKey parameter for the amount of time specified in the
        /// duration parameter.
        /// </summary>
        /// <param name="client">An initialized S3 client object used to call
        /// the GetPresignedUrl method.</param>
        /// <param name="bucketName">The name of the S3 bucket containing the
        /// object for which to create the presigned URL.</param>
        /// <param name="objectKey">The name of the object to access with the
        /// presigned URL.</param>
        /// <param name="duration">The length of time for which the presigned
        /// URL will be valid.</param>
        /// <returns>A string representing the generated presigned URL.</returns>
        private static string GeneratePresignedURL(IAmazonS3 client, string bucketName, string objectKey, double duration)
        {
            string urlString = string.Empty;
            try
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddHours(duration),
                };
                urlString = client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error:'{ex.Message}'");
            }

            return urlString;
        }

    }
}

