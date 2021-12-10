using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public interface IRestApi2
    {
        IWebResponse ProcessAsync(IWebRequest request, CancellationToken cancellationToken);
    }
}
