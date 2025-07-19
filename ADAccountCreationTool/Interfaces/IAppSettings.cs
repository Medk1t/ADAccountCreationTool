using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Interfaces
{
    public interface IAppSettings
    {
        string Domain { get; }
        string NetLogonPath { get; }
        string? ExchangeServer { get; }
    }

}
