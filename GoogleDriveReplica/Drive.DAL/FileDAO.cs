using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Drive.Entities;

namespace Drive.DAL
{
    public static class FileDAO
    {
          
           public static int Save(FileDTO dto)
           {
               String sqlQuery = "";
               if (dto.Id > 0)
               {
                   sqlQuery = String.Format("Update dbo.Files Set Name='{0}',  Where UserID={1}", dto.Name, dto.Id);
               }
               else
               {
                   sqlQuery = String.Format("INSERT INTO dbo.Files(Name, ParentFolderId,FileExt, FileSizeInKB,CreatedBy,UploadedOn,IsActive,UniqueName,ContentType) VALUES('{0}',{1},'{2}',{3},{4},'{5}',{6},'{7}','{8}' )",
                       dto.Name, dto.ParentFolderId, dto.FileExt, dto.FileSizeInKB,dto.CreatedBy,dto.UploadedOn,1,dto.UniqueName,dto.ContentType);
               }
               using (DBHelper helper = new DBHelper())
               {
                   return helper.ExecuteQuery(sqlQuery);
               }
           }

           public static int DeleteUser(int fid)
           {
               String sqlQuery = String.Format("Update dbo.Files Set IsActive=0 Where Id={0}", fid);

               using (DBHelper helper = new DBHelper())
               {
                   return helper.ExecuteQuery(sqlQuery);
               }
           }

           public static List<FileDTO> GetAllFiles(int uid, int pid)
           {
               String sqlQuery = String.Format("Select * from dbo.Files Where CreatedBy={0} AND ParentFolderId={1}", uid, pid);

               using (DBHelper helper = new DBHelper())
               {
                   SqlDataReader dto = helper.ExecuteReader(sqlQuery);
                   return MYFillDTO(dto);
               }
           }

           private static List<FileDTO> MYFillDTO(SqlDataReader reader)
           {
               var list = new List<FileDTO>();
               while (reader.Read())
               {
                   var obj = new FileDTO();
                   obj.Id = reader.GetInt32(0);
                   obj.Name = reader.GetString(1);
                   obj.ParentFolderId = reader.GetInt32(2);
                   obj.FileExt = reader.GetString(3);
                   obj.FileSizeInKB = reader.GetInt32(4);
                   obj.CreatedBy = reader.GetInt32(5);
                   obj.UploadedOn = reader.GetDateTime(6);
                   obj.IsActive = reader.GetBoolean(7);
                   obj.UniqueName = reader.GetString(8);
                   obj.ContentType = reader.GetString(9);
                   list.Add(obj);
               }
               return list;
           }

           private static FileDTO FillDTO(SqlDataReader reader)
           {
               var obj = new FileDTO();
               if (reader.Read())
               {
                   obj.Id = reader.GetInt32(0);
                   obj.Name = reader.GetString(1);
                   obj.ParentFolderId = reader.GetInt32(2);
                   obj.FileExt = reader.GetString(3);
                   obj.FileSizeInKB = reader.GetInt32(4);
                   obj.CreatedBy = reader.GetInt32(5);
                   obj.UploadedOn = reader.GetDateTime(6);
                   obj.IsActive = reader.GetBoolean(7);
                   obj.UniqueName = reader.GetString(8);
                   obj.ContentType = reader.GetString(9);
               }
               return obj;
           }

           public static FileDTO GetFileByUniqueName(string uniqueName)
           {
               String sqlQuery = String.Format("Select * from dbo.Files Where UniqueName='{0}'", uniqueName);

               using (DBHelper helper = new DBHelper())
               {
                   SqlDataReader dto = helper.ExecuteReader(sqlQuery);
                   return FillDTO(dto);
               }
           }

           public static int DeleteFile(int fid)
           {
               String sqlQuery = String.Format("Delete From dbo.Files Where Id={0}", fid);

               using (DBHelper helper = new DBHelper())
               {
                   return helper.ExecuteQuery(sqlQuery);
               }
           }
    }
}
