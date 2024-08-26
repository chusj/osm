using OpenSmsPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSmsPlatform.IService
{
    public interface IOspLimitService
    {
        /// <summary>
        /// 是否在限制名单中
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<OspLimit> IsInLimitList(string mobile);
    }
}
