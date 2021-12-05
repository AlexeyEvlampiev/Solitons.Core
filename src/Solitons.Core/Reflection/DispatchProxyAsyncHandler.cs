using System.Threading.Tasks;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public delegate Task<object> DispatchProxyAsyncHandler(object[] parameters);


}
