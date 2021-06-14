using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Assets;
using WorkAppReactAPI.Configuration;
using WorkAppReactAPI.Data.Interface;
using WorkAppReactAPI.Dtos.Requests;

namespace WorkAppReactAPI.Data.SqlQuery
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly WorkerServiceContext _context;
        public NotificationRepo(WorkerServiceContext context)
        {
            _context = context;
        }
        public async Task<DynamicResult> Notifications(NotifiQuery model)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = model.phone},
                new SqlParameter("@isReaded", SqlDbType.Int) { Value = model.status },
                new SqlParameter("@start", SqlDbType.Int) { Value = model.start},
                new SqlParameter("@length", SqlDbType.Int) { Value = model.length},
                new SqlParameter("@type", SqlDbType.Int) { Value = model.type}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_GetNotifications]", parameters).JsonDataAsync();
            return result;

        }

        public async Task<DynamicResult> AddNotifications(NotificationUpdate model, UserLogin auth)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "Account already Exists", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }//    
            Guid? key;
            do
            {
                key = Guid.NewGuid();

            } while (await _context.Notifications.FirstOrDefaultAsync(x => x.Id == key) != null);

            int number = 1;
            //
            do
            {
                string code = DateTime.Now.ToString("ddMMyyhhmmss") + number;
                model.Code = "Notifi" + code;
                number++;
            } while (await _context.Notifications.FirstOrDefaultAsync(x => x.Code == model.Code) != null);
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = key},
                new SqlParameter("@Code", SqlDbType.VarChar) { Value = model.Code},
                new SqlParameter("@Title", SqlDbType.NVarChar) { Value = model.Title},
                new SqlParameter("@Content", SqlDbType.NVarChar) { Value = model.Content},
                new SqlParameter("@SendBy", SqlDbType.VarChar) { Value = user.Phone},
                new SqlParameter("@ReceiveBy", SqlDbType.VarChar) { Value = model.ReceiveBy},

                new SqlParameter("@SendByDelete", SqlDbType.Int) { Value = 0},
                new SqlParameter("@ReceiveByDelete", SqlDbType.Int) { Value = 0},

                new SqlParameter("@PostCode", SqlDbType.VarChar) { Value = model.PostCode},
                new SqlParameter("@CreateAt", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@status", SqlDbType.Int) { Value =  model.status},
                new SqlParameter("@isReaded", SqlDbType.Int) { Value =  0},
                new SqlParameter("@type", SqlDbType.Int) { Value =  model.type}

    };
            var result = await _context.ExecuteDataTable("[dbo].[sp_InsertNotification]", parameters).JsonDataAsync();
            if (result.Status == 1)
            {
                var noti = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == key);
                var resp = new List<Dictionary<string, object>>();
                resp.Add(new Dictionary<string, object>()
                {
                    ["Code"] = noti.Code,
                    ["Title"] = noti.Title,
                    ["Content"] = noti.Content,
                    ["SendBy"] = noti.SendBy,
                    ["ReceiveBy"] = noti.ReceiveBy,
                    ["CreateAt"] = noti.CreateAt,
                    ["isReaded"] = noti.isReaded,
                });
                return new DynamicResult() { Message = "Success", Data = resp, Totalrow = 0, Type = "Success", Status = 1 };
            }
            return result;
        }
        public async Task<DynamicResult> UpdateStatusNotifications(string code, UserLogin auth)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "Tài khoản không tồn tại", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var notifi = await _context.Notifications.FirstOrDefaultAsync(x => x.Code == code && (x.ReceiveBy == user.Phone || x.SendBy == user.Phone));
            if (notifi == null)
            {
                return new DynamicResult() { Message = "Không tìm thấy thông báo này", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            }
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = notifi.Id},
                new SqlParameter("@isReaded", SqlDbType.Int) { Value =  1}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_UpdateNotificationStatus]", parameters).JsonDataAsync();
            return result;
        }
        public async Task<DynamicResult> DeleteNotifications(string code, UserLogin auth)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == auth.Phone && x.Password == Encryptor.Encrypt(auth.Password));
            if (user == null)
            {
                return new DynamicResult() { Message = "Tài khoản không tồn tại", Data = null, Totalrow = 0, Type = "Error", Status = 2 };

            }
            var notifi = await _context.Notifications.FirstOrDefaultAsync(x => x.Code == code && (x.ReceiveBy == user.Phone || x.SendBy == user.Phone));
            if (notifi == null)
            {
                return new DynamicResult() { Message = "Không tìm thấy thông báo này", Data = null, Totalrow = 0, Type = "Error", Status = 2 };
            } 
            SqlParameter[] parameters ={
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Value = notifi.Id},
                new SqlParameter("@Phone", SqlDbType.VarChar) { Value = user.Phone}
            };
            var result = await _context.ExecuteDataTable("[dbo].[sp_DeleteNotification]", parameters).JsonDataAsync();
            return result;
        }
    }
}