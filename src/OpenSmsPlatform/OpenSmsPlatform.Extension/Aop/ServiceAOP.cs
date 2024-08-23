using Castle.DynamicProxy;
using Newtonsoft.Json;
using opensmsplatform.Common;
using System.Reflection;

namespace opensmsplatform.Extension
{
    /// <summary>
    /// 拦截器AOP，继承自IInterceptor接口
    /// </summary>
    public class ServiceAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            string json = string.Empty;
            try
            {
                json = JsonConvert.SerializeObject(invocation.Arguments);
            }
            catch (Exception ex)
            {
                json = "无法序列化，" + ex.Message;
            }

            DateTime startDateTime = DateTime.Now;
            AOPLogInfo logInfo = new AOPLogInfo()
            {
                RequestTime = startDateTime.ToString("yyyy-MM-dd HH:mm:ss fff"),
                OpUserName = string.Empty,
                RequestMethodName = invocation.Method.Name,
                RequestParamsName = string.Join(",", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()),
                RequestParamsData = json
            };

            try
            {
                //在被拦截的方法执行完毕后，执行执行当前方法，注意被拦截的是异步的
                invocation.Proceed();

                //异步获取异常，先执行
                if (IsAsyncMethod(invocation.Method))
                {
                    if (invocation.Method.ReturnType == typeof(Task))
                    {
                        invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                            (Task)invocation.ReturnValue,
                            async () => await SuccessAction(invocation, logInfo, startDateTime),
                            ex =>
                            {
                                LogEx(ex, logInfo);
                            });
                    }
                    else
                    {
                        invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyGetResult(
                            invocation.Method.ReturnType.GenericTypeArguments[0],
                            invocation.ReturnValue,
                            async (o) => await SuccessAction(invocation, logInfo, startDateTime, o),
                            ex =>
                            {
                                LogEx(ex, logInfo);
                            });
                    }
                }
                else  //同步
                {
                    string jsonResult;
                    try
                    {
                        jsonResult = JsonConvert.SerializeObject(invocation.Arguments);
                    }
                    catch (Exception ex)
                    {
                        jsonResult = "无法序列化，可能是兰姆达表达式等原因造成，按照框架优化代码" + ex.ToString();
                    }

                    DateTime endTime = DateTime.Now;
                    string repsonseTime = (endTime - startDateTime).Milliseconds.ToString();
                    logInfo.ResponseTime = endTime.ToString("yyyy-MM-dd HH:mm:ss fff");
                    logInfo.ResponseIntervalTime = repsonseTime + "ms";
                    logInfo.ResponseJsonData = jsonResult;
                    Console.WriteLine(JsonConvert.SerializeObject(logInfo));
                }
            }
            catch (Exception ex)
            {
                LogEx(ex, logInfo);
                throw;
            }
        }

        private async Task SuccessAction(IInvocation invocation, AOPLogInfo logInfo, DateTime startDateTime, object o = null)
        {
            DateTime endDateTime = DateTime.Now;
            string responseTime = (startDateTime - endDateTime).Milliseconds.ToString();
            logInfo.ResponseTime = endDateTime.ToString("yyyy-MM-dd HH:mm:ss fff");
            logInfo.ResponseIntervalTime = responseTime + "ms";
            logInfo.ResponseJsonData = JsonConvert.SerializeObject(o);

            await Task.Run(() =>
            {
                //控制输出请求成功的日志
                //Console.WriteLine("执行成功 => " + JsonConvert.SerializeObject(logInfo));
            });
        }


        private void LogEx(Exception ex, AOPLogInfo logInfo)
        {
            if (ex != null)
            {
                Console.WriteLine("Eror !!!" + ex.Message);

                //控制输出请求异常时的日志
                //Console.WriteLine(JsonConvert.SerializeObject(logInfo));
            }
        }

        /// <summary>
        /// 是否为异步方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return
                method.ReturnType == typeof(Task) ||
                method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }

        internal static class InternalAsyncHelper
        {
            public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
            {
                Exception exception = null;

                try
                {
                    await actualReturnValue;
                    await postAction();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    finalAction(exception);
                }
            }

            public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetReuslt<T>(Task<T> actualReturnValue, Func<object, Task> postAction,
                Action<Exception> finalAction)
            {
                Exception exception = null;
                try
                {
                    var result = await actualReturnValue;
                    await postAction(result);
                    return result;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    throw;
                }
                finally
                {
                    finalAction(exception);
                }
            }

            public static object CallAwaitTaskWithPostActionAndFinallyGetResult(Type taskReturnType, object actualReturnValue,
                Func<object, Task> action, Action<Exception> finalAction)
            {
                return typeof(InternalAsyncHelper)
                    .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetReuslt", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(taskReturnType)
                    .Invoke(null, new object[] { actualReturnValue, action, finalAction });
            }
        }
    }
}
