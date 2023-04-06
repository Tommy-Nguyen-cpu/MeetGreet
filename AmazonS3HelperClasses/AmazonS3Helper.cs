using System;
using System.Collections;
using System.Security.AccessControl;
using Amazon.S3;
using Amazon.S3.Model;
using MeetGreet.Data;
using MeetGreet.Models;
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

        // Returns Object Name
        public async Task<string> uploadImageToS3Bucket(byte[] imageBytes, Event userEvent)
        {
            return await uploadEventImages(Convert.ToBase64String(imageBytes), userEvent);
        }



        private string generateObjectName()
        {
            return DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HH:mm:ss") + "-" + Guid.NewGuid().ToString();
        }

        private async Task<string> uploadEventImages(string byteString, Event userEvent)
        {
            if(connect.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("The connection state is already open.");
            }
            else {
                connect.Open();
            }

            // GET S3 CLIENT FROM SQL

            // Sends a request for API KEYS FOR AWS
            MySqlCommand command = new MySqlCommand("SELECT * FROM AWSAPIKey WHERE ID=1", connect);

            IAmazonS3 s3Client = new AmazonS3Client("", "", Amazon.RegionEndpoint.USEast1);

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                s3Client = new AmazonS3Client(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), Amazon.RegionEndpoint.USEast1);
            }

            reader.Close();

            string objectName = generateObjectName();
            byte[] byteArray = Convert.FromBase64String(byteString);

            if (await UploadFileAsync(s3Client, BUCKET_NAME, objectName, "", byteArray))
            {
                //saveImageToDatabase(userEvent, objectName);
                return objectName;
            }
            else
            {
                return "ERROR";
            }
        }

        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="bucketName">The Amazon S3 bucket to which the object
        /// will be uploaded.</param>
        /// <param name="objectName">The object to upload.</param>
        /// <param name="filePath">The path, including file name, of the object
        /// on the local computer to upload.</param>
        /// <returns>A boolean value indicating the success or failure of the
        /// upload procedure.</returns>
        private static async Task<bool> UploadFileAsync(
            IAmazonS3 client,
            string bucketName,
            string objectName,
            string filePath,
            byte[] imageBytes)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                FilePath = filePath,
            };
            using (var ms = new MemoryStream(imageBytes))
            {
                request.InputStream = ms;

                var response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
                    Console.WriteLine($"{response.ToString}");
                    return false;
                }
            }
        }

        public string retrieveS3BucketImageURL(string s3Key)
        {
            return GeneratePresignedURL(s3Client, BUCKET_NAME, s3Key, TIMEOUT_DURATION);
        }


        private static IAmazonS3 instantiateS3Client(MySqlConnection connect) {
            if (connect.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("The connection state is already open.");
            }
            else
            {
                connect.Open();
            }


            // Sends a request for API KEYS FOR AWS
            MySqlCommand command = new MySqlCommand("SELECT * FROM AWSAPIKey WHERE ID=1", connect);

            IAmazonS3 s3Client = new AmazonS3Client("", "", Amazon.RegionEndpoint.USEast1);

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

