using System.Security.Claims;

namespace opensmsplatform.Common.HttpContextUser
{
    public interface IUser
    {
        string Name { get; }

        long ID { get; }

        /// <summary>
        /// 租户ID
        /// </summary>
        long TenantId { get; }

        /// <summary>
        /// 是否认证
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();

        IEnumerable<Claim> GetClaimsIdentity();

        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();

        List<string> GetUserInfoFromToken(string ClaimType);
    }
}
