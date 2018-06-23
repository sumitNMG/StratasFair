using Amazon.S3;
using Amazon.S3.Encryption;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Common
{
    public static class AwsS3Bucket
    {
        static string bucketName = ConfigurationManager.AppSettings["AwsBucketName"].ToString();
        static string awsAccessKeyId = ConfigurationManager.AppSettings["AwsAccessKey"].ToString();
        static string awsSecretAccessKeyId = ConfigurationManager.AppSettings["AwsSecretAccessKey"].ToString();



        public static string AccessPath()
        {
            return ConfigurationManager.AppSettings["AwsViewAccessPath"].ToString();
        }
        static string FindBucketLocation(IAmazonS3 client)
        {
            string bucketLocation;
            GetBucketLocationRequest request = new GetBucketLocationRequest()
            {
                BucketName = bucketName
            };
            GetBucketLocationResponse response = client.GetBucketLocation(request);
            bucketLocation = response.Location.ToString();
            return bucketLocation;
        }

        public static string CreateABucket()
        {
            string result = "1";
            try
            {

                using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
                {

                    if (!(AmazonS3Util.DoesS3BucketExist(client, bucketName)))
                    {
                        //CreateABucket(client);
                        PutBucketRequest putRequest1 = new PutBucketRequest
                        {
                            BucketName = bucketName,
                            UseClientRegion = true
                        };

                        PutBucketResponse response1 = client.PutBucket(putRequest1);
                        result = response1.HttpStatusCode.ToString().ToLower(); // ok
                    }
                    else
                    {
                        // Retrieve bucket location.
                        //string bucketLocation = FindBucketLocation(client);
                        result = "alreadyexist";
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    result = "Check the provided AWS Credentials.";
                    //Console.WriteLine("Check the provided AWS Credentials.");
                    //Console.WriteLine("For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    result = "Error occurred. Message:" + amazonS3Exception.Message + " when writing an object;";
                }
            }

            return result;
        }


        public static bool DoesFolderExist(string folderName)
        {
            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = bucketName;
            request.Prefix = folderName + "/";
            request.MaxKeys = 1;

            using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
            {
                ListObjectsResponse response = client.ListObjects(request);
                return response.S3Objects.Count > 0;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName">foldername or foldername1/foldername2 or foldername1/foldername2/foldername3 etc</param>
        /// <returns></returns>
        public static int CreateFolder(string folderName)
        {
            int result = -1;
            var folderKey = folderName + "/"; //end the folder name with "/"

            var request = new PutObjectRequest();

            request.BucketName  = bucketName;

            request.StorageClass = S3StorageClass.Standard;
            request.ServerSideEncryptionMethod = ServerSideEncryptionMethod.None;

            //request.CannedACL = S3CannedACL.BucketOwnerFullControl;

            request.Key = folderKey;

            request.ContentBody = string.Empty;

            using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
            {
                PutObjectResponse response = client.PutObject(request);
                if (response.ETag != null)
                {
                    // folder created successfully
                    result = 0;
                }
            }
           // string contentType = MimeMapping.GetMimeMapping("someFileName.pdf");
            return result;
        }


        public static int CreateFile(string fileName, string filePath)
        {
            int result = -1;

            //Push the given object into S3 Bucket
            PutObjectRequest request = new PutObjectRequest
            {
                Key = fileName,
                FilePath = filePath,
                ContentType = MimeMapping.GetMimeMapping(fileName),
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead,
                StorageClass = S3StorageClass.Standard,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.None
            };


            using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
            {
                PutObjectResponse response = client.PutObject(request);
                if (response.ETag != null)
                {
                    // file created successfully
                    result = 0;
                }
            }
            return result;
        }


        /// <summary>
        /// It may be file (filename1.txt) or folder (foldername1/)
        /// folder must be empty for delete
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int DeleteObject(string objectName)
        {
            int result = -1;

            //Push the given object into S3 Bucket
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                  {
                      BucketName = bucketName,
                      Key = objectName
                  };


            using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
            {
                client.DeleteObject(deleteObjectRequest);
            }
            return result;
        }



        public static void DownloadObject(string fileName,string filePath)
        {
            string dest = System.IO.Path.Combine(HttpRuntime.CodegenDir, fileName);
            string path = filePath.Replace("~/","") + fileName;
            using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
            {
                GetObjectRequest request = new GetObjectRequest(){
                      BucketName = bucketName,
                      Key = path
                  };

                using (GetObjectResponse response = client.GetObject(request))
                {
                    response.WriteResponseStreamToFile(dest, false);
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.TransmitFile(dest);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();

                // Clean up temporary file.
                System.IO.File.Delete(dest);
            }
        }

        public static string DownloadObject2(string fileName, string filePath)
        {
            string dest = System.IO.Path.Combine(HttpRuntime.CodegenDir, fileName);
            string path = filePath.Replace("~/", "") + fileName;
            using (var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId, Amazon.RegionEndpoint.USEast1))
            {
                GetObjectRequest request = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = path
                };

                using (GetObjectResponse response = client.GetObject(request))
                {
                    response.WriteResponseStreamToFile(dest, false);
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.TransmitFile(dest);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();

                
                // Clean up temporary file.
                //System.IO.File.Delete(dest);
            }
            return dest;
        }
       


        //https://stackoverflow.com/questions/27009599/how-to-create-subdirectory-in-amazon-s3-bucket
        //https://stackoverflow.com/questions/9944671/amazon-s3-creating-folder-through-net-sdk-vs-through-management-console
       
    }
}
