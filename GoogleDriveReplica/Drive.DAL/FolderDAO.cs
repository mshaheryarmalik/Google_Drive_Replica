using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.Entities;
using System.Data.SqlClient;

namespace Drive.DAL
{
    public static class FolderDAO
    {
        public static int SaveFolder(FolderDTO dto)
        {
            String sqlQuery = "";
            if (dto.Id > 0)
            {
                sqlQuery = String.Format("Update dbo.Folder Set Name='{0}',  Where UserID={1}", dto.Name, dto.Id);
            }
            else
            {
                sqlQuery = String.Format("INSERT INTO dbo.Folder(Name, ParentFolderId,CreatedBy,CreatedOn,IsActive) VALUES('{0}',{1},{2},'{3}','{4}' )",
                    dto.Name, dto.ParentFolderId, dto.CreatedBy, dto.CreatedOn, 1);
            }
            using (DBHelper helper = new DBHelper())
            {
                return helper.ExecuteQuery(sqlQuery);
            }
        }

        public static int DeleteUser(int fid)
        {
            String sqlQuery = String.Format("Update dbo.Folder Set IsActive=0 Where Id={0}", fid);

            using (DBHelper helper = new DBHelper())
            {
                return helper.ExecuteQuery(sqlQuery);
            }
        }


        public static int DeleteFolder(int id)
        {
            String sqlQuery = String.Format("Delete From dbo.Folder Where Id={0}", id);

            using (DBHelper helper = new DBHelper())
            {
                return helper.ExecuteQuery(sqlQuery);
            }
        }

        public static FolderDTO FindFolderName(String fname, int uid, int pid)
        {
            String sqlQuery = String.Format("Select * from dbo.Folder Where Name='{0}' AND CreatedBy={1} AND ParentFolderId={2}", fname,uid,pid);

            using (DBHelper helper = new DBHelper())
            {
                SqlDataReader dto = helper.ExecuteReader(sqlQuery);
                return FillDTO(dto);
            }
        }

        public static List<FolderDTO> GetAllMainFoldersByUser(int uid, int pid)
        {
            String sqlQuery = String.Format("Select * from dbo.Folder Where CreatedBy={0} AND ParentFolderId={1}", uid, pid);

            using (DBHelper helper = new DBHelper())
            {
                SqlDataReader dto = helper.ExecuteReader(sqlQuery);
                return MYFillDTO(dto);
            }
        }

        private static FolderDTO FillDTO(SqlDataReader reader)
        {
            var obj = new FolderDTO();
            if (reader.Read())
            {
                obj.Id = reader.GetInt32(0);
                obj.Name = reader.GetString(1);
                obj.ParentFolderId = reader.GetInt32(2);
                obj.CreatedBy = reader.GetInt32(3);
                obj.CreatedOn = reader.GetDateTime(4);
                obj.IsActive = reader.GetBoolean(5);
            }
            return obj;
        }

        private static List<FolderDTO> MYFillDTO(SqlDataReader reader)
        {
            var list = new List<FolderDTO>();
            while (reader.Read())
            {
                var obj = new FolderDTO();
                obj.Id = reader.GetInt32(0);
                obj.Name = reader.GetString(1);
                obj.ParentFolderId = reader.GetInt32(2);
                obj.CreatedBy = reader.GetInt32(3);
                obj.CreatedOn = reader.GetDateTime(4);
                obj.IsActive = reader.GetBoolean(5);
                list.Add(obj);
            }
            return list;
        }


        public static FolderDTO GetFolders(int uid, int pid)
        {
            String sqlQuery = String.Format("Select * from dbo.Folder Where CreatedBy={0} AND id={1}", uid, pid);

            using (DBHelper helper = new DBHelper())
            {
                SqlDataReader dto = helper.ExecuteReader(sqlQuery);
                return FillDTO(dto);
            }
        }
    }
}
