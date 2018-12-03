using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Configuration.Model
{
    /// <summary>
    /// 是否存在的实体,缓存使用
    /// </summary>
    public class ClientRoleModel
    {
        /// <summary>
        /// 缓存时间
        /// </summary>
        public DateTime CacheTime { get; set; }
        /// <summary>
        /// 是否有访问权限
        /// </summary>
        public bool Role { get; set; }
    }
}
