using Drive.BAL;
using Drive.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Web;
using System.Net.Http.Headers;

namespace GoogleDriveReplica.Controllers
{
    public class MetaDataController : ApiController
    {
        [HttpPost]
        public void GetMetaData(int uid, int pid)
        {
            FolderDTO main = FolderBO.GetFolder(uid, pid);
            List<FolderDTO> folderList = FolderBO.GetAllMainFoldersByUser(uid,pid);
            String dest = "D:\\Extras\\MetaData.pdf";
            var writer = new PdfWriter(dest);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            document.Add(new Paragraph("Meta Information of Folder: " + main.Name));
            document.Add(new Paragraph("Name: " + main.Name));
            document.Add(new Paragraph("Type: Folder"));
            document.Add(new Paragraph("Size: None"));
            if (main.ParentFolderId == -1)
                document.Add(new Paragraph("Parent: Root"));
            else
            {
                FolderDTO folder1 = FolderBO.GetFolder(main.CreatedBy, main.ParentFolderId);
                document.Add(new Paragraph("Parent: " + folder1.Name));
            }
            int count = folderList.Count;
            Queue<FolderDTO> foldersQueue = new Queue<FolderDTO>();
            for (int i = 0; i < count; i++)
                foldersQueue.Enqueue(folderList[i]);
            
            List<FileDTO> fileList = FileBO.GetAllFiles(uid,pid);

            count = fileList.Count;
            Queue<FileDTO> filesQueue = new Queue<FileDTO>();
            for (int i = 0; i < count; i++)
                filesQueue.Enqueue(fileList[i]);

            while (foldersQueue.Count > 0)
            {
                FolderDTO folder = foldersQueue.Dequeue();
                document.Add(new Paragraph("Name: " + folder.Name));
                document.Add(new Paragraph("Type: Folder"));
                document.Add(new Paragraph("Size: None"));
                if (folder.ParentFolderId == -1)
                    document.Add(new Paragraph("Parent: Root"));
                else
                {
                    FolderDTO folder1 = FolderBO.GetFolder(folder.CreatedBy,folder.ParentFolderId);
                    document.Add(new Paragraph("Parent: " + folder1.Name));
                }
                document.Add(new Paragraph(""));
                folder.ParentFolderId = folder.Id;
                folderList = FolderBO.GetAllMainFoldersByUser(folder.CreatedBy, folder.ParentFolderId);
                count = folderList.Count;
                for (int i = 0; i < count; i++)
                    foldersQueue.Enqueue(folderList[i]);

                fileList = FileBO.GetAllFiles(folder.CreatedBy,folder.ParentFolderId);
                count = fileList.Count;
                for (int i = 0; i < count; i++)
                    filesQueue.Enqueue(fileList[i]);
            }



            while (filesQueue.Count > 0)
            {
                FileDTO file = filesQueue.Dequeue();
                document.Add(new Paragraph("Name: " + file.Name));
                document.Add(new Paragraph("Type: File"));
                document.Add(new Paragraph("Size: " + file.FileSizeInKB + " KB"));
                if (file.ParentFolderId == -1)
                    document.Add(new Paragraph("Parent: Root"));
                else
                {
                    FolderDTO folder1 = FolderBO.GetFolder(uid, file.ParentFolderId);
                    document.Add(new Paragraph("Parent: " + folder1.Name));
                }
                document.Add(new Paragraph(""));
            }
            document.Add(new Paragraph(""));
            document.Close();
            return;
        }

        [HttpGet]
        public HttpResponseMessage DownloadMetaData()
        {
            var rootPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var fileFullPath = System.IO.Path.Combine(rootPath, "MetaData.pdf");

            byte[] file = System.IO.File.ReadAllBytes(fileFullPath);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(file);

            response.Content = new ByteArrayContent(file);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentDisposition.FileName = "MetaData.pdf";
            return response;
        }
    }
}