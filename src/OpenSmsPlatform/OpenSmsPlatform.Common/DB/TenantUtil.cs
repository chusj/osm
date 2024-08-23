using opensmsplatform.Model;
using opensmsplatform.Model.Tenants;
using SqlSugar;
using System.Reflection;

namespace opensmsplatform.Common.DB
{
    public static class TenantUtil
    {
        public static List<Type> GetTenantEntityTypes(TenantTypeEnum? tenantType = null)
        {
            return RepositorySetting.Entitys
                .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass)
                .Where(s => IsTenantEntity(s, tenantType))
                .ToList();
        }

        public static bool IsTenantEntity(this Type u, TenantTypeEnum? tenantType = null)
        {
            var mta = u.GetCustomAttribute<MultiTenantAttribute>();
            if (mta is null)
            {
                return false;
            }

            if (tenantType != null)
            {
                if (mta.TenantType != tenantType)
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetTenantTableName(this Type type, ISqlSugarClient db, string id)
        {
            var entityInfo = db.EntityMaintenance.GetEntityInfo(type);

            //约定大于配置，约定(多表多租户的)表名等于 => 表名_租户ID
            return $@"{entityInfo.DbTableName}_{id}";
        }

        public static void SetTenantTable(this ISqlSugarClient db, string id)
        {
            var types = GetTenantEntityTypes(TenantTypeEnum.Tables);

            foreach (var type in types)
            {
                db.MappingTables.Add(type.Name, type.GetTenantTableName(db, id));
            }
        }

        /// <summary>
        /// 获取连接字符串（分库多租户下）
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ConnectionConfig GetConnectionConfig(this SysTenant tenant)
        {
            if (tenant.DbType is null)
            {
                throw new ArgumentException("Tenant DbType Must");
            }

            return new ConnectionConfig()
            {
                ConfigId = tenant.ConfigId,
                DbType = tenant.DbType.Value,
                ConnectionString = tenant.Connection,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true,
                    SqlServerCodeFirstNvarchar = true,
                },
            };
        }
    }
}
