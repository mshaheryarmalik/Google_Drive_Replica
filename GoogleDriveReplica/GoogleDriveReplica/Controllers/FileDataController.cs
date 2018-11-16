using Drive.BAL;
using Drive.Entities;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace GoogleDriveReplica.Controllers
{
    public class FileDataController : ApiController
    {

        [HttpPost]
        public void UploadFile(int uid, int pid)
        {
            if(HttpContext.Current.Request.Files.Count > 0)
            {
                try
                {
                    foreach(var fileName in HttpContext.Current.Request.Files.AllKeys)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[fileName];
                        if(file != null)
                        {
                            FileDTO dto = new FileDTO();
                            dto.Name = file.FileName;
                            dto.FileExt = VirtualPathUtility.GetExtension(file.FileName);
                            dto.UploadedOn = DateTime.Now;
                            dto.CreatedBy = uid;
                            dto.ParentFolderId = pid;
                            dto.UniqueName = Guid.NewGuid().ToString();
                            dto.FileSizeInKB = file.ContentLength / 1024;
                            dto.ContentType = file.ContentType;
                            var rootPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
                            var fileSavePath = System.IO.Path.Combine(rootPath, dto.UniqueName+dto.FileExt);
                            file.SaveAs(fileSavePath);
                            FileBO.Save(dto);
                        }
                    }
                }
                catch(Exception e)
                {
                }
              }
            }
        [HttpGet]
        public Object DownloadFile(String uname)
        {
            var rootPath = System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles");
            var dto = FileBO.GetFileByUniqueName(uname);

            if(dto != null)
            {
                HttpResponseMessage response = new  HttpResponseMessage(HttpStatusCode.OK);
                var fileFullPath = System.IO.Path.Combine(rootPath, dto.UniqueName+dto.FileExt);
                byte[] file = System.IO.File.ReadAllBytes(fileFullPath);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(file);

                response.Content = new ByteArrayContent(file);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(dto.ContentType);
                response.Content.Headers.ContentDisposition.FileName = dto.Name;
                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }
        }
        [HttpGet]
        public int DeleteFile(int fid)
        {
            return FileBO.DeleteFile(fid);
        }
        [HttpGet]
        public Object GetThumbnail(String uniqueName)
        {
            var rootPath = System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles");

            var FileDTO = FileBO.GetFileByUniqueName(uniqueName);
            var fileFullPath = Path.Combine(rootPath, FileDTO.UniqueName + FileDTO.FileExt);

            ShellFile shellFile = ShellFile.FromFilePath(fileFullPath);
            Bitmap shellThumb = shellFile.Thumbnail.MediumBitmap;

            if (FileDTO != null)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                byte[] file = ImageToBytes(shellThumb); // Call to private function ImageToBytes

                MemoryStream ms = new MemoryStream(file);

                response.Content = new ByteArrayContent(file);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(FileDTO.ContentType);
                response.Content.Headers.ContentDisposition.FileName = FileDTO.Name;
                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }

        }
        private byte[] ImageToBytes(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
      }
    }