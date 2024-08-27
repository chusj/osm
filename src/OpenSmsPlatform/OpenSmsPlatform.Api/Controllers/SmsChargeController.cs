using Microsoft.AspNetCore.Mvc;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository.UnitOfWorks;
using System.Linq.Expressions;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmsChargeController : ControllerBase
    {
        private readonly IBaseService<OspAccCharge, OspAccChargeVo> _chargeService;
        private readonly IBaseService<OspAccount, OspAccountVo> _accountService;
        private readonly IUnitOfWorkManage _unitOfWorkManage;

        public SmsChargeController(
            IBaseService<OspAccCharge, OspAccChargeVo> chargeService,
            IBaseService<OspAccount, OspAccountVo> accountService,
            IUnitOfWorkManage unitOfWorkManage)
        {
            _chargeService = chargeService;
            _accountService = accountService;
            _unitOfWorkManage = unitOfWorkManage;
        }

        /// <summary>
        /// 获取充值
        /// </summary>
        /// <param name="accId">账号id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<OspAccCharge>> GetCharge(string accId)
        {
            if (string.IsNullOrWhiteSpace(accId))
            {
                return null;
            }

            // 构建表达式
            Expression<Func<OspAccCharge, bool>> whereExpression = entity =>
                   entity.AccId == accId;

            return await _chargeService.Query(whereExpression, 10, "create_on desc");
        }

        /// <summary>
        /// 添加充值
        /// </summary>
        /// <param name="accId">账号id</param>
        /// <param name="amount">充值金额</param>
        /// <param name="remarks">充值备注，可为空</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddCharge(string accId, int amount, string remarks)
        {
            if (string.IsNullOrWhiteSpace(accId) || amount < 1)
            {
                return "param format error";
            }

            try
            {
                OspAccount account = await _accountService.QueryById(accId);
                if (account == null)
                {
                    return "param error";
                }

                OspAccCharge charge = new OspAccCharge();
                charge.AccId = accId;
                charge.Amount = amount;
                charge.Remarks = remarks;

                charge.BeforeCounts = account.AccCounts;
                charge.Counts = amount * 10;
                charge.AfterCounts = account.AccCounts + (amount * 10);
                charge.CreateOn = DateTime.Now;
                charge.CreateBy = "admin";
                charge.CreateUid = 0;

                //1.开始事务
                _unitOfWorkManage.BeginTran();

                await _chargeService.Add(charge);

                account.AccCounts = charge.AfterCounts;
                await _accountService.Update(account);

                //2.提交事务
                _unitOfWorkManage.CommitTran();

                return $"{account.AccName} 充值{amount}元成功，,充值后条数：{account.AccCounts}";
            }
            catch (Exception ex)
            {
                //3.回滚事务 => 如果忘记写这句，第2次提交出现不能正常回滚导致页面一直转圈
                _unitOfWorkManage.RollbackTran();

                return $"充值失败，发生异常{ex.ToString()}";
            }
        }
    }
}
