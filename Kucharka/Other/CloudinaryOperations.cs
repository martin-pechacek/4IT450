using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Semestralka.Other
{
    public class CloudinaryOperations
    {
        private string cloud_name = "dn9v4khef";
        private string api_key = "388956466687414";
        private string secret_key = "xjqi1fBmKrMRvncSn9Y_AA25V2E";

        private Account getAccount() 
        {
            Account account = new Account(
                cloud_name,
                api_key,
                secret_key);

            return account;
        }

        public void Upload(HttpPostedFileBase file, int id)
        {
            Cloudinary cloudinary = new Cloudinary(getAccount());

            //control if file exists
            if(file.ContentLength == 0)
            {
                //set upload parameters for cloudinary
                var uploadParams = new ImageUploadParams()
                {

                    File = new FileDescription(@"http://res.cloudinary.com/dn9v4khef/image/upload/v1496165011/4IT450/no-image.png"),
                    PublicId = Convert.ToString("4IT450/" + id),
                    Transformation = new Transformation().Crop("fill").Width(240).Height(260)
                };

                cloudinary.Upload(uploadParams);
            } 
            else
            {
                string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDirectory);

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(tempDirectory, fileName);

                file.SaveAs(path);
                //set upload parameters for cloudinary
                var uploadParams = new ImageUploadParams()
                {

                    File = new FileDescription(@path),
                    PublicId = Convert.ToString("4IT450/" + id),
                    Transformation = new Transformation().Crop("fill").Width(240).Height(260)
                };

                cloudinary.Upload(uploadParams);   
             
            }            
        }

        public string getImageUrl(int id) 
        {
            Cloudinary cloudinary = new Cloudinary(getAccount());

            string url = "http://res.cloudinary.com/dn9v4khef/image/upload/4IT450/" + id + ".jpg";

            return url;
        }
    }
}