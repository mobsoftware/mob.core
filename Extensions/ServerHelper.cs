using System;
using System.Collections.ObjectModel;
using System.Web;

namespace Mob.Core.Extensions
{
    public class ServerHelper
    {
        //http://stackoverflow.com/questions/1064274/get-current-asp-net-trust-level-programmatically

        private static AspNetHostingPermissionLevel? _permissionLevel;
        public static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            if (_permissionLevel.HasValue)
                return _permissionLevel.Value;

            foreach (var trustLevel in
                    new[] {
            AspNetHostingPermissionLevel.Unrestricted,
            AspNetHostingPermissionLevel.High,
            AspNetHostingPermissionLevel.Medium,
            AspNetHostingPermissionLevel.Low,
            AspNetHostingPermissionLevel.Minimal 
        })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                    _permissionLevel = trustLevel;
                    return _permissionLevel.Value;
                }
                catch (System.Security.SecurityException)
                {
                    continue;
                }
                
            }

            return AspNetHostingPermissionLevel.None;
        }

        /// <summary>
        /// Maps a relative path to local path on file system
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetLocalPathFromRelativePath(string relativePath)
        {
            return HttpContext.Current.Server.MapPath(relativePath);
        }

        /// <summary>
        /// Maps a file system path to relative path
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string GetRelativePathFromLocalPath(string localPath)
        {
            var appPath = HttpContext.Current.Server.MapPath("~");
            var res = $"~{localPath.Replace(appPath, "").Replace("\\", "/")}";
            return res;
        }
        /// <summary>
        /// Gets available timezones on the server
        /// </summary>
        public static ReadOnlyCollection<TimeZoneInfo> GetAvailableTimezones()
        {
            var availableTimezones = TimeZoneInfo.GetSystemTimeZones();
            return availableTimezones;
        }
    }
}